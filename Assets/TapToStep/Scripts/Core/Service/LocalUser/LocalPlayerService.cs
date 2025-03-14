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
            var roundValue = Math.Round(currentDistance, 1);
            PlayerModel.CurrentDistance.Value = roundValue;
        }

        public void ResetCurrentDistance()
        {
            _remoteDataStorageService.SaveUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.CURRENT_DISTANCE_KEY, 0.0).Forget();
            PlayerModel.CurrentDistance.Value = 0;
        }
        
        public void SetBestDistance(double bestDistance)
        {
            var roundValue = Math.Round(bestDistance, 1);
            PlayerModel.BestDistance.Value = roundValue;
        }

        public void ForceUpdateDistanceDataOnRemote()
        {
            UpdateDistanceDataOnRemote();
        }

        public async UniTask<bool> TryChangeUserNameAsync(string userName, uint cost)
        {
            if(PlayerModel.Bits.Value < cost) return false;
            
            await _remoteDataStorageService.SaveUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.USER_NAME_KEY, userName);
            await _leaderboardService.SaveUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.USER_NAME_KEY,userName);
            PlayerModel.UserName.Value = userName;
            RemoveBits(cost);
            return true;
        }

        public void AddBits(ushort newValue)
        {
            PlayerModel.Bits.Value += newValue;
            _remoteDataStorageService.SaveUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.BITS_KEY, PlayerModel.Bits
                .Value.ToString());
        }

        public void RemoveBits(uint removedValue)
        {
            PlayerModel.Bits.Value -= removedValue;
            _remoteDataStorageService.SaveUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.BITS_KEY, PlayerModel.Bits
                .Value.ToString());
        }

        public void AddDistance(double distance)
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
            _remoteDataStorageService.SaveUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.CURRENT_DISTANCE_KEY, PlayerModel.CurrentDistance.Value.ToString(CultureInfo.InvariantCulture));
            UpdateBestDistance(PlayerModel.CurrentDistance.Value);
        }

        public double GetStepLenght()
        {
            return _startupPlayerSetting.StepLenght + _playerPerkSystem.GetPerkValueByType(PerkType.StepLenght);
        }

        public double GetStepTime()
        {
            return _startupPlayerSetting.StepSpeed - _playerPerkSystem.GetPerkValueByType(PerkType.StepSpeed);
        }

        public double GetTurnSpeed()
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
            var currentDistance = await _remoteDataStorageService.LoadBaseUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.CURRENT_DISTANCE_KEY);

            if (double.TryParse(currentDistance, out var convertedDistance) == false) return;
            if (ulong.TryParse(bits, out var convertedBits) == false) return;
            if (string.IsNullOrEmpty(userName)) return;
            
            SetCurrentDistance(convertedDistance);
            PlayerModel.Bits.Value = convertedBits;
            PlayerModel.UserName.Value = userName;
        }

        private void UpdateBestDistance(double currentDistance)
        {
            if ((currentDistance > PlayerModel.BestDistance.Value) == false) return;
            
            PlayerModel.BestDistance.Value = currentDistance;
            _leaderboardService.SaveUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.BEST_DISTANCE_KEY, currentDistance.ToString(CultureInfo.InvariantCulture));
            _remoteDataStorageService.SaveUserDataAsync(PlayerModel.UserId.Value, DatabaseKeyAssets.BEST_DISTANCE_KEY, currentDistance.ToString(CultureInfo.InvariantCulture));
        }
    }
}