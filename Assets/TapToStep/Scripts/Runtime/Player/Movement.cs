using System;
using System.Threading;
using CompositionRoot.Enums;
using DG.Tweening;
using InputActions;
using Runtime.Player.Perks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Player
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Transform _playerRoot;
        [SerializeField] private Rigidbody _playerRigidBody;
        
        private PlayerEntryPoint _entryPoint;
        private TouchInputAction _inputAction;
        private PlayerPerkSystem _perkSystem;
        private Tween _movementTween;
        
        private bool _canMove;

        private const float MIN_HORIZONTAL_SLIDE = -2f;
        private const float MAX_HORIZONTAL_SLIDE = 2f;
        private const float MIN_HIGH_POSITION = -50f;
        

        public void Init(PlayerEntryPoint entryPoint, PlayerPerkSystem playerPerkSystem)
        {
            _canMove = true;
            _playerRigidBody.isKinematic = false;
            
            _perkSystem = playerPerkSystem;
            _entryPoint = entryPoint;
            _entryPoint.PlayerEventHandler.OnMoveButtonTouched += MoveAfterTouch;
            
        }

        public void Destruct()
        {
            _entryPoint.PlayerEventHandler.OnMoveButtonTouched -= MoveAfterTouch;
        }

        public void OnPlayerDied()
        {
            _playerRigidBody.isKinematic = true;
            _movementTween?.Pause();
            _movementTween?.Kill();
            _canMove = false;
        }

        public void OnPlayerResumed()
        {
            _playerRigidBody.isKinematic = false;
            _canMove = true;
        }

        private void MoveAfterTouch(MoveDirection moveDirection)
        {
            if (_movementTween != null && _movementTween.IsActive() && !_movementTween.IsComplete()) return;
            if(_canMove == false) return;
            
            MakeStep(moveDirection);
        }

        private void MakeStep(MoveDirection moveDirection)
        {
            if(moveDirection == MoveDirection.None) return;
            var setting = _entryPoint.PlayerSettingSo;
            
            var horizontalSlide = GetOffsetByDirection(moveDirection);
            var stepTime = setting.StepSpeed - _perkSystem.GetPerkValueByType(PerkType.StepSpeed);
            var stepLenght = setting.StepLenght + _perkSystem.GetPerkValueByType(PerkType.StepLenght);
            
            var newStepPosition = _playerRoot.position.z + stepLenght;
            var newSlidePosition = Math.Clamp(_playerRoot.position.x + horizontalSlide, MIN_HORIZONTAL_SLIDE, MAX_HORIZONTAL_SLIDE) ;
            
            _movementTween = _playerRoot.DOMoveZ(newStepPosition, stepTime).SetEase(Ease.InOutFlash);
            _playerRoot.DOMoveX(newSlidePosition, stepTime).SetEase(Ease.InOutFlash);
            
            CheckHorizontalPosition();
            CheckVerticalPosition();

            _entryPoint.PlayerStatistic.UpdateDistance(stepLenght);
            _entryPoint.PlayerEventHandler.InvokeStartMoving();
        }

        private float GetOffsetByDirection(MoveDirection moveDirection)
        {
            return moveDirection switch
            {
                MoveDirection.Left => -0.3f - _perkSystem.GetPerkValueByType(PerkType.TurnSpeed),
                MoveDirection.Up => 0f,
                MoveDirection.Right => 0.3f + _perkSystem.GetPerkValueByType(PerkType.TurnSpeed),
                MoveDirection.Down => 0f,
                MoveDirection.None => 0f,
                _ => 0f
            };
        }

        private void CheckHorizontalPosition()
        {
            switch (_playerRoot.position.x)
            {
                case > MAX_HORIZONTAL_SLIDE:
                    _playerRoot.position = new Vector3(MAX_HORIZONTAL_SLIDE, _playerRoot.position.y, _playerRoot.position.z);
                    return;
                case < MIN_HORIZONTAL_SLIDE:
                    _playerRoot.position = new Vector3(MIN_HORIZONTAL_SLIDE, _playerRoot.position.y, _playerRoot.position.z);
                    return;
            }
        }

        private void CheckVerticalPosition()
        {
            if (_playerRoot.position.y < MIN_HIGH_POSITION)
            {
                _playerRigidBody.linearVelocity = Vector3.zero;
                _playerRoot.position = new Vector3(_playerRoot.position.x, 5f, _playerRoot.position.z);
            }
        }
    }
}