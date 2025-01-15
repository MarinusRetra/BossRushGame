using UnityEngine;
using UnityEngine.AI;

using Unity.Netcode.Components;

namespace BossRush.FiniteStateMachine.Entities
{
    /// <summary>
    /// The base Entity class, this is the parent of all entities.
    /// </summary>
    /// <remarks>
    /// Requires the component <see cref="Rigidbody"/>.
    /// </remarks>
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Entity : MonoBehaviour
    {
        [field: Header("Standard Entity Component Attributes")]
        [field: SerializeField] public NetworkTransform Transform { get; set; }
        [field: SerializeField] public NetworkRigidbody Body { get; set; }
        [field: SerializeField] public Collider Collider { get; set; }
        [field: SerializeField] public NavMeshAgent NavAgent { get; set; }
        [field: SerializeField] public NetworkAnimator Animator { get; protected set; }

        // finite state machine attributes
        protected BlackboardReference BlackboardRef;
        public StateMachine Machine;

        // virtual standard unity events
        protected virtual void Start() {  }
        protected virtual void Update() { Machine.Update(); }
        protected virtual void FixedUpdate() { Machine.FixedUpdate(); }

        // virtual event registration unity events
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        protected virtual void OnDestroy() { }

        public virtual void Initialize()
        {
            // Set all the component references with the entities'
            BlackboardRef = new BlackboardReference
            {
                Transform = Transform,
                Animator = Animator,
                Collider = Collider,
                NavMeshAgent = NavAgent,
                Rigidbody = Body
            };

            // After that initialize the StateMachine
            // Note: State Machine retrieves the component from the owner(Entity).
            Machine = new StateMachine(this, BlackboardRef);
        }
    }
}
