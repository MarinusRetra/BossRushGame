#region System namespace
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

using UnityEngine;

#region Unity Services namespace
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
#endregion

#region Unity Netcode namespaces
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
#endregion

using BossRush.Utility;

// TODO: Remove when game is finished.
using Random = UnityEngine.Random;

namespace BossRush.Multiplayer
{
    /// <summary>
    /// 
    /// </summary>
    public class MultiplayerManager : Singleton<MultiplayerManager>
    {
        [Tooltip("Key used to store and retrieve the relay join code for the lobby.")]
        private const string KEY_RELAY_JOIN_CODE = "RelayJoinCode";
        [Tooltip("Maximum number of players allowed in a single lobby.")]
        private const int MAX_PLAYER_AMOUNT = 10;

        [Tooltip("Reference to the current lobby the player has joined.")]
        private Lobby _joinedLobby;
        [Tooltip("Interval (in seconds) between heartbeat pings to keep the lobby active.")]
        private float _heartbeatTimer;
        [Tooltip("Interval (in seconds) between refreshes of the lobby list.")]
        private float _listLobbiesTimer;

        protected override void Awake()
        {
            base.Awake();

            _ = InitializeUnityAuthentication();
        }

        private void Update()
        {
            HandleHeartbeat();
            // HandlePeriodicListLobbies();
            // Uncomment if we are adding a lobby list.
        }

        private void HandlePeriodicListLobbies()
        {
            if (_joinedLobby != null || !AuthenticationService.Instance.IsSignedIn) return;

            _listLobbiesTimer -= Time.deltaTime;

            if (!(_listLobbiesTimer < 0)) return;
            const float LIST_LOBBY_TIMER_MAX = 3f;
            _listLobbiesTimer = LIST_LOBBY_TIMER_MAX;

            ListLobbies();
        }

        /// <summary>
        /// Sends a periodic heartbeat to keep the lobby active, if the player is the host.
        /// </summary>
        private void HandleHeartbeat()
        {
            if (!IsLobbyHost()) return;
            _heartbeatTimer -= Time.deltaTime;

            if (!(_heartbeatTimer < 0)) return;
            const float HEARTBEAT_TIMER_MAX = 15f;
            _heartbeatTimer = HEARTBEAT_TIMER_MAX;

            LobbyService.Instance.SendHeartbeatPingAsync(_joinedLobby.Id);
        }

        /// <summary>
        /// Determines whether the current player is the host of the joined lobby.
        /// </summary>
        /// <returns>True if the player is the lobby host; otherwise, false.</returns>
        private bool IsLobbyHost() => _joinedLobby != null && _joinedLobby.HostId == AuthenticationService.Instance.PlayerId;

        /// <summary>
        /// Checks if there are any active lobbies with at least one available slot.
        /// </summary>
        /// <returns>True if there are lobbies available to join; otherwise, false.</returns>
        private static async Task<bool> AreLobbiesAvailable()
        {
            try
            {
                var queryLobbiesOptions = new QueryLobbiesOptions
                {
                    Filters = new List<QueryFilter>
                    {
                        new(QueryFilter.FieldOptions.AvailableSlots, "1", QueryFilter.OpOptions.GE)
                    }
                };


                var queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);

                return queryResponse.Results.Count > 0;
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);

