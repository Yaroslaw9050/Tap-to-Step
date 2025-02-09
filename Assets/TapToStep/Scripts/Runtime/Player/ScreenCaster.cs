using Cysharp.Threading.Tasks;
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
        private MoveDirection _moveDirection;

        public void Init(PlayerEntryPoint entryPoint, GameEventHandler gameEventHandler)
        {
            _entryPoint = entryPoint;
            _gameEventHandler = gameEventHandler;
            
            _inputAction = new TouchInputAction();
            _gameEventHandler.OnPlayerScreenCastStatusChanged += ScreenCastStatusChanged;
            ActivateControlAsync().Forget();
        }

        public void Destruct()
        {
            DeactivateControl();
            _gameEventHandler.OnPlayerScreenCastStatusChanged -= ScreenCastStatusChanged;
            _inputAction = null;
        }

        private void ScreenCastStatusChanged(bool castIsActive)
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
            await UniTask.NextFrame();
            _inputAction.GameTouch.TapPosition.performed += ReadTapPosition;
            _inputAction.GameTouch.Taped.started += TapStatusChanged;
            _inputAction.GameTouch.Taped.canceled += TapStatusChanged;
        }

        private void TapStatusChanged(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                _entryPoint.PlayerEventHandler.InvokeMoveButtonTouched(_moveDirection);
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