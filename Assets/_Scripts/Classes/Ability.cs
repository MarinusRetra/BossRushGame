using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace BossRush.Classes.Abilities
{
    /// <summary>
    /// Base class for defining abilities in the Boss Rush game. Abilities are networked behaviors that can be executed with animations and damage.
    /// </summary>
    public abstract class Ability : NetworkBehaviour
    {
        /// <summary>
        /// Executes the ability with the specified animation and base damage.
        /// </summary>
        /// <param name="animation">The animation clip to play during the ability.</param>
        /// <param name="baseDmg">The base damage value of the ability.</param>
        /// <param name="callBack">Callback invoked when the ability execution is complete. Passes a boolean indicating whether the ability has finished executing.</param>
        /// <returns>An IEnumerator for coroutine-based execution.</returns>
        public abstract IEnumerator Do(AnimationClip animation, float baseDmg, System.Action<bool> callBack);
    }
}
