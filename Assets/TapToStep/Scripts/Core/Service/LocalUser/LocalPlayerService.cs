using System;
using System.Globalization;
using System.Threading.Tasks;
using CompositionRoot.Enums;
using CompositionRoot.SO.Player.Logic;
using Core.Service.RemoteDataStorage;
using Cysharp.Threading.Tasks;
using Patterns.MVVM.Models;
using Runtime.Player.Upgrade;
using UnityEngine;
using Zenject;

namespace Core.Service.LocalUser
{
    public sealed class LocalPlayerService : MonoBehaviour
    {
        [SerializeField] private PlayerSettingSO _startupPlayerSetting;
        
        private PlayerPerkSystem _playerPerkSystem;
        private IRemoteDataStorageService _remoteDataStorageService;
        private short _distanceCounter;
        public PlayerModel PlayerModel { get; } = new();
        
        
        [Inject]
        public void Constructor(PlayerPerkSystem perkSystem, IRemoteDataStorageService remoteDataStorageService)
        {
            _playerPerkSystem = perkSystem;
            _remoteDataStorageService = remoteDataStorageService;
        }

        public PlayerPerkSO GetPerk(PerkType perkType)
        {
            return _playerPerkSystem.GetPerkByType(perkType);
        }
        
        public void SetCurrentDistance(double currentDistance)
        {
            PlayerModel.CurrentDistance.Value = currentDistance;
        }
        
        public void SetBestDistance(double bestDistance)
        {
            PlayerModel.BestDistance.Value = bestDistance;
        }

        public void AddBits(ushort newValue)
        {
            PlayerModel.Bits.Value += newValue;
            _remoteDataStorageService.SaveBaseUserData<string>(PlayerModel.UserId.Value, "bits", PlayerModel.Bits
                .Value.ToString());
        }

        public void RemoveBits(uint removedValue)
        {
            PlayerModel.Bits.Value -= removedValue;
            _remoteDataStorageService.SaveBaseUserData<string>(PlayerModel.UserId.Value, "bits", PlayerModel.Bits
                .Value.ToString());
        }

        public void AddDistance(float distance)
        {
            var newDistance = Math.Round(distance, 1);
            PlayerModel.CurrentDistance.Value += newDistance;
            _distanceCounter++;

            if (_distanceCounter >= 10)
            {
                _distanceCounter = 0;
                _remoteDataStorageService.SaveBaseUserData<string>(PlayerModel.UserId.Value, "currentDistance", 
                    PlayerModel.CurrentDistance.Value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public float GetStepLenght()
        {
            return _startupPlayerSetting.StepLenght + _playerPerkSystem.GetPerkValueByType(PerkType.StepLenght);
        }

        public float GetStepTime()
        {
            return _startupPlayerSetting.StepSpeed - _playerPerkSystem.GetPerkValueByType(PerkType.StepSpeed);
        }

        public float GetTurnSpeed()
        {
            return _playerPerkSystem.GetPerkValueByType(PerkType.TurnSpeed);
        }

        public async UniTask LoadAllPerksAsync()
        {
            await _playerPerkSystem.LoadAllPerksAsync();
        }

        public async UniTask LoadBaseUserDataAsync()
        {
            var userName = await _remoteDataStorageService.LoadBaseUserDataAsync(PlayerModel.UserId.Value, "userName");
            var bits = await _remoteDataStorageService.LoadBaseUserDataAsync(PlayerModel.UserId.Value, "bits");
            var currentDistance = await _remoteDataStorageService.LoadBaseUserDataAsync(PlayerModel.UserId.Value, "currentDistance");
            
            SetCurrentDistance(double.Parse(currentDistance));
            PlayerModel.Bits.Value = ushort.Parse(bits);
            PlayerModel.UserName.Value = userName;
        }
    }
}