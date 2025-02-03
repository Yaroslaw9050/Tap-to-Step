using System;
using CompositionRoot.Enums;
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
            PlayerPerkSystem playerPerkSystem, PlayerEntryPoint playerEntryPoint)
        {
            _gameEventHandler = gameEventHandler;
            _playerPerkSystem = playerPerkSystem;
            _playerEntryPoint = playerEntryPoint;

            foreach (var subView in _upgradeSubViews)
            {
                subView.Init();
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
            
            _bitsText.text = _playerEntryPoint.PlayerStatistic.Bits.ToString();
            var cost = _playerPerkSystem.GetPerkPrice(perk);

            foreach (var subView in _upgradeSubViews)
            {
                if(subView.PerkType != perk) continue;
                Debug.Log($"Foreach. Enter with perk: {perk} | Cost: {cost}");
                subView.UpdateElementsData(_playerPerkSystem.GetPerkLevel(perk), cost);
            }
        }

        private void ButtonPressed(PerkType perkType)
        {
            var cost = _playerPerkSystem.GetPerkPrice(perkType);

            _gameEventHandler.InvokeOnUiElementClicked();
            if (_playerEntryPoint.PlayerStatistic.Bits < cost) return;
            if(_playerPerkSystem.TryUpgradePerk(perkType) == false) return;
            
            _playerEntryPoint.PlayerStatistic.RemoveBits(cost);

            foreach (var subView in _upgradeSubViews)
            {
                if(subView.PerkType != perkType) continue;
                subView.PlayPurchaseAnimation();
            }

            _gameEventHandler.InvokeSomePlayerSkillUpgraded(perkType);
        }
    }
}