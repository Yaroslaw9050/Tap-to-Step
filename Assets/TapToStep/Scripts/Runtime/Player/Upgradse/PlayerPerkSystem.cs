using System;
using System.Linq;
using CompositionRoot.Enums;
using CompositionRoot.SO.Player.Logic;
using Core.Service.LocalUser;
using Core.Service.RemoteDataStorage;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Runtime.Player.Upgrade
{
    public class PlayerPerkSystem : MonoBehaviour
    {
        [SerializeField] private PlayerPerkSO[] _supportedUpgrades;
        
        private IRemoteDataStorageService _remoteDataStorageService;
        private LocalPlayerService _localPlayerService;
        
        [Inject]
        public void Constructor(IRemoteDataStorageService remoteDataStorageService, LocalPlayerService localPlayerService)
        {
            _remoteDataStorageService = remoteDataStorageService;
            _localPlayerService = localPlayerService;
        }

        public PlayerPerkSO GetPerkByType(PerkType type)
        {
            return _supportedUpgrades.First(upgrade => upgrade.UpgradeType == type);
        }

        public int GetPerkLevel(PerkType perkType)
        {
            var perk = GetPerkByType(perkType);
            if (perk.CurrentLevel == perk.MaxLevel)
            {
                return -1;
            }
            return perk.CurrentLevel;
        }

        public int GetPerkPrice(PerkType type)
        {
            var perk = GetPerkByType(type);
            return perk.UpgradeCost;
        }

        public bool TryUpgradePerk(PerkType type)
        {
            var perk = GetPerkByType(type);
            if (perk.UpgradePerk())
            {
                SaveToRemoteAsync(perk.UpgradeType).Forget();
                return true;
            }

            return false;
        }

        public async UniTask LoadAllPerksAsync()
        {
            foreach (var perk in _supportedUpgrades)
            {
                perk.Reset();
                await LoadFromRemoteAsync(perk.UpgradeType);
            }
        }

        public double GetPerkValueByType(PerkType type)
        {
            var perk = GetPerkByType(type);
            var raw = perk.CurrentLevel / 20f;
            var rounded = (double)Math.Round(raw, 1);
            return rounded;
        }

        private async UniTask SaveToRemoteAsync(PerkType type)
        {
            var perk = GetPerkByType(type);
            var data = new PlayerPerkData(perk);
            await _remoteDataStorageService.SavePerkAsync(_localPlayerService.PlayerModel.UserId.Value, data);
        }

        private async UniTask LoadFromRemoteAsync(PerkType type)
        {
            var data = await _remoteDataStorageService.LoadPerkAsync(_localPlayerService.PlayerModel.UserId.Value, type);
            var perk = GetPerkByType(type);
            perk.WriteData(data);
        }
    }
}