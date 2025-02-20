using System;
using CompositionRoot.Enums;
using Core.Extension.UI;
using Core.Service.Leaderboard;
using Runtime.EntryPoints.EventHandlers;
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
        
        private GameEventHandler _gameEventHandler;
        private PlayerPerkSystem _playerPerkSystem;
        private PlayerEntryPoint _playerEntryPoint;

        public void Init(GameEventHandler gameEventHandler,
            PlayerPerkSystem playerPerkSystem, PlayerEntryPoint playerEntryPoint, LeaderboardService leaderboardService)
        {
            _gameEventHandler = gameEventHandler;
            _playerPerkSystem = playerPerkSystem;
            _playerEntryPoint = playerEntryPoint;

            foreach (var subView in _upgradeSubViews)
            {
                subView.Init(leaderboardService, _gameEventHandler);
                subView.OnUpgradeButtonPressed += ButtonPressed;
            }
            
            _gameEventHandler.OnSomeSkillUpgraded += DisplayActualValues;
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

            _gameEventHandler.InvokeOnUiElementClicked();
            if ((int)_playerEntryPoint.PlayerStatistic.Bits < cost) return;
            if(_playerPerkSystem.TryUpgradePerk(perkType) == false) return;
            
            Debug.Log($"cost is: {cost}");
            _gameEventHandler.InvokeOnCollectablesChanged(-cost);

            foreach (var subView in _upgradeSubViews)
            {
                if(subView.PerkType != perkType) continue;
                subView.PlayPurchaseAnimation();
            }
        }
    }
}