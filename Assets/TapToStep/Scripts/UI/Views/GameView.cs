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
        
        [Header("Bit")]
        [SerializeField] private TextMeshProUGUI _bitText;
        
        [Header("Energy")]
        [SerializeField] private MPImage _energyLine;

        private GameEventHandler _gameEventHandler;
        private PlayerSettingSO _playerSetting;

        public void Init(GameEventHandler eventHandler, PlayerSettingSO playerSetting)
        {
            _gameEventHandler = eventHandler;
            _playerSetting = playerSetting;
            
            _gameEventHandler.OnCollectablesChanged += OnCollectablesChanged;
            _gameEventHandler.OnPlayerStartMoving += OnPlayerStartMoving;
            _toMenuButton.onClick.AddListener(ToMenuButtonClicked);
            
            _distanceText.SetText($"Distance\n{ConvertToDistance(_playerSetting.Distance)}");
            _bitText.SetText(ConvertToBits(_playerSetting.Bits));
            _energyLine.fillAmount = 1f;
        }

        public void Destruct()
        {
            _gameEventHandler.OnCollectablesChanged -= OnCollectablesChanged;
            _gameEventHandler.OnPlayerStartMoving -= OnPlayerStartMoving;
            _toMenuButton.onClick.RemoveListener(ToMenuButtonClicked);
            Debug.Log("Game view destroyed!");
        }

        public void ShowView()
        {
            _canvasGroup.SetActive(true, 0.5f);
        }

        public void HideView()
        {
            _canvasGroup.SetActive(false, 0.5f);
        }

        private void ToMenuButtonClicked()
        {
            _gameEventHandler.InvokeOnUiElementClicked();
        }

        private void OnPlayerStartMoving()
        {
            _distanceText.SetText($"Distance\n{ConvertToDistance(_playerSetting.Distance)}");
            _energyLine.fillAmount = 0f;
            _energyLine.DOFillAmount(1f, _playerSetting.StepTime).SetEase(Ease.Linear);
        }

        private void OnCollectablesChanged(int value)
        {
            _playerSetting.Bits += value;
            _bitText.SetText(ConvertToBits(_playerSetting.Bits));
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