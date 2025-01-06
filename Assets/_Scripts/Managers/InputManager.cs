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
        private PlayerInputSystem InputSystem;
        private PlayerInputSystem.PlayerActions PlayerActions;
        private PlayerInputSystem.UIActions UI;


        //These events are called in the OnEvent functions
        //When an input event is called these are called afterwards
        public event Action JumpEvent;
        public event Action JumpCancelledEvent;
        public event Action BasicAttackEvent;
        public event Action CrouchEvent;
        public event Action CrouchEventCancelled;
        public event Action Ability1Event;
        public event Action Ability2Event;
        public event Action Ability3Event;
        public event Action SprintEvent;
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

                PlayerActions = InputSystem.Player; //Reference naar de Player op lijn
                UI = InputSystem.UI; //Reference naar de UI van PlayerInputSystem op lijn 508

                //SetCallbacks maps the events from the InputSystem to the functions in this script,
                //so when an event is called in the input system the corresponding function here is called.
                PlayerActions.SetCallbacks(this);
                UI.SetCallbacks(this);
                //You have to call it twice because UI and PlayerActions are two different interfaces.

                EnablePlayerActions();
            }
        }

        //These two functions are used to make sure only one input mapping is enabled at a time.
        public void EnablePlayerActions()
        { 
            PlayerActions.Enable();
            UI.Disable();
        }

        public void EnableUIActions()
        { 
            PlayerActions.Disable();
            UI.Enable();
        }

        //PlayerAction functions
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnBasic_Attack(InputAction.CallbackContext context)
        {
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

            if (context.phase == InputActionPhase.Canceled)
            {
                JumpCancelledEvent?.Invoke();
            }
        }

        public void OnAbilty1(InputAction.CallbackContext context)
        {
            Ability1Event?.Invoke();
        }

        public void OnAbilty2(InputAction.CallbackContext context)
        {
            Ability2Event?.Invoke();
        }

        public void OnAbility3(InputAction.CallbackContext context)
        {
            Ability3Event?.Invoke();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            SprintEvent?.Invoke();
        }

        //UIAction functions
        public void OnNavigate(InputAction.CallbackContext context)
        {
            NavigateEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                ResumeEvent?.Invoke();
                EnablePlayerActions();
            }
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            PointEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            ClickEvent?.Invoke();
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                PauseEvent?.Invoke();
                EnableUIActions();
            }
        }
    }
}
