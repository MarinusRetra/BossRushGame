using UnityEngine;
using UnityEngine.AI;

namespace BossRush.FiniteStateMachine.Entities
{
    /// <summary>
    /// Base class for all the bosses in the project.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class BossEntity : Entity
    {
        protected override void OnEnable()
        {
            Initialize();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Get all related components
            Transform = transform;

            Body = GetComponent<Rigidbody>();
            Collider = GetComponentInChildren<Collider>();
            NavAgent = GetComponent<NavMeshAgent>();

            // We assume that the Animator is attached to the mesh
            Animator = GetComponentInChildren<Animator>();
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
    }
}
