using System;
using DG.Tweening;
using InputActions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Player
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Transform _playerRoot;
        
        private PlayerEntryPoint _entryPoint;
        private TouchInputAction _inputAction;
        private Tween _movementTween;
        
        private bool _canMove;

        private const float MIN_HORIZONTAL_SLIDE = -2f;
        private const float MAX_HORIZONTAL_SLIDE = 2f;
        

        public void Init(PlayerEntryPoint entryPoint)
        {
            _canMove = true;
            
            _entryPoint = entryPoint;
            _entryPoint.PlayerEventHandler.OnMoveButtonTouched += MoveAfterTouch;
            _entryPoint.PlayerEventHandler.OnPlayerDied += OnPlayerDied;
        }

        private void MoveAfterTouch(MoveDirection moveDirection)
        {
            if (_movementTween != null && _movementTween.IsActive() && !_movementTween.IsComplete()) return;
            if(_canMove == false) return;
            
            MakeStep(moveDirection);
        }

        private void MakeStep(MoveDirection moveDirection)
        {
            var horizontalSlide = GetOffsetByDirection(moveDirection);
            var setting = _entryPoint.PlayerSettingSo;
            var newStepPosition = _playerRoot.position.z + setting.StepDistance;
            var newSlidePosition = Math.Clamp(_playerRoot.position.x + horizontalSlide, MIN_HORIZONTAL_SLIDE, MAX_HORIZONTAL_SLIDE) ;
            
            _movementTween = _playerRoot.DOMoveZ(newStepPosition, setting.StepTime).SetEase(Ease.InOutFlash);
            _playerRoot.DOMoveX(newSlidePosition, setting.StepTime).SetEase(Ease.InOutFlash);

            setting.Distance += setting.StepDistance;
            _entryPoint.PlayerEventHandler.InvokeStartMoving();
        }

        private float GetOffsetByDirection(MoveDirection moveDirection)
        {
            return moveDirection switch
            {
                MoveDirection.Left => -0.75f,
                MoveDirection.Up => 0f,
                MoveDirection.Right => 0.75f,
                MoveDirection.Down => 0f,
                MoveDirection.None => 0f,
                _ => 0f
            };
        }

        private void OnPlayerDied()
        {
            _movementTween?.Kill();
            _canMove = false;
        }
    }
}