using BossRush.FiniteStateMachine.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace BossRush.FiniteStateMachine
{
    /// <summary>
    /// Structure containing common blackboard component references.
    /// </summary>
    public struct BlackboardReference
    {
        public Transform Transform { get; set; }
        public Rigidbody Rigidbody { get; set; }
        public NavMeshAgent NavMeshAgent { get; set; }
        public Animator Animator { get; set; }
        public Collider Collider { get; set; }
    }

    /// <summary>
    /// Manages state transitions for an entity in the game.
    /// This class handles the core functionality of the Finite State Machine.
    /// </summary>
    public class StateMachine
    {
        private IState _currentState;
        private IState _previousState;

        // Cached components
        private BlackboardReference _blackboardReference;
        private readonly Entity _owner;

        /// <summary>
        /// Constructor to initialize the blackboard references from the owning entity.
        /// </summary>
        /// <param name="owner">The entity that owns this state machine.</param>
        /// <param name="componentRef">The component ref that got initialized from said entity.</param>
        public StateMachine(Entity owner, BlackboardReference componentRef)
        {
            // set the blackboard references to cache them
            _blackboardReference = componentRef;

            _blackboardReference.Transform = owner.transform;
            _blackboardReference.Rigidbody = owner.GetComponent<Rigidbody>();
            _blackboardReference.Animator = owner.GetComponent<Animator>();
            _blackboardReference.Collider = owner.GetComponentInParent<Collider>();
            _owner = owner;
        }

        public void Update()
        {
            _currentState?.Update();
        }

        public void FixedUpdate()
        {
            _currentState?.FixedUpdate();
        }

        #region StateMachine Setters

        public void SetState(IState newState)
        {
            if (newState == _currentState) return;

            // exit and enter the new state
            _previousState = _currentState;

            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void SetNavMeshAgent(NavMeshAgent agent)
        {
            _blackboardReference.NavMeshAgent = agent;
        }

        #endregion

        #region StateMachine Getters

        public BlackboardReference GetBlackboardReference() => _blackboardReference;
        public Entity GetEntity() => _owner;
        public IState GetCurrentState() => _currentState;

        #endregion

        public void ResetToPreviousState()
        {
            if (_previousState != null)
                SetState(_previousState);
        }
    }
}
