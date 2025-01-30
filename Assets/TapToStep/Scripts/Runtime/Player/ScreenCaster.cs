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
        private GameEventHandler _gameEventHandler;

        public void Init(PlayerEntryPoint entryPoint, GameEventHandler gameEventHandler)
        {
            _entryPoint = entryPoint;
            _gameEventHandler = gameEventHandler;
            
            _inputAction = new TouchInputAction();
            _inputAction.GameTouch.Tap.performed += TapPrefer;
            _gameEventHandler.OnPlayerDied += OnPlayerDied;

            ActivateControl();
            
            Debug.Log("Called Init in screen cast system");
        }

        public void Destruct()
        {
            DeactivateControl();
            _inputAction.GameTouch.Tap.performed -= TapPrefer;
            _gameEventHandler.OnPlayerDied -= OnPlayerDied;
            _inputAction = null;
            Debug.Log("Called Destruct in screen cast system");
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