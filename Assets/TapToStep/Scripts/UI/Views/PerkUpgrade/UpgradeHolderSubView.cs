using System;
using CompositionRoot.Enums;
using Core.Service.GlobalEvents;
using Core.Service.Leaderboard;
using Core.Service.LocalUser;
using Runtime.Player.Upgrade;
using UnityEngine;

namespace UI.Views.Upgrades
{
    [Serializable]
    public class UpgradeHolderSubView
    {
        [SerializeField] private UpgradeSubView[] _upgradeSubViews;
        
        private GlobalEventsHolder _globalEventsHolder;
        private PlayerPerkSystem _playerPerkSystem;
        private LocalPlayerService _localPlayerService;

        public void Init(GlobalEventsHolder globalEventsHolder,
            PlayerPerkSystem playerPerkSystem, LocalPlayerService localPlayerService,
            LeaderboardService leaderboardService)
        {
            _globalEventsHolder = globalEventsHolder;
            _playerPerkSystem = playerPerkSystem;
            _localPlayerService = localPlayerService;

            foreach (var subView in _upgradeSubViews)
            {
                subView.Init(leaderboardService, _globalEventsHolder);
                subView.OnUpgradeButtonPressed += ButtonPressed;
            }
            
            _globalEventsHolder.OnSomeSkillUpgraded += DisplayActualValues;
        }

        public void DisplayActualValues()
        {
            DisplayActualValues(PerkType.TurnSpeed);
            DisplayActualValues(PerkType.StepSpeed);
            DisplayActualValues(PerkType.StepLenght);
        }

        private void DisplayActualValues(PerkType perk)
        {
            if(perk == PerkType.None) return;
            
            var cost = _playerPerkSystem.GetPerkPrice(perk);

            foreach (var subView in _upgradeSubViews)
            {
                if(subView.PerkType != perk) continue;
                subView.UpdateElementsData(_playerPerkSystem.GetPerkLevel(perk), cost);
            }
        }

        private void ButtonPressed(PerkType perkType)
        {
            var cost = _playerPerkSystem.GetPerkPrice(perkType);

            _globalEventsHolder.UIEvents.InvokeClickedOnAnyElements();
            if (_localPlayerService.PlayerModel.Bits.Value < (ulong)cost) return;
            if(_playerPerkSystem.TryUpgradePerk(perkType) == false) return;
            
            _localPlayerService.RemoveBits((uint)cost);
            
            _globalEventsHolder.InvokeOnCollectablesChanged();

            foreach (var subView in _upgradeSubViews)
            {
                if(subView.PerkType != perkType) continue;
                subView.PlayPurchaseAnimation();
            }
        }
    }
}