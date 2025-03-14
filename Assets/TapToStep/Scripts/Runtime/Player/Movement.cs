using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using CompositionRoot.Enums;
using Core.Service.LocalUser;
using InputActions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Player
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Transform _playerRoot;
        [SerializeField] private Rigidbody _playerRigidBody;

        private ScreenCaster _screenCaster;
        private PlayerEntryPoint _entryPoint;
        private TouchInputAction _inputAction;
        private LocalPlayerService _localPlayerService;
        private CancellationTokenSource _groundCheckCts;
        
        private bool _canMove;
        private bool _isMoving;
        private Vector3 _targetPosition;
        private Vector3 _movementDirection;
        private Vector3 _groundNormal = Vector3.up;

        private const float MIN_HORIZONTAL_SLIDE = -2f;
        private const float MAX_HORIZONTAL_SLIDE = 2f;
        private const float GROUND_CHECK_DISTANCE = 0.5f;
        private const int SURFACE_CHECK_INTERVAL = 5;

        private void FixedUpdate()
        {
            if (_isMoving)
            {
                Vector3 movePosition = _playerRigidBody.position + _movementDirection * (Time.fixedDeltaTime * 5f);
                _playerRigidBody.MovePosition(movePosition);
            }
        }

        public void Initialise(PlayerEntryPoint entryPoint,
            LocalPlayerService localPlayerService, ScreenCaster screenCaster)
        {
            _canMove = true;
            _groundCheckCts = new CancellationTokenSource();
            _playerRigidBody.isKinematic = false;
            _playerRigidBody.interpolation = RigidbodyInterpolation.Interpolate;

            _localPlayerService = localPlayerService;
            _entryPoint = entryPoint;
            _screenCaster = screenCaster;

            _screenCaster.OnTouchedToScreenWithDirection += MoveAfterTouch;
            CheckGroundSurfaceAsync().Forget();
        }

        public void Destruct()
        {
            _groundCheckCts.Cancel();
            _groundCheckCts.Dispose();
            _screenCaster.OnTouchedToScreenWithDirection -= MoveAfterTouch;
        }

        public void OnPlayerDied()
        {
            _playerRigidBody.isKinematic = true;
            _canMove = false;
            _isMoving = false;
        }

        public void OnPlayerResumed()
        {
            _playerRigidBody.isKinematic = false;
            _canMove = true;
        }

        private void MoveAfterTouch(MoveDirection moveDirection)
        {
            if (!_canMove || _isMoving || !IsGrounded()) return;
            MakeStepAsync(moveDirection).Forget();
        }

        private bool IsGrounded()
        {
            var rayStart = _playerRoot.position + Vector3.down * 1.5f;
            var grounded = Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, GROUND_CHECK_DISTANCE);
            return grounded;
        }

        private float GetOffsetByDirection(MoveDirection moveDirection)
        {
            return moveDirection switch
            {
                MoveDirection.Left => -0.3f - (float)_localPlayerService.GetTurnSpeed(),
                MoveDirection.Up => 0f,
                MoveDirection.Right => 0.3f + (float)_localPlayerService.GetTurnSpeed(),
                MoveDirection.Down => 0f,
                MoveDirection.None => 0f,
                _ => 0f
            };
        }

        private async UniTaskVoid MakeStepAsync(MoveDirection moveDirection)
        {
            if (moveDirection == MoveDirection.None) return;
            
            var horizontalSlide = GetOffsetByDirection(moveDirection);
            var stepLength = _localPlayerService.GetStepLenght();
            var stepTime = _localPlayerService.GetStepTime();
            
            var startPosition = _playerRoot.position;
            var endPosition = new Vector3(
                Mathf.Clamp(startPosition.x + horizontalSlide, MIN_HORIZONTAL_SLIDE, MAX_HORIZONTAL_SLIDE),
                startPosition.y,
                startPosition.z + (float)stepLength
            );
            
            _isMoving = true;
            _entryPoint.GlobalEventsHolder.PlayerEvents.InvokeStartMoving();
            
            var elapsedTime = 0f;
            while (elapsedTime < stepTime)
            {
                elapsedTime += Time.deltaTime;
                var t = elapsedTime / stepTime;
                _playerRigidBody.MovePosition(Vector3.Lerp(startPosition, endPosition, (float)t));
                await UniTask.Yield();
            }
            
            _playerRigidBody.MovePosition(endPosition);
            _localPlayerService.AddDistance(stepLength);
            _entryPoint.PlayerStatistic.UpdateDistance(stepLength);
            
            _isMoving = false;
        }

        private async UniTaskVoid CheckGroundSurfaceAsync()
        {
            while (_groundCheckCts.IsCancellationRequested == false)
            {
                await UniTask.DelayFrame(SURFACE_CHECK_INTERVAL, cancellationToken: _groundCheckCts.Token);

                if (Physics.Raycast(_playerRoot.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 2f))
                {
                    _groundNormal = hit.normal;
                }
            }
        }
    }
}