                return false;
            }
        }

        // TODO: Make this static when game is finished.
        /// <summary>
        /// Initializes Unity services and signs the player in anonymously.
        /// </summary>
        private async Task InitializeUnityAuthentication()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                var initializationOptions = new InitializationOptions();
                // TODO: Remove the random when game is finished.
                initializationOptions.SetProfile(Random.Range(0, 10000).ToString());

                await UnityServices.InitializeAsync(initializationOptions);

                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                Debug.Log("User signed in");
            }
        }

        private static async void ListLobbies()
        {
            try
            {
                var queryLobbiesOptions = new QueryLobbiesOptions { 
                    Filters = new List<QueryFilter>
                    {
                        new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "1", QueryFilter.OpOptions.GE)
                    }
                };
                var queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
                // TODO: add an event below here to make it visible on the UI. . .
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async Task CreateLobby(string lobbyName)
        {
            try
            {
                Debug.Log("Starting lobby . . .");

                _joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, MAX_PLAYER_AMOUNT, new CreateLobbyOptions
                {
                    IsPrivate = false
                });

                var allocation = await AllocateRelay();

                string joinCode = await GetRelayJoinCode(allocation);

                await LobbyService.Instance.UpdateLobbyAsync(_joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject> {
                        {
                            KEY_RELAY_JOIN_CODE,
                            new DataObject(DataObject.VisibilityOptions.Member, joinCode)
                        }
                    }
                });


                RelayServerData relayServerData = allocation.ToRelayServerData("dtls");

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
                    _joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync(new QuickJoinLobbyOptions
                    {
                        Filter = new List<QueryFilter>
                        {
                            new(QueryFilter.FieldOptions.AvailableSlots, "1", QueryFilter.OpOptions.GE)
                        },
                    });
                }

                if (_joinedLobby == null) await CreateLobby("New Lobby");

                Debug.Log(IsLobbyHost() ? "Player is host" : "Player is client");

                if (IsLobbyHost()) return;
                string relayJoinCode = _joinedLobby?.Data[KEY_RELAY_JOIN_CODE].Value;

                print(relayJoinCode);

                var joinAllocation = await JoinRelayByCode(relayJoinCode);

                if (joinAllocation == null)
                {
                    Debug.LogError("JoinAllocation is null.");
                    return;
                }

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                    joinAllocation.ToRelayServerData("dtls"));

                NetworkManager.Singleton.StartClient();

                Debug.Log("Joined");
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }


        public async void JoinWithCode(string lobbyCode)
        {
            try
            {
                _joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

                NetworkManager.Singleton.StartClient(); 
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);
            }
            catch (Exception e)
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
                if (_joinedLobby == null) 
                {
                    Debug.LogError("JoinedLobby is null");
                    return;
                }
                await LobbyService.Instance.DeleteLobbyAsync(_joinedLobby.Id);

                _joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);
            }
            catch (Exception e)
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
                if (_joinedLobby == null)
                {
                    Debug.LogError("JoinedLobby is null");
                    return;
                }
                await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id,
                    AuthenticationService.Instance.PlayerId);

                _joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);
            }
            catch (Exception e)
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
            try
            {
                if (!IsLobbyHost()) return;
                try
                {
                    await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, playerId);
                }
                catch (LobbyServiceException e)
                {
                    Debug.LogException(e);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Creates a relay allocation for hosting the game session.
        /// </summary>
        /// <returns>An allocation object representing the relay session.</returns>
        private static async Task<Allocation> AllocateRelay()
        {
            try
            {
                return await RelayService.Instance.CreateAllocationAsync(MAX_PLAYER_AMOUNT - 1);
            }
            catch (RelayServiceException e)
            {
                Debug.LogException(e);

                return null;
            }
        }


        /// <summary>
        /// Generates a join code for the specified relay allocation.
        /// </summary>
        /// <param name="allocation">The relay allocation for which to generate the join code.</param>
        /// <returns>A string containing the relay join code.</returns>
        private static async Task<string> GetRelayJoinCode(Allocation allocation)
        {
            try
            {
                return await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            }
            catch (RelayServiceException e)
            {
                Debug.LogException(e);

                return null;
            }
        }


        /// <summary>
        /// Joins a relay session using the specified join code.
        /// </summary>
        /// <param name="joinCode">The join code for the relay session.</param>
        /// <returns>A JoinAllocation object for the joined relay session.</returns>
        private static async Task<JoinAllocation> JoinRelayByCode(string joinCode)
        {
            try
            {
                return await RelayService.Instance.JoinAllocationAsync(joinCode);
            }
            catch (RelayServiceException e)
            {
                Debug.LogException(e);
                return null;
            }
        }
    }
}
