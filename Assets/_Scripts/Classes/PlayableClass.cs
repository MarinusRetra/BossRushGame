using System;
using BossRush.Classes.DataObj;
using BossRush.Classes.Abilities;
using Unity.Netcode;
using UnityEngine;

namespace BossRush.Classes
{
    [Serializable]
    public abstract class PlayableClass : MonoBehaviour
    {
        [Tooltip("The data object containing configuration and stats for this playable class.")]
        [SerializeField] public PlayableClassData Data { get; protected set; }

        [Tooltip("The current level of this playable class instance.")]
        public int Level { get; protected set; } = 0;

        [Tooltip("The primary ability of the class.")]
        public Ability PrimaryAbility;

        [Tooltip("The secondary ability of the class.")]
        public Ability SecondaryAbility;

        [Tooltip("The tertiary ability of the class.")]
        public Ability TertiaryAbility;

        [Tooltip("The quaternary ability of the class.")]
        public Ability QuaternaryAbility;

        /// <summary>
        /// Add all instances for the abilities here keep in mind the class
        /// </summary>
        protected abstract void Awake();

        /// <summary>
        /// Increases the level of the class by one.
        /// </summary>
        public void IncreaseLevel() => Level++;
    }
}