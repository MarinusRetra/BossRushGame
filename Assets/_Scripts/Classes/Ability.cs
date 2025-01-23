using System;
using System.Collections;
using BossRush.Classes.DataObj;
using Unity.Netcode;
using UnityEngine;

namespace BossRush.Classes.Abilities
{
    [Serializable]
    public struct EntityAbilityData
    {
        public Animator AnimatorData;
        public Rigidbody Rb;
        public float DmgMulti;
        public float AttackSpeedMulti;
        public bool HasTakenDmg;
        public float Timer;

        public EntityAbilityData(Animator animatorData, Rigidbody rbData)
        {
            AnimatorData = animatorData;
            Rb = rbData;
            AttackSpeedMulti = 1f;
            DmgMulti = 1f;
            HasTakenDmg = false;
            Timer = 0f;
        }
    }

    /// <summary>
    /// Base class for defining abilities in the Boss Rush game. Abilities are networked behaviors that can be executed with animations and damage.
    /// </summary>
    public abstract class Ability : NetworkBehaviour
    {
        /// <summary>
        /// Executes the ability with the specified animation and base damage.
        /// </summary>
        /// <param name="entityAbilityData">Hold data of the entity that uses it.</param>
        /// <param name="baseDmg">The base damage value of the ability.</param>
        /// <param name="entityTransform">The Transform of the entity using the ability.</param>
        /// <param name="callBack">Callback invoked when the ability execution is complete. Passes a boolean indicating
        /// whether the ability has finished executing.</param>
        /// <returns>An IEnumerator for coroutine-based execution.</returns>
        public abstract IEnumerator Do(EntityAbilityData entityAbilityData, AbilityData abilityData, float baseDmg,
            Transform entityTransform, System.Action<bool> callBack = null);
    }
}
