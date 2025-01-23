using UnityEngine;
using Unity.Netcode;

namespace BossRush.Utility
{
    public interface IDamageable
    {
        [ServerRpc(RequireOwnership = false)]
        public void TakeDamageServerRpc(float damage);
    }
}
