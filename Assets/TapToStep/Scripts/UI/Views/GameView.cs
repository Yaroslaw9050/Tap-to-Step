using System;
using System.Threading;
using CompositionRoot.Enums;
using CompositionRoot.SO.Player.Logic;
using Core.Extension.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MPUIKIT;
using Runtime.EntryPoints.EventHandlers;
using Runtime.Player;
using Runtime.Player.CompositionRoot;
using Runtime.Player.Perks;
using TapToStep.Scripts.Core.Service.AdMob;
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
        [SerializeField] private Button _getBitsButton;
        
        [SerializeField] private RectTransform _getBitsRect;
        
        [Header("Bit")]
        [SerializeField] private TextMeshProUGUI _bitText;
        
        [Header("Energy")]
        [SerializeField] private MPImage _energyLine;

        private GameEventHandler _gameEventHandler;
        private PlayerEntryPoint _playerEntryPoint;
        private PlayerPerkSystem _playerPerkSystem;
        private IMobileAdsService _mobileAdsService;

        private CancellationTokenSource _cts;
        
        public event Action OnToMenuButtonPressed;

        [Inject]
        public void Constructor(GameEventHandler gameEventHandler, PlayerPerkSystem playerPerkSystem,
            IMobileAdsService mobileAdsService)
        {
            _gameEventHandler = gameEventHandler;
            _playerPerkSystem = playerPerkSystem;
            _mobileAdsService = mobileAdsService;
        }
        
        public void Init(PlayerEntryPoint playerEntryPoint)
        {
            _cts = new CancellationTokenSource();
            _playerEntryPoint = playerEntryPoint;
            
            _gameEventHandler.OnCollectablesChanged += OnCollectablesChanged;
            _gameEventHandler.OnPlayerStartMoving += OnPlayerStartMoving;
            _gameEventHandler.OnSomeSkillUpgraded += OnSomeSkillUpgraded;
            
            _toMenuButton.onClick.AddListener(ToMenuButtonClicked);
            _getBitsButton.onClick.AddListener(GetBitsButtonClicked);
            
            _distanceText.SetText($"Distance\n{TextMeshProExtension.ConvertToDistance(_playerEntryPoint.PlayerStatistic.Distance)}");
            _bitText.ConvertToBits(_playerEntryPoint.PlayerStatistic.Bits);
            _energyLine.fillAmount = 1f;
            PreloadBitAdsAsync().Forget();
        }

        public void Destruct()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            
            _gameEventHandler.OnSomeSkillUpgraded -= OnSomeSkillUpgraded;
            _gameEventHandler.OnCollectablesChanged -= OnCollectablesChanged;
            _gameEventHandler.OnPlayerStartMoving -= OnPlayerStartMoving;
            
            _getBitsButton.onClick.RemoveListener(GetBitsButtonClicked);
            _toMenuButton.onClick.RemoveListener(ToMenuButtonClicked);
        }

        private async UniTaskVoid PreloadBitAdsAsync()
        {
            _getBitsButton.interactable = false;
            _getBitsRect.anchoredPosition = new Vector2(-_getBitsRect.rect.width, _getBitsRect.anchoredPosition.y);
            await UniTask.WaitForSeconds(40, cancellationToken: _cts.Token);
            _mobileAdsService.LoadRewardBitsAd(BitAdsLoaded);
        }
        

        private void BitAdsLoaded()
        {
            _getBitsButton.interactable = true;
            _getBitsRect.DOMoveX(-20, 1f);
        }

        private void GetBitsButtonClicked()
        {
            _mobileAdsService.ShowRewardedBitsAd(BitAdsShowed);
        }

        private void BitAdsShowed(double value)
        {
            _gameEventHandler.InvokeOnCollectablesChanged((int)value);
            PreloadBitAdsAsync().Forget();
        }

        private void ToMenuButtonClicked()
        {
            OnToMenuButtonPressed?.Invoke();
            _gameEventHandler.InvokeOnUiElementClicked();
        }

        private void OnPlayerStartMoving()
        {
            var stepSpeedDuration = _playerEntryPoint.PlayerSettingSo.StepSpeed - _playerPerkSystem.GetPerkValueByType(PerkType.StepSpeed);
            
            _distanceText.SetText($"Distance\n{TextMeshProExtension.ConvertToDistance(_playerEntryPoint.PlayerStatistic.Distance)}");
            _energyLine.fillAmount = 0f;
            _energyLine.DOFillAmount(1f, stepSpeedDuration).SetEase(Ease.Linear);
        }

        private void OnSomeSkillUpgraded(PerkType _)
        {
            _bitText.ConvertToBits(_playerEntryPoint.PlayerStatistic.Bits);
        }

        private void OnCollectablesChanged(int value)
        {
            _playerEntryPoint.PlayerStatistic.ChangeBits(value);
            _bitText.ConvertToBits(_playerEntryPoint.PlayerStatistic.Bits);
        }
    }
}