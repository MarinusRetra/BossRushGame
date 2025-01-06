using BossRush.Utility;
using System;
using UnityEngine;

namespace BossRush.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        public PlayerInputSystem InputSystem;
        public PlayerInputSystem.PlayerActions PlayerActions;
    }
}
