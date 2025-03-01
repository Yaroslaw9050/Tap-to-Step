using System;
using Cysharp.Threading.Tasks;
using CompositionRoot.Enums;
using InputActions;
using Runtime.Player.Perks;
using TapToStep.Scripts.Core.Service.LocalUser;
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
        private PlayerPerkSystem _perkSystem;
        private LocalPlayerService _localPlayerService;
        
        private bool _canMove;
        private bool _isMoving;
        private Vector3 _targetPosition;
        private Vector3 _movementDirection;
        private Vector3 _groundNormal = Vector3.up;

        private const float MIN_HORIZONTAL_SLIDE = -2f;
        private const float MAX_HORIZONTAL_SLIDE = 2f;
        private const float GROUND_CHECK_DISTANCE = 0.3f;
        private const int SURFACE_CHECK_INTERVAL = 5;

        private void FixedUpdate()
        {
            if (_isMoving)
            {
                Vector3 movePosition = _playerRigidBody.position + _movementDirection * (Time.fixedDeltaTime * 5f);
                _playerRigidBody.MovePosition(movePosition);
            }
        }

        public void Initialise(PlayerEntryPoint entryPoint, PlayerPerkSystem playerPerkSystem,
            LocalPlayerService localPlayerService, ScreenCaster screenCaster)
        {
            _canMove = true;
            _playerRigidBody.isKinematic = false;
            _playerRigidBody.interpolation = RigidbodyInterpolation.Interpolate;

            _localPlayerService = localPlayerService;
            _perkSystem = playerPerkSystem;
            _entryPoint = entryPoint;
            _screenCaster = screenCaster;

            _screenCaster.OnTouchedToScreenWithDirection += MoveAfterTouch;
            CheckGroundSurfaceAsync().Forget();
        }

        public void Destruct()
        {
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
                MoveDirection.Left => -0.3f - _perkSystem.GetPerkValueByType(PerkType.TurnSpeed),
                MoveDirection.Up => 0f,
                MoveDirection.Right => 0.3f + _perkSystem.GetPerkValueByType(PerkType.TurnSpeed),
                MoveDirection.Down => 0f,
                MoveDirection.None => 0f,
                _ => 0f
            };
        }

        private async UniTaskVoid MakeStepAsync(MoveDirection moveDirection)
        {
            if (moveDirection == MoveDirection.None) return;

            var setting = _entryPoint.PlayerSettingSo;
            var horizontalSlide = GetOffsetByDirection(moveDirection);
            var stepLenght = setting.StepLenght + _perkSystem.GetPerkValueByType(PerkType.StepLenght);
            var stepTime = setting.StepSpeed - _perkSystem.GetPerkValueByType(PerkType.StepSpeed);

            _movementDirection = new Vector3(
                Mathf.Clamp(_playerRoot.position.x + horizontalSlide, MIN_HORIZONTAL_SLIDE, MAX_HORIZONTAL_SLIDE) - _playerRoot.position.x,
                0,
                stepLenght
            );
            
            _movementDirection = Vector3.ProjectOnPlane(_movementDirection, _groundNormal).normalized;

            _isMoving = true;
            _entryPoint.GlobalEventsHolder.PlayerEvents.InvokeStartMoving();

            await UniTask.Delay(TimeSpan.FromSeconds(stepTime));

            _localPlayerService.AddDistance(stepLenght);
            _entryPoint.PlayerStatistic.UpdateDistance(stepLenght);

            _isMoving = false;
        }

        private async UniTaskVoid CheckGroundSurfaceAsync()
        {
            while (true)
            {
                await UniTask.DelayFrame(SURFACE_CHECK_INTERVAL);

                if (Physics.Raycast(_playerRoot.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 2f))
                {
                    _groundNormal = hit.normal;
                }
            }
        }
    }
}