using System;
using InputActions;
using Runtime.EntryPoints.EventHandlers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Runtime.Player
{
    public class ScreenCaster : MonoBehaviour
    {
        private TouchInputAction _inputAction;
        private PlayerEntryPoint _entryPoint;
        private GlobalEventHandler _globalEventHandler;

        public void Init(PlayerEntryPoint entryPoint, GlobalEventHandler globalEventHandler)
        {
            _entryPoint = entryPoint;
            _globalEventHandler = globalEventHandler;
            
            _inputAction = new TouchInputAction();
            _inputAction.GameTouch.Tap.performed += TapPrefer;
            _globalEventHandler.OnPlayerDied += OnPlayerDied;

            ActivateControl();
        }

        public void Destruct()
        {
            DeactivateControl();
            _inputAction.GameTouch.Tap.performed -= TapPrefer;
            _globalEventHandler.OnPlayerDied -= OnPlayerDied;
        }

        private void ActivateControl()
        {
            _inputAction.Enable();
        }

        private void DeactivateControl()
        {
            _inputAction.Disable();
        }

        private void OnPlayerDied()
        {
            DeactivateControl();
        }

        private void TapPrefer(InputAction.CallbackContext context)
        {
            var screenPosition = context.ReadValue<Vector2>();
            
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            var upperLimit = screenHeight * 0.75f;
            var bottomLimit = screenHeight * 0.1f;
            
            var leftBound = screenWidth * 0.25f;
            var rightBound = screenWidth * 0.75f;
            
            if(screenPosition.y < bottomLimit || screenPosition.y > upperLimit) 
                return;
            
            if (screenPosition.x < leftBound)
            {
                _entryPoint.PlayerEventHandler.InvokeMoveButtonTouched(MoveDirection.Left);
            }
            else if (screenPosition.x > rightBound)
            {
                _entryPoint.PlayerEventHandler.InvokeMoveButtonTouched(MoveDirection.Right);
            }
            else
            {
                _entryPoint.PlayerEventHandler.InvokeMoveButtonTouched(MoveDirection.Up);
            }
        }
    }
}