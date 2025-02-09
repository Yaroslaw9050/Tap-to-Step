using System;
using CompositionRoot.Enums;
using CompositionRoot.SO.Player.Logic;
using DG.Tweening;
using MPUIKIT;
using Runtime.EntryPoints.EventHandlers;
using Runtime.Player;
using Runtime.Player.CompositionRoot;
using Runtime.Player.Perks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views.Upgrades
{
    public class GameView : BaseView
    {
        [Header("Base")]
        [SerializeField] private TextMeshProUGUI _distanceText;
        [SerializeField] private Button _toMenuButton;
        
        [Header("Bit")]
        [SerializeField] private TextMeshProUGUI _bitText;
        
        [Header("Energy")]
        [SerializeField] private MPImage _energyLine;

        private GameEventHandler _gameEventHandler;
        private PlayerEntryPoint _playerEntryPoint;
        private PlayerPerkSystem _playerPerkSystem;
        
        public event Action OnToMenuButtonPressed;

        [Inject]
        public void Constructor(GameEventHandler gameEventHandler, PlayerPerkSystem playerPerkSystem)
        {
            _gameEventHandler = gameEventHandler;
            _playerPerkSystem = playerPerkSystem;
        }
        
        public void Init(PlayerEntryPoint playerEntryPoint)
        {
            _playerEntryPoint = playerEntryPoint;
            
            _gameEventHandler.OnCollectablesChanged += OnCollectablesChanged;
            _gameEventHandler.OnPlayerStartMoving += OnPlayerStartMoving;
            _gameEventHandler.OnSomeSkillUpgraded += OnSomeSkillUpgraded;
            _toMenuButton.onClick.AddListener(ToMenuButtonClicked);
            
            _distanceText.SetText($"Distance\n{ConvertToDistance(_playerEntryPoint.PlayerStatistic.Distance)}");
            _bitText.SetText(ConvertToBits(_playerEntryPoint.PlayerStatistic.Bits));
            _energyLine.fillAmount = 1f;
        }

        public void Destruct()
        {
            _gameEventHandler.OnSomeSkillUpgraded -= OnSomeSkillUpgraded;
            _gameEventHandler.OnCollectablesChanged -= OnCollectablesChanged;
            _gameEventHandler.OnPlayerStartMoving -= OnPlayerStartMoving;
            _toMenuButton.onClick.RemoveListener(ToMenuButtonClicked);
        }

        private void ToMenuButtonClicked()
        {
            OnToMenuButtonPressed?.Invoke();
            _gameEventHandler.InvokeOnUiElementClicked();
        }

        private void OnPlayerStartMoving()
        {
            var stepSpeedDuration = _playerEntryPoint.PlayerSettingSo.StepSpeed - _playerPerkSystem.GetPerkValueByType(PerkType.StepSpeed);
            
            _distanceText.SetText($"Distance\n{ConvertToDistance(_playerEntryPoint.PlayerStatistic.Distance)}");
            _energyLine.fillAmount = 0f;
            _energyLine.DOFillAmount(1f, stepSpeedDuration).SetEase(Ease.Linear);
        }

        private void OnSomeSkillUpgraded(PerkType _)
        {
            _bitText.SetText(ConvertToBits(_playerEntryPoint.PlayerStatistic.Bits));
        }

        private void OnCollectablesChanged(int value)
        {
            _playerEntryPoint.PlayerStatistic.AddBits(value);
            _bitText.SetText(ConvertToBits(_playerEntryPoint.PlayerStatistic.Bits));
        }
        
        private  string ConvertToBits(int rawBitValue)
        {
            string result = "";

            switch (rawBitValue)
            {
                case < 1000:
                    return rawBitValue.ToString();
                case >= 1000 and < 1000000: 
                {
                    int k = rawBitValue / 1000; 
                    result += $"{k}k";
                
                    return result.Trim();
                }
            }
            
            var v = rawBitValue / 1000000; 
            result += $"{v}m";
                
            return result.Trim(); 
        }
        
        private  string ConvertToDistance(float distance)
        {
            var meters = (int)distance;
            var centimeters = (int)((distance - meters) * 100); 
            
            var result = "";
            
            if (meters >= 1000)
            {
                int kilometers = meters / 1000; 
                meters = meters % 1000; 
                result += $"{kilometers}km";
                
                if (meters > 0)
                {
                    result += $" {meters}m";
                }
            }
            else
            {
                if (meters > 0)
                {
                    result += $"{meters}m";
                }
                
                if (centimeters > 0)
                {
                    result += $" {centimeters}cm";
                }
            }

            return result.Trim();
        }
    }
}