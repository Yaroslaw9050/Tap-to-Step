using System;
using DG.Tweening;
using Runtime.EntryPoints.EventHandlers;
using UnityEngine;

namespace Runtime.Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Camera _camera;

        private GameEventHandler _gameEventHandler;
        private PlayerEntryPoint _entryPoint;
        private Tween _cameraTurnTween;
        
        private float _currentRotationY;
        
        private const float CAMERA_LIFT_AMOUNT = 0.05f;

        public void Init(PlayerEntryPoint playerEntryPoint, GameEventHandler gameEventHandler)
        {
            _entryPoint = playerEntryPoint;
            _gameEventHandler = gameEventHandler;
            _entryPoint.PlayerEventHandler.OnPlayerStartMoving += MoveLikeStep;
            _entryPoint.PlayerEventHandler.OnPlayerDied += MoveToDeadPosition;
            _gameEventHandler.OnMenuViewStatusChanged += MenuViewCalled;
        }

        public void Destruct()
        {
            _entryPoint.PlayerEventHandler.OnPlayerStartMoving -= MoveLikeStep;
            _entryPoint.PlayerEventHandler.OnPlayerDied -= MoveToDeadPosition;
        }

        private void MoveLikeStep()
        {
            var setting = _entryPoint.PlayerSettingSo;
            var firstHalfCamPosition = _cameraTransform.localPosition.z + CAMERA_LIFT_AMOUNT;
            var secondHalfCamPosition = firstHalfCamPosition - CAMERA_LIFT_AMOUNT;
            var halfStepTime = setting.StepSpeed / 2;

            _cameraTransform.DOLocalMoveY(firstHalfCamPosition, halfStepTime).SetEase(Ease.InFlash).OnComplete(() =>
            {
                _cameraTransform.DOLocalMoveY(secondHalfCamPosition, halfStepTime).SetEase(Ease.OutFlash);
            });
        }

        private void MoveToDeadPosition()
        {
            var camPosition = new Vector3(_cameraTransform.position.x, 0.2f, _cameraTransform.position.z);
            _cameraTransform.DOMove(camPosition, 1f).SetEase(Ease.InOutFlash);
        }

        private void MenuViewCalled(bool isActive)
        {
            LookAt(isActive ? CameraTargetType.Up : CameraTargetType.Forward, 0.5f);
        }

        private void LookAt(CameraTargetType cameraTargetType, float animTime = 0, Action onCompleted = null)
        {
            if (_cameraTurnTween != null && _cameraTurnTween.IsActive() && !_cameraTurnTween.IsComplete()) return;

            switch (cameraTargetType)
            {
                case CameraTargetType.Forward:
                    TurnCameraForward(animTime, onCompleted);
                    _camera.DOFieldOfView(136, animTime).SetEase(Ease.InOutCubic);
                    break;
                case CameraTargetType.Up:
                    TurnCameraToUp(animTime, onCompleted);
                    _camera.DOFieldOfView(90f, animTime).SetEase(Ease.InOutCubic);
                    break;
                case CameraTargetType.Backward:
                    TurnCameraBackward(animTime, onCompleted);
                    _camera.DOFieldOfView(136, animTime).SetEase(Ease.InOutCubic);
                    break;
            }
        }

        private void TurnCameraToUp(float duration, Action onCompleted)
        {
            var rotationValue = Vector3.left * 60f;
            _cameraTurnTween = _cameraTransform.DOLocalRotate(rotationValue, duration).SetEase(Ease.InOutCubic).OnComplete(() => onCompleted?.Invoke());
        }

        private void TurnCameraForward(float duration, Action onCompleted)
        {
            var rotationValue = Vector3.zero;
            _cameraTurnTween = _cameraTransform.DOLocalRotate(rotationValue, duration).SetEase(Ease.InOutCubic).OnComplete(() => onCompleted?.Invoke());
        }
        
        private void TurnCameraBackward(float duration, Action onCompleted)
        {
            var rotationValue = Vector3.up * -180f;
            _cameraTurnTween = _cameraTransform.DOLocalRotate(rotationValue, duration).SetEase(Ease.InOutCubic).OnComplete(() => onCompleted?.Invoke());
        }
    }

    public enum CameraTargetType
    {
        Forward,
        Backward,
        Up 
    }
}