using BossRush.Utility;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BossRush.Managers
{
    /// <summary>
    /// Inherits a singleton to make an instance from itself and inherits IPlayerActions and IUIActions to override the function that are called on event calls
    /// </summary>
    public class InputManager : Singleton<InputManager>, PlayerInputSystem.IPlayerActions, PlayerInputSystem.IUIActions
    {
        //Caches the input system, PlayerActions and UiActions
        public PlayerInputSystem InputSystem;
        public PlayerInputSystem.PlayerActions Player_Actions;
        public PlayerInputSystem.UIActions UI_Actions;


        //These events are called in the OnEvent functions
        //When an input event is called these are called afterwards
        public event Action JumpEvent;
        public event Action JumpCancelledEvent;
        public event Action BasicAttackEvent;
        public event Action CrouchEvent;
        public event Action CrouchEventCancelled;
        public event Action PrimaryEvent;
        public event Action SecondaryEvent;
        public event Action TertiaryEvent;
        public event Action SprintEvent;
        public event Action SprintEventCancelled;
        public event Action PauseEvent;
        public event Action<Vector2> MoveEvent;
        public event Action<Vector2> LookEvent;

        public event Action ResumeEvent;
        public event Action ClickEvent;
        public event Action<Vector2> PointEvent;
        public event Action<Vector2> NavigateEvent;


        private void OnEnable()
        {
            if (InputSystem == null)
            { 
                InputSystem = new PlayerInputSystem();

                Player_Actions = InputSystem.Player;
                UI_Actions = InputSystem.UI;

                // SetCallbacks maps the events from the InputSystem to the functions in this script,
                // so when an event is called in the input system the corresponding function here is called.
                Player_Actions.SetCallbacks(this);
                UI_Actions.SetCallbacks(this);
                // You have to call it twice because UIActions and PlayerActions are two different interfaces.

                EnablePlayerActions();
            }
        }

        // These two functions are used to make sure only one input mapping is enabled at a time.
        public void EnablePlayerActions()
        { 
            Player_Actions.Enable();
            UI_Actions.Disable();
        }

        public void EnableUIActions()
        { 
            Player_Actions.Disable();
            UI_Actions.Enable();
        }

        // PlayerAction functions
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.phase == InputActionPhase.Performed ? context.ReadValue<Vector2>() : Vector2.zero);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                LookEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnBasic_Attack(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                BasicAttackEvent?.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            { 
                CrouchEvent?.Invoke();
            }
            if (context.phase == InputActionPhase.Canceled)
            {
                CrouchEventCancelled?.Invoke();
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                JumpEvent?.Invoke();
            }

            if(context.phase == InputActionPhase.Canceled)
            {
                JumpCancelledEvent?.Invoke();
            }
        }

        public void OnPrimary(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                PrimaryEvent?.Invoke();
        }

        public void OnSecondary(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                SecondaryEvent?.Invoke();
        }

        public void OnTertiary(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                TertiaryEvent?.Invoke();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                SprintEvent?.Invoke();
            else if(context.phase == InputActionPhase.Canceled)
                SprintEventCancelled?.Invoke();
        }
        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                PauseEvent?.Invoke();
                EnableUIActions();
            }
        }

        // UIAction functions
        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                NavigateEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnResume(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                ResumeEvent?.Invoke();
                EnablePlayerActions();
            }
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                PointEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                ClickEvent?.Invoke();
        }
    }
}
