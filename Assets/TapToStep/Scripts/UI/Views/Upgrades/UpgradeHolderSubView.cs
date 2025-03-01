using System;
using CompositionRoot.Enums;
using Core.Extension.UI;
using Core.Service.GlobalEvents;
using Core.Service.Leaderboard;
using Runtime.Player;
using Runtime.Player.CompositionRoot;
using Runtime.Player.Perks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Views.Upgrades
{
    [Serializable]
    public class UpgradeHolderSubView
    {
        [SerializeField] private TextMeshProUGUI _bitsText;
        
        [SerializeField] private UpgradeSubView[] _upgradeSubViews;
        
        private GlobalEventsHolder _globalEventsHolder;
        private PlayerPerkSystem _playerPerkSystem;
        private PlayerEntryPoint _playerEntryPoint;

        public void Init(GlobalEventsHolder globalEventsHolder,
            PlayerPerkSystem playerPerkSystem, PlayerEntryPoint playerEntryPoint, LeaderboardService leaderboardService)
        {
            _globalEventsHolder = globalEventsHolder;
            _playerPerkSystem = playerPerkSystem;
            _playerEntryPoint = playerEntryPoint;

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
            
            _bitsText.SetText(ValueConvertor.ToBits(_playerEntryPoint.PlayerStatistic.Bits));
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
            if ((int)_playerEntryPoint.PlayerStatistic.Bits < cost) return;
            if(_playerPerkSystem.TryUpgradePerk(perkType) == false) return;
            
            Debug.Log($"cost is: {cost}");
            //TODO: Fix on collectable changed!
            //_gameEventHandler.InvokeOnCollectablesChanged(-cost);

            foreach (var subView in _upgradeSubViews)
            {
                if(subView.PerkType != perkType) continue;
                subView.PlayPurchaseAnimation();
            }
        }
    }
}