using CompositionRoot.SO.Player.Logic;
using Core.Extension;
using DG.Tweening;
using MPUIKIT;
using Runtime.EntryPoints.EventHandlers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class GameView : MonoBehaviour
    {
        [Header("Base")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _distanceText;
        [SerializeField] private Button _toMenuButton;

        [Header("Coin")]
        [SerializeField] private TextMeshProUGUI _coinsText;
        
        [Header("Energy")]
        [SerializeField] private MPImage _energyLine;

        private GlobalEventHandler _globalEventHandler;
        private PlayerSettingSO _playerSetting;

        public void Init(GlobalEventHandler eventHandler, PlayerSettingSO playerSetting)
        {
            _globalEventHandler = eventHandler;
            _playerSetting = playerSetting;
            
            _globalEventHandler.OnCollectablesChanged += OnCollectablesChanged;
            _globalEventHandler.OnPlayerStartMoving += OnPlayerStartMoving;
            
            _distanceText.SetText($"Distance\n{ConvertToDistance(_playerSetting.Distance)}");
            _coinsText.SetText(ConvertToCoin(_playerSetting.Coins));
            _energyLine.fillAmount = 1f;
        }

        public void ShowView()
        {
            _canvasGroup.SetActive(true, 0.5f);
        }

        public void HideView()
        {
            _canvasGroup.SetActive(false, 0.5f);
        }

        private void OnPlayerStartMoving()
        {
            _distanceText.SetText($"Distance\n{ConvertToDistance(_playerSetting.Distance)}");
            _energyLine.fillAmount = 0f;
            _energyLine.DOFillAmount(1f, _playerSetting.StepTime).SetEase(Ease.Linear);
        }

        private void OnCollectablesChanged(int value)
        {
            _playerSetting.Coins += value;
            _coinsText.SetText(ConvertToCoin(_playerSetting.Coins));
        }
        
        private  string ConvertToCoin(int rawCoinValue)
        {
            string result = "";

            switch (rawCoinValue)
            {
                case < 1000:
                    return rawCoinValue.ToString();
                case >= 1000 and < 1000000: 
                {
                    int k = rawCoinValue / 1000; 
                    result += $"{k}k";
                
                    return result.Trim();
                }
            }
            
            var v = rawCoinValue / 1000000; 
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