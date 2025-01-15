using UnityEngine;
using UnityEngine.AI;

using Unity.Netcode.Components;

namespace BossRush.FiniteStateMachine.Entities
{
    /// <summary>
    /// Base class for all the bosses in the project.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class BossEntity : Entity
    {
        [field: Header("Targeting Properties")]
        [field: SerializeField, Tooltip("A reference to the current target's transform.")]
        private NetworkTransform _targetTransform;

        protected override void OnEnable()
        {
            Initialize();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Get all related components
            Transform = GetComponent<NetworkTransform>();

            Body = GetComponent<NetworkRigidbody>();
            Collider = GetComponentInChildren<Collider>();
            NavAgent = GetComponent<NavMeshAgent>();

            // We assume that the Animator is attached to the mesh
            Animator = GetComponentInChildren<NetworkAnimator>();
        }
#endif

        public override void Initialize()
        {
            // We call base to initialize the blackboard references
            base.Initialize();

            // Get the component if it wasn't already applied
            if (NavAgent == null)
                NavAgent = GetComponent<NavMeshAgent>();

            // Set the navmesh agent for the machine to use for boss specific behaviors
            Machine.SetNavMeshAgent(NavAgent);
        }

        /// <summary>
        /// Set a new target for the boss to follow.
        /// </summary>
        /// <param name="newTarget"></param>
        public void SetNewTarget(NetworkTransform newTarget)
        {
            _targetTransform = newTarget;
        }
    }
}
