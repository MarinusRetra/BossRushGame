using System;
using BossRush.Classes.Data;
using BossRush.FiniteStateMachine;
using BossRush.FiniteStateMachine.Behaviors;
using Unity.Netcode;
using UnityEngine;

namespace BossRush.Classes
{
    [Serializable]
    public abstract class PlayableClass : NetworkBehaviour
    {
        [Tooltip("Holds the data for the class")]
        [SerializeField] protected PlayableClassData Data;

        [Tooltip("The level of the class")] 
        public int Level { get; protected set; } = 0;

        [Tooltip("The current health the player/class has")]
        protected float Health;

        [Tooltip("The primary ability of the class")]
        public State PrimaryAbility;

        [Tooltip("The secondary ability of the class")]
        public State SecondaryAbility;

        [Tooltip("The tertiary ability of the class")]
        public State TertiaryAbility;

        [Tooltip("the quaternary ability of the class")]
        public State QuaternaryAbility;

        /// <summary>
        /// Add each of the abilities in the constructor.
        /// After that make a new instance for each state with the correct ability.
        /// </summary>
        /// <param name="stateMachine"></param>
        protected PlayableClass(StateMachine stateMachine)
        {
        }

        /// <summary>
        /// Increases the level of the class.
        /// </summary>
        public void IncreaseLevel() => Level++;
    }
}
