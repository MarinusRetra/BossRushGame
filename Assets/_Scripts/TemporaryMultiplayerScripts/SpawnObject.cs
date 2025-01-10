using UnityEngine;
using Unity.Netcode;

namespace BossRush
{
    public class SpawnObject : NetworkBehaviour
    {
        [SerializeField] private GameObject obj;

        [ServerRpc(RequireOwnership = false)]
        public void SpawnObjServerRpc()
        {
            GameObject spawnedObj = Instantiate(obj, transform);
            
            if (spawnedObj.TryGetComponent(out NetworkObject networkObject))
            {
                networkObject.Spawn();
            }
        }
    }
}
