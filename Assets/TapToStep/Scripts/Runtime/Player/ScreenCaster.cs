using System;
using InputActions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Runtime.Player
{
    public class ScreenCaster : MonoBehaviour
    {
        private TouchInputAction _inputAction;
        private PlayerEntryPoint _entryPoint;

        public void ActivateControl()
        {
            _inputAction.Enable();
        }

        public void DeactivateControl()
        {
            _inputAction.Disable();
        }

        public void Init(PlayerEntryPoint entryPoint)
        {
            _entryPoint = entryPoint;
            
            _inputAction = new TouchInputAction();
            _inputAction.GameTouch.Tap.performed += TapPrefer;
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
                _entryPoint.EventHandler.MoveButtonTouched(MoveDirection.Left);
            }
            else if (screenPosition.x > rightBound)
            {
                _entryPoint.EventHandler.MoveButtonTouched(MoveDirection.Right);
            }
            else
            {
                _entryPoint.EventHandler.MoveButtonTouched(MoveDirection.Up);
            }
        }
    }
}