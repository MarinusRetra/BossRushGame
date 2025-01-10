using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace BossRush
{
    public class ReloadCurrentScene : NetworkBehaviour
    {
        [ServerRpc]
        public void ReloadSceneServerRpc()
        {
            for (int i = NetworkManager.SpawnManager.SpawnedObjects.Count;
                i > 0; i--)
            {
                if (!NetworkManager.SpawnManager.SpawnedObjects[(ulong)i].IsPlayerObject)
                {
                    NetworkManager.SpawnManager.SpawnedObjects[(ulong)i].Despawn();
                }
            }

            Scene scene = SceneManager.GetActiveScene();
            NetworkManager.SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
        }
    }
}
