using BossRush.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace BossRush.FiniteStateMachine.Entities
{
    /// <summary>
    /// The base Entity class, this is the parent of all entities.
    /// </summary>
    /// <remarks>
    /// Requires the component <see cref="Rigidbody"/>.
    /// </remarks>
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Entity : MonoBehaviour, IDamageable
    {
        [field: Header("Standard Entity Component Attributes")]
        [field: SerializeField] public Transform Transform { get; set; }
        [field: SerializeField] public Rigidbody Body { get; set; }
        [field: SerializeField] public Collider Collider { get; set; }
        [field: SerializeField] public NavMeshAgent NavAgent { get; set; }
        [field: SerializeField] public Animator Animator { get; protected set; }

        // finite state machine attributes
        protected BlackboardReference BlackboardRef;
        public StateMachine Machine;

        // TODO: Standard behaviors


        // virtual standard unity events
        protected virtual void Start() { Initialize(); }
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
            Machine = new StateMachine(this, BlackboardRef);
        }

        //  Handles the taking dmg for the entity.
        public virtual void TakeDamageServerRpc(float damage)
        {
            //  TODO: Implement actual logic for taking dmg.
            Debug.Log("Did dmg");
        }
    }
}
