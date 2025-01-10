using NUnit.Framework;
using UnityEngine;
using BossRush.Managers;
using UnityEngine.InputSystem;

namespace BossRush
{
    public class PlayerInputSystemPlayModeTest : InputTestFixture
    {
        int swapAmount;
        int inputAmount;
        private InputManager input;
        Keyboard keyboard;
        Mouse mouse;

        [SetUp]
        public void SetUp()
        {
            // Used for simulating key presses and mouse movements
            keyboard = InputSystem.AddDevice<Keyboard>();
            mouse = InputSystem.AddDevice<Mouse>();

            input = InputManager.Instance;

            swapAmount = 0;
            inputAmount = 0;

            // Subscribes this scripts functions to the events in InputManager.
            input.MoveEvent += Input_MoveEvent;
            input.PrimaryEvent += Input_Ability1Event;
            input.SecondaryEvent += Input_Ability2Event;
            input.Ability3Event += Input_Ability3Event;
            input.BasicAttackEvent += Input_BasicAttackEvent;
            input.CrouchEvent += Input_CrouchEvent;
            input.CrouchEventCancelled += Input_CrouchEventCancelled;
            input.JumpEvent += Input_JumpEvent;
            input.JumpCancelledEvent += Input_JumpEventCancelled;
            input.SprintEvent += Input_SprintEvent;
            input.LookEvent += Input_LookEvent;
            input.PauseEvent += Input_PauseEvent;
            input.PointEvent += UIInput_PointEvent;
            input.NavigateEvent += UIInput_NavigateEvent;
            input.ResumeEvent += UIInput_ResumeEvent;
            input.ClickEvent += UIInput_ClickEvent;

            input.EnablePlayerActions();
        }

        [Test]
        public void InvokesEventsWhenInputIsDetected()
        {
            Debug.Log("Starting InvokeEventsWhenInputIsDetected" + " inputAmount: " + inputAmount);
            // Presses every key that is mapped to an PlayerAction
            Press(keyboard.wKey);// 1
            Press(keyboard.shiftKey);// 2
            Press(keyboard.spaceKey);// 3
            Release(keyboard.spaceKey);// 4
            Press(keyboard.leftCtrlKey);// 5
            Release(keyboard.leftCtrlKey);// 6
            Press(keyboard.digit1Key);// 7 
            Press(keyboard.digit2Key);// 8 
            Press(keyboard.digit3Key);// 9 
            Press(mouse.leftButton);// 10 
            Move(mouse.position, new Vector2(1, 1)); // 11 
            Press(keyboard.escapeKey); // Swaps action mapping // 12 
            
            // Presses every key that is mapped to an UIAction
            Move(mouse.position, new Vector2(2, 2)); // 13 
            Press(keyboard.dKey); // 14 
            Press(mouse.leftButton); // 15 
            Press(keyboard.escapeKey); // 16 

            Debug.Log("Ending InvokeEventsWhenInputIsDetected" + " inputAmount: " + inputAmount);
            Assert.AreEqual(16, inputAmount); 
            
            // Every input detection calls one event except spaceKey and leftCTRLKey which call two
            // so inputAmount is 16 if all input is detected correctly
        }

        [Test]
        public void ActionMapSwapsOnEscapePress()
        {
            Debug.Log($"Starting ActionMapSwapsOnEscapePress swapAmount: {swapAmount}");

            Press(keyboard.escapeKey); // Pressing escape causes the active ActionMap to change to UIActions
            Release(keyboard.escapeKey);

            swapAmount += input.UI_Actions.enabled ? 1 : 0;

            Press(keyboard.escapeKey); // Pressing escape again causes the active ActionMap to change back to PlayerActions
            Release(keyboard.escapeKey);

            swapAmount += input.Player_Actions.enabled ? 1 : 0;

            Debug.Log($"Ending ActionMapSwapsOnEscapePress swapAmount: {swapAmount}");

            Assert.AreEqual(2, swapAmount);
        }

        [TearDown]
        public void CleanUp()
        {
            // Unsubscribes from the InputManager events
            input.MoveEvent -= Input_MoveEvent;
            input.PrimaryEvent -= Input_Ability1Event;
            input.SecondaryEvent -= Input_Ability2Event;
            input.Ability3Event -= Input_Ability3Event;
            input.BasicAttackEvent -= Input_BasicAttackEvent;
            input.CrouchEvent -= Input_CrouchEvent;
            input.CrouchEventCancelled -= Input_CrouchEventCancelled;
            input.JumpEvent -= Input_JumpEvent;
            input.JumpCancelledEvent -= Input_JumpEventCancelled;
            input.SprintEvent -= Input_SprintEvent;
            input.LookEvent -= Input_LookEvent;
            input.PauseEvent -= Input_PauseEvent;
            input.PointEvent -= UIInput_PointEvent;
            input.NavigateEvent -= UIInput_NavigateEvent;
            input.ResumeEvent -= UIInput_ResumeEvent;
            input.ClickEvent -= UIInput_ClickEvent;
        }


        // These are called when their corresponding events are called.
        private void Input_PauseEvent()
        {
            Debug.Log("Switched from PlayerActions to UIActions");
            inputAmount++;
            Debug.Log(inputAmount);
        }

        private void Input_SprintEvent()
        {
            inputAmount++;
            Debug.Log(inputAmount);
        }

        private void Input_LookEvent(Vector2 obj)
        {
            inputAmount++;
            Debug.Log(inputAmount);
        }
        private void Input_JumpEvent()
        {
            inputAmount++;
            Debug.Log(inputAmount);
        }

        private void Input_JumpEventCancelled()
        {
            inputAmount++;
            Debug.Log(inputAmount);
        }

        private void Input_CrouchEventCancelled()
        {
            inputAmount++;
            Debug.Log(inputAmount);
        }

        private void Input_CrouchEvent()
        {
            inputAmount++;
            Debug.Log(inputAmount);
        }

        private void Input_BasicAttackEvent()
        {
            inputAmount++;
            Debug.Log(inputAmount);
        }

        private void Input_Ability1Event()
        {
            inputAmount++;
            Debug.Log(inputAmount);
        }

        private void Input_Ability2Event()
        {
            inputAmount++;
            Debug.Log(inputAmount); ;
        }

        private void Input_Ability3Event()
        {
            inputAmount++;
            Debug.Log(inputAmount);
        }

        private void Input_MoveEvent(Vector2 obj)
        {
            inputAmount++;
            Debug.Log(inputAmount);
        }

        // These are called when the corresponding UI events are called.
        private void UIInput_ClickEvent()
        {
            inputAmount++;
            Debug.Log(inputAmount);
        }
        private void UIInput_ResumeEvent()
        {
            Debug.Log("Swapped to PlayerActions from UI Actions");
            inputAmount++;
            Debug.Log(inputAmount);
        }
        private void UIInput_PointEvent(Vector2 obj)
        {
            inputAmount++;
            Debug.Log(inputAmount);
        }

        private void UIInput_NavigateEvent(Vector2 obj)
        {
            inputAmount++;
            Debug.Log(inputAmount);
        }
    }
}
