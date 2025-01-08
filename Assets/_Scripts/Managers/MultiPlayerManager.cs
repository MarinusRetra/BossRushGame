using UnityEngine;
using BossRush.Utility;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Networking.Transport.Relay;
using Unity.Netcode.Transports.UTP;

namespace BossRush
{
    public class MultiPlayerManager : Singleton<MultiPlayerManager>
    {
        [Tooltip("Key used to store and retrieve the relay join code for the lobby.")]
        private const string KEY_RELAY_JOIN_CODE = "RelayJoinCode";
        [Tooltip("Maximum number of players allowed in a single lobby.")]
        private const int MAX_PLAYER_AMOUNT = 10;

        [Tooltip("Reference to the current lobby the player has joined.")]
        private Lobby joinedLobby;
        [Tooltip("Interval (in seconds) between heartbeat pings to keep the lobby active.")]
        private float heartbeatTimer;
        [Tooltip("Interval (in seconds) between refreses of the lobby list.")]
        float listLobbiesTimer;

        protected override void Awake()
        {
            base.Awake();

            InitializeUnityAuthenication();
        }

        private void Update()
        {
            HandleHeartbeat();
            // HandlePeriodicListLobbies();
            // Uncomment if we are adding a lobby list.
        }

        private void HandlePeriodicListLobbies()
        {
            if (joinedLobby != null || !AuthenticationService.Instance.IsSignedIn) return;

            listLobbiesTimer -= Time.deltaTime;

            if (listLobbiesTimer < 0)
            {
                float listLobbiesTimerMax = 3f;
                listLobbiesTimer = listLobbiesTimerMax;

                ListLobbies();
            }
        }

        /// <summary>
        /// Sends a periodic heartbeat to keep the lobby active, if the player is the host.
        /// </summary>
        private void HandleHeartbeat()
        {
            if (IsLobbyHost())
            {
                heartbeatTimer -= Time.deltaTime;

                if (heartbeatTimer < 0)
                {
                    float heartbeatTimerMax = 15f;
                    heartbeatTimer = heartbeatTimerMax;

                    LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
                }
            }
        }

        /// <summary>
        /// Determines whether the current player is the host of the joined lobby.
        /// </summary>
        /// <returns>True if the player is the lobby host; otherwise, false.</returns>
        private bool IsLobbyHost() => joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;

        /// <summary>
        /// Checks if there are any active lobbies with at least one available slot.
        /// </summary>
        /// <returns>True if there are lobbies available to join; otherwise, false.</returns>
        private async Task<bool> AreLobbiesAvailable()
        {
            try
            {
                QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
                {
                    Filters = new List<QueryFilter>
                    {
                        new(QueryFilter.FieldOptions.AvailableSlots, "1", QueryFilter.OpOptions.GE)
                    }
                };


                QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);

                return queryResponse.Results.Count > 0;
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);

                return false;
            }
        }

        /// <summary>
        /// Initializes Unity services and signs the player in anonymously.
        /// </summary>
        private async void InitializeUnityAuthenication()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                InitializationOptions initializationOptions = new InitializationOptions();
                initializationOptions.SetProfile(Random.Range(0, 10000).ToString());

                await UnityServices.InitializeAsync(initializationOptions);

                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                Debug.Log("User signed in");
            }
        }

        private async void ListLobbies()
        {
            try
            {
                QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions { 
                    Filters = new List<QueryFilter>
                    {
                        new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "1", QueryFilter.OpOptions.GE)
                    }
                };
                QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
                // add a event below here to make it visible on the UI. . .
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);
            }
        }

        public async Task CreateLobby(string lobbyName)
        {
            try
            {
                Debug.Log("Starting lobby . . .");

                joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, MAX_PLAYER_AMOUNT, new CreateLobbyOptions
                {
                    IsPrivate = false
                });

                Allocation allocation = await AllocateRelay();

                string joinCode = await GetRelayJoinCode(allocation);

                await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject> {
                        {
                            KEY_RELAY_JOIN_CODE,
                            new DataObject(DataObject.VisibilityOptions.Member, joinCode)
                        }
                    }
                });


                RelayServerData relayServerData = AllocationUtils.ToRelayServerData(allocation, "dtls");

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

                NetworkManager.Singleton.StartHost();

                Debug.Log("Hosting");
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);
            }
        }

        public async void QuickJoin()
        {
            try
            {
                Debug.Log("Joining . . .");

                if (await AreLobbiesAvailable())
                {
                    joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync(new QuickJoinLobbyOptions
                    {
                        Filter = new List<QueryFilter>
                        {
                            new(QueryFilter.FieldOptions.AvailableSlots, "1", QueryFilter.OpOptions.GE)
                        },
                    });
                }

                if (joinedLobby == null) await CreateLobby("New Lobby");

                Debug.Log(IsLobbyHost() ? "Player is host" : "Player is client");

                if (!IsLobbyHost())
                {
                    string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;

                    print(relayJoinCode);

                    JoinAllocation joinAllocation = await JoinRelayByCode(relayJoinCode);

                    if (joinAllocation == null)
                    {
                        Debug.LogError("JoinAllocation is null.");
                        return;
                    }

                    NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                        AllocationUtils.ToRelayServerData(joinAllocation, "dtls"));

                    NetworkManager.Singleton.StartClient();

                    Debug.Log("Joined");
                }
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);
            }
        }


        public async void JoinWithCode(string lobbyCode)
        {
            try
            {
                joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

                NetworkManager.Singleton.StartClient(); 
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);
            }

        }

        /// <summary>
        /// Deletes the currently joined lobby.
        /// </summary>
        public async void DeleteLobby()
        {
            try
            {
                await LobbyService.Instance?.DeleteLobbyAsync(joinedLobby.Id);

                joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Makes the player leave the current lobby but not the relay session.
        /// </summary>
        public async void LeaveLobby()
        {
            try
            {
                await LobbyService.Instance?.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Removes a player from the lobby (host only).
        /// </summary>
        /// <param name="playerId">The ID of the player to kick from the lobby.</param>
        public async void KickPlayer(string playerId)
        {
            if (IsLobbyHost())
            {
                try
                {
                    await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
                }
                catch (LobbyServiceException e)
                {
                    Debug.LogException(e);
                }
            }
        }

        /// <summary>
        /// Creates a relay allocation for hosting the game session.
        /// </summary>
        /// <returns>An allocation object representing the relay session.</returns>
        private async Task<Allocation> AllocateRelay()
        {
            try
            {
                return await RelayService.Instance.CreateAllocationAsync(MAX_PLAYER_AMOUNT - 1);
            }
            catch (RelayServiceException e)
            {
                Debug.LogException(e);

                return default;
            }
        }


        /// <summary>
        /// Generates a join code for the specified relay allocation.
        /// </summary>
        /// <param name="allocation">The relay allocation for which to generate the join code.</param>
        /// <returns>A string containing the relay join code.</returns>
        private async Task<string> GetRelayJoinCode(Allocation allocation)
        {
            try
            {
                return await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            }
            catch (RelayServiceException e)
            {
                Debug.LogException(e);

                return default;
            }
        }


        /// <summary>
        /// Joins a relay session using the specified join code.
        /// </summary>
        /// <param name="joinCode">The join code for the relay session.</param>
        /// <returns>A JoinAllocation object for the joined relay session.</returns>
        private async Task<JoinAllocation> JoinRelayByCode(string joinCode)
        {
            try
            {
                return await RelayService.Instance.JoinAllocationAsync(joinCode);
            }
            catch (RelayServiceException e)
            {
                Debug.LogException(e);
                return default;
            }
        }
    }
}
