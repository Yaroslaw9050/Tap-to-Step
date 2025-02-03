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
            _inputAction.GameTouch.Tap.Disable();
            _inputAction.GameTouch.Tap.performed -= TapPrefer;
        }

        private async UniTask ActivateControlAsync()
        {
            _inputAction.GameTouch.Tap.Enable();
            await UniTask.NextFrame();
            _inputAction.GameTouch.Tap.performed += TapPrefer;
        }

        private void TapPrefer(InputAction.CallbackContext context)
        {
            var screenPosition = context.ReadValue<Vector2>();
            
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            var upperLimit = screenHeight * 0.75f;
            var bottomLimit = screenHeight * 0.1f;
            
            var leftBound = screenWidth * 0.3f;
            var rightBound = screenWidth * 0.7f;
            
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