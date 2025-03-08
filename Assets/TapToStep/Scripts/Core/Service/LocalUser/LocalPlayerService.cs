using System;
using System.Globalization;
using CompositionRoot.Constants;
using CompositionRoot.Enums;
using CompositionRoot.SO.Player.Logic;
using Core.Service.Leaderboard;
using Core.Service.RemoteDataStorage;
using Cysharp.Threading.Tasks;
using Runtime.Player.Upgrade;
using UI.Models;
using UnityEngine;
using Zenject;

namespace Core.Service.LocalUser
{
    public sealed class LocalPlayerService : MonoBehaviour
    {
        [SerializeField] private PlayerSettingSO _startupPlayerSetting;
        
        private PlayerPerkSystem _playerPerkSystem;
        private IRemoteDataStorageService _remoteDataStorageService;
        private ILeaderboardService _leaderboardService;
        private short _distanceCounter;
        public PlayerModel PlayerModel { get; } = new();

        [Inject]
        public void Constructor(PlayerPerkSystem perkSystem, IRemoteDataStorageService remoteDataStorageService, ILeaderboardService leaderboardService)
        {
            _playerPerkSystem = perkSystem;
            _remoteDataStorageService = remoteDataStorageService;
            _leaderboardService = leaderboardService;
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

        public void ForceUpdateDistanceDataOnRemote()
        {
            UpdateDistanceDataOnRemote();
        }

        public async UniTask<bool> TryChangeUserNameAsync(string userName, uint cost)
        {
            if(PlayerModel.Bits.Value < cost) return false;
            
            await _remoteDataStorageService.SaveBaseUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.USER_NAME_KEY, userName);
            await _leaderboardService.SaveUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.USER_NAME_KEY,userName);
            PlayerModel.UserName.Value = userName;
            RemoveBits(cost);
            return true;
        }

        public void AddBits(ushort newValue)
        {
            PlayerModel.Bits.Value += newValue;
            _remoteDataStorageService.SaveBaseUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.BITS_KEY, PlayerModel.Bits
                .Value.ToString());
        }

        public void RemoveBits(uint removedValue)
        {
            PlayerModel.Bits.Value -= removedValue;
            _remoteDataStorageService.SaveBaseUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.BITS_KEY, PlayerModel.Bits
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
                UpdateDistanceDataOnRemote();
            }
        }

        private void UpdateDistanceDataOnRemote()
        {
            _remoteDataStorageService.SaveBaseUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.CURRENT_DISTANCE_KEY, 
                PlayerModel.CurrentDistance.Value.ToString(CultureInfo.InvariantCulture));
            UpdateBestDistance(PlayerModel.CurrentDistance.Value);
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
            var userName = await _remoteDataStorageService.LoadBaseUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.USER_NAME_KEY);
            var bits = await _remoteDataStorageService.LoadBaseUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.BITS_KEY);
            var currentDistance = await _remoteDataStorageService.LoadBaseUserDataAsync(PlayerModel.UserId.Value, 
                DatabaseKeyAssets.CURRENT_DISTANCE_KEY);
            
            SetCurrentDistance(double.Parse(currentDistance));
            PlayerModel.Bits.Value = ushort.Parse(bits);
            PlayerModel.UserName.Value = userName;
        }

        private void UpdateBestDistance(double currentDistance)
        {
            if (!(currentDistance > PlayerModel.BestDistance.Value)) return;
            
            PlayerModel.BestDistance.Value = currentDistance;
            _leaderboardService.SaveUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.BEST_DISTANCE_KEY, currentDistance.ToString(CultureInfo
                .InvariantCulture));
            _remoteDataStorageService.SaveBaseUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.BEST_DISTANCE_KEY, currentDistance.ToString
                (CultureInfo.InvariantCulture));
        }
    }
}