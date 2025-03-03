using System;
using Core.Service.GlobalEvents;
using Cysharp.Threading.Tasks;
using InputActions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Runtime.Player
{
    public class ScreenCaster
    {
        private TouchInputAction _inputAction;
        private MoveDirection _moveDirection;

        private readonly PlayerEntryPoint r_entryPoint;
        private readonly GlobalEventsHolder r_globalEventsHolder;

        public event Action<MoveDirection> OnTouchedToScreenWithDirection;

        public ScreenCaster(PlayerEntryPoint entryPoint, GlobalEventsHolder globalEventsHolder)
        {
            r_entryPoint = entryPoint;
            r_globalEventsHolder = globalEventsHolder;
            
            _inputAction = new TouchInputAction();
            r_globalEventsHolder.PlayerEvents.OnScreenInputStatusChanged += ScreenInputStatusChanged;
        }

        public void Destruct()
        {
            DeactivateControl();
            r_globalEventsHolder.PlayerEvents.OnScreenInputStatusChanged -= ScreenInputStatusChanged;
            _inputAction = null;
        }

        private void ScreenInputStatusChanged(bool castIsActive)
        {
            if (castIsActive)
                ActivateControlAsync().Forget();
            else
                DeactivateControl();
        }

        private void DeactivateControl()
        {
            _inputAction.GameTouch.TapPosition.Disable();
            _inputAction.GameTouch.Taped.Disable();
            _inputAction.GameTouch.TapPosition.performed -= ReadTapPosition;
            _inputAction.GameTouch.Taped.started -= TapStatusChanged;
            _inputAction.GameTouch.Taped.canceled -= TapStatusChanged;
        }

        private async UniTask ActivateControlAsync()
        {
            _inputAction.GameTouch.TapPosition.Enable();
            _inputAction.GameTouch.Taped.Enable();
            await UniTask.DelayFrame(60);
            _inputAction.GameTouch.TapPosition.performed += ReadTapPosition;
            _inputAction.GameTouch.Taped.started += TapStatusChanged;
            _inputAction.GameTouch.Taped.canceled += TapStatusChanged;
        }

        private void TapStatusChanged(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                OnTouchedToScreenWithDirection?.Invoke(_moveDirection);
            }
        }

        private void ReadTapPosition(InputAction.CallbackContext context)
        {
            var screenPosition = context.ReadValue<Vector2>();

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            var upperLimit = screenHeight * 0.75f;
            var bottomLimit = screenHeight * 0.1f;
            
            var leftBound = screenWidth * 0.3f;
            var rightBound = screenWidth * 0.7f;

            if (screenPosition.y < bottomLimit || screenPosition.y > upperLimit)
            {
                _moveDirection = MoveDirection.None;
                return;
            }
            
            if (screenPosition.x < leftBound)
            {
                _moveDirection = MoveDirection.Left;
            }
            else if (screenPosition.x > rightBound)
            {
                _moveDirection = MoveDirection.Right;
            }
            else
            {
                _moveDirection = MoveDirection.Up;
            }
        }
    }
}