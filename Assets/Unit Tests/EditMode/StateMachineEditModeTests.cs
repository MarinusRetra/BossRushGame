using BossRush.FiniteStateMachine;
using BossRush.FiniteStateMachine.Behaviors;
using BossRush.FiniteStateMachine.Entities;
using NUnit.Framework;
using System;
using UnityEngine;

namespace BossRush.UnitTests.EditMode
{
    public class StateMachineEditModeTests
    {
        public class TestIdleState : State
        {
            public TestIdleState(StateMachine machine) : base(machine)
            { }

            public override void Enter()
            {
                Debug.Log($"{Machine.GetOwner().name} - Entered Idle state");
            }
        }

        public class TestMoveState : State
        {
            public TestMoveState(StateMachine machine) : base(machine)
            { }

            public override void Enter()
            {
                Debug.Log($"{Machine.GetOwner().name} - Entered Move state");
            }
        }

        public class TestAttackState : State
        {
            public TestAttackState(StateMachine machine) : base(machine)
            {
                Debug.Log($"Constructed {this} with parent class {typeof(State)}");
            }

            public override void Enter()
            {
                Debug.Log("Slashing!");
            }
        }

        public class TestRangedAttackState : TestAttackState
        {
            public TestRangedAttackState(StateMachine machine) : base(machine)
            {
                Debug.Log($"Constructed {this} with parent class {typeof(TestAttackState)}");
            }

            public override void Enter()
            {
                Debug.Log("Shooting an arrow!");
            }
        }

        public class TestEntity : Entity
        {
            public TestIdleState idleState;
            public TestMoveState moveState;
            public TestAttackState slashState;
            public TestAttackState rangedAttackState;

            public override void Initialize()
            {
                base.Initialize();

                Debug.Log($"Called {name}'s Initialize()");

                idleState = new TestIdleState(Machine);
                moveState = new TestMoveState(Machine);

                slashState = new TestAttackState(Machine);
                rangedAttackState = new TestRangedAttackState(Machine);

                Machine.SetState(idleState);
            }
        }

        private TestEntity _entityA;
        private TestEntity _entityB;

        [SetUp]
        public void Setup()
        {
            var objA = new GameObject("EntityA");
            var objB = new GameObject("EntityB");

            _entityA = objA.AddComponent<TestEntity>();
            _entityB = objB.AddComponent<TestEntity>();
        }

        [TearDown]
        public void Teardown()
        {
            if (_entityA != null && _entityA.gameObject != null)
            {
                UnityEngine.Object.DestroyImmediate(_entityA.gameObject);
            }

            if (_entityB != null && _entityB.gameObject != null)
            {
                UnityEngine.Object.DestroyImmediate(_entityB.gameObject);
            }
        }

        [Test]
        public void TestIdle()
        {
            _entityA.Initialize();
            _entityB.Initialize();

            Assert.AreEqual(_entityA.idleState, _entityA.Machine.GetCurrentState(), "EntityA is not in the expected Idle state.");
            Assert.AreEqual(_entityB.idleState, _entityB.Machine.GetCurrentState(), "EntityB is not in the expected Idle state.");
            Debug.Log("TestIdle() - Completed");
        }

        [Test]
        public void StateTransitionTest()
        {
            _entityA.Initialize();
            _entityB.Initialize();

            Debug.Log("Successfully initialized both entities. Now changing their states...");
            try
            {
                _entityA.Machine.SetState(_entityA.moveState);
                _entityB.Machine.SetState(_entityB.moveState);
            }
            catch (AssertionException e)
            {
                throw new AssertionException(e.ToString());
            }
            Debug.Log("Successfully changed both the entities' states to a Moving State.");
        }

        [Test]
        public void SharedStateTest()
        {
            _entityA.Initialize();
            _entityB.Initialize();

            Debug.Log("Successfully initialized both entities. Now changing their states...");
            try
            {
                _entityA.Machine.SetState(_entityA.slashState);
                _entityB.Machine.SetState(_entityB.rangedAttackState);
            }
            catch (AssertionException e)
            {
                throw new AssertionException(e.ToString());
            }
            Debug.Log("Successfully changed both the entities' states to a Attacking State.");
        }

        [Test]
        public void InvalidTransitionTest()
        {
            _entityA.Initialize();
            _entityB.Initialize();

            Debug.Log("Successfully initialized both entities. Now changing their states...");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => _entityA.Machine.SetState(null),
                "Setting a null state should throw ArgumentNullException for EntityA."
            );

            Assert.Throws<ArgumentNullException>(
                () => _entityB.Machine.SetState(null),
                "Setting a null state should throw ArgumentNullException for EntityB."
            );
        }

        [Test]
        public void EntityIndependenceTest()
        {
            _entityA.Initialize();
            _entityB.Initialize();

            Debug.Log("Successfully initialized both entities. Now changing their states...");
            try
            {
                _entityA.Machine.SetState(_entityA.idleState);
                _entityB.Machine.SetState(_entityB.moveState);
            }
            catch (AssertionException e)
            {
                throw new AssertionException(e.ToString());
            }
            Debug.Log($"EntityA's active state: {_entityA.Machine.GetCurrentState()}");
            Debug.Log($"EntityB's active state: {_entityB.Machine.GetCurrentState()}");
        }
    }
}
