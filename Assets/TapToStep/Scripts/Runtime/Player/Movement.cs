using System;
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
        
        private PlayerEntryPoint _entryPoint;
        private TouchInputAction _inputAction;
        private PlayerPerkSystem _perkSystem;
        private Tween _movementTween;
        
        private bool _canMove;

        private const float MIN_HORIZONTAL_SLIDE = -2f;
        private const float MAX_HORIZONTAL_SLIDE = 2f;
        

        public void Init(PlayerEntryPoint entryPoint, PlayerPerkSystem playerPerkSystem)
        {
            _canMove = true;
            
            _perkSystem = playerPerkSystem;
            _entryPoint = entryPoint;
            _entryPoint.PlayerEventHandler.OnMoveButtonTouched += MoveAfterTouch;
            _entryPoint.PlayerEventHandler.OnPlayerDied += OnPlayerDied;
        }

        public void Destruct()
        {
            _entryPoint.PlayerEventHandler.OnMoveButtonTouched -= MoveAfterTouch;
            _entryPoint.PlayerEventHandler.OnPlayerDied -= OnPlayerDied;
        }

        private void MoveAfterTouch(MoveDirection moveDirection)
        {
            if (_movementTween != null && _movementTween.IsActive() && !_movementTween.IsComplete()) return;
            if(_canMove == false) return;
            
            MakeStep(moveDirection);
        }

        private void MakeStep(MoveDirection moveDirection)
        {
            var setting = _entryPoint.PlayerSettingSo;
            var statistic = _entryPoint.PlayerStatistic;
            
            var horizontalSlide = GetOffsetByDirection(moveDirection);
            var stepTime = setting.StepSpeed - _perkSystem.GetPerkValueByType(PerkType.StepSpeed);
            var stepLenght = setting.StepLenght + _perkSystem.GetPerkValueByType(PerkType.StepLenght);
            
            Debug.Log($"StepTime: {stepTime}");
            Debug.Log($"StepLenght: {stepLenght}");
            Debug.Log($"TurnSpeed: {horizontalSlide}");
            
            var newStepPosition = _playerRoot.position.z + stepLenght;
            var newSlidePosition = Math.Clamp(_playerRoot.position.x + horizontalSlide, MIN_HORIZONTAL_SLIDE, MAX_HORIZONTAL_SLIDE) ;
            
            _movementTween = _playerRoot.DOMoveZ(newStepPosition, stepTime).SetEase(Ease.InOutFlash);
            _playerRoot.DOMoveX(newSlidePosition, stepTime).SetEase(Ease.InOutFlash);

            _entryPoint.PlayerStatistic.Distance += stepLenght;
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

        private void OnPlayerDied()
        {
            _movementTween?.Kill();
            _canMove = false;
        }
    }
}