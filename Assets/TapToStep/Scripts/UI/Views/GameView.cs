using System;
using CompositionRoot.Enums;
using CompositionRoot.SO.Player.Logic;
using Core.Extension.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MPUIKIT;
using Patterns.ViewModels;
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
        [Header("Buttons:")]
        [SerializeField] private Button _toMenuButton;
        [SerializeField] private Button _getBitsButton;

        [Header("Texts:")]
        [SerializeField] private TextMeshProUGUI _distanceText;
        [SerializeField] private TextMeshProUGUI _bitText;

        [Header("Images:")]
        [SerializeField] private Image _energyLine;
        
        [Header("Other UI elements:")]
        [SerializeField] private RectTransform _getBitsRect;
        
        private GameViewModel _gameViewModel;
        
        [Inject]
        public void Constructor(GameViewModel gameViewModel)
        {
            _gameViewModel = gameViewModel;
        }

        protected override void SubscribeToEvents()
        {
            _toMenuButton.onClick.AddListener(OnToMenuButtonPressed);
            _getBitsButton.onClick.AddListener(OnGetBitsButtonPressed);
        }

        protected override void UnSubscribeFromEvents()
        {
            _toMenuButton.onClick.RemoveListener(OnToMenuButtonPressed);
            _getBitsButton.onClick.RemoveListener(OnGetBitsButtonPressed);
        }

        private void OnToMenuButtonPressed()
        {
            _gameViewModel.ToMenuCommand.Execute();
        }

        private void OnGetBitsButtonPressed()
        {
            _gameViewModel.GetRewardCommand.Execute();
        }

        // private GameEventHandler _gameEventHandler;
        // private PlayerEntryPoint _playerEntryPoint;
        // private PlayerPerkSystem _playerPerkSystem;
        // private IMobileAdsService _mobileAdsService;
        //
        // public event Action OnToMenuButtonPressed;
        //
        // [Inject]
        // public void Constructor(GameEventHandler gameEventHandler, PlayerPerkSystem playerPerkSystem,
        //     IMobileAdsService mobileAdsService)
        // {
        //     _gameEventHandler = gameEventHandler;
        //     _playerPerkSystem = playerPerkSystem;
        //     _mobileAdsService = mobileAdsService;
        // }
        //
        // public void Init(PlayerEntryPoint playerEntryPoint)
        // {
        //     _playerEntryPoint = playerEntryPoint;
        //     
        //     _gameEventHandler.OnCollectablesChanged += OnCollectablesChanged;
        //     _gameEventHandler.OnPlayerStartMoving += OnPlayerStartMoving;
        //     _gameEventHandler.OnSomeSkillUpgraded += OnSomeSkillUpgraded;
        //     
        //     _toMenuButton.onClick.AddListener(ToMenuButtonClicked);
        //     _getBitsButton.onClick.AddListener(GetBitsButtonClicked);
        //     
        //     _distanceText.SetText($"Distance\n{TextMeshProExtension.ConvertToDistance(_playerEntryPoint.PlayerStatistic.Distance)}");
        //     _bitText.ConvertToBits(_playerEntryPoint.PlayerStatistic.Bits);
        //     _energyLine.fillAmount = 1f;
        //     PreloadBitAdsAsync().Forget();
        // }
        //
        // public void Destruct()
        // {
        //     _gameEventHandler.OnSomeSkillUpgraded -= OnSomeSkillUpgraded;
        //     _gameEventHandler.OnCollectablesChanged -= OnCollectablesChanged;
        //     _gameEventHandler.OnPlayerStartMoving -= OnPlayerStartMoving;
        //     
        //     _getBitsButton.onClick.RemoveListener(GetBitsButtonClicked);
        //     _toMenuButton.onClick.RemoveListener(ToMenuButtonClicked);
        // }
        //
        // private async UniTaskVoid PreloadBitAdsAsync()
        // {
        //     _getBitsButton.interactable = false;
        //     _getBitsRect.anchoredPosition = new Vector2(-_getBitsRect.rect.width, _getBitsRect.anchoredPosition.y);
        //     await UniTask.WaitForSeconds(40);
        //     _mobileAdsService.LoadRewardBitsAd(BitAdsLoaded);
        // }
        //
        //
        // private void BitAdsLoaded()
        // {
        //     _getBitsButton.interactable = true;
        //     _getBitsRect.DOMoveX(-20, 1f);
        // }
        //
        // private void GetBitsButtonClicked()
        // {
        //     _mobileAdsService.ShowRewardedBitsAd(BitAdsShowed);
        // }
        //
        // private void BitAdsShowed(double value)
        // {
        //     _gameEventHandler.InvokeOnCollectablesChanged((int)value);
        //     PreloadBitAdsAsync().Forget();
        // }
        //
        // private void ToMenuButtonClicked()
        // {
        //     OnToMenuButtonPressed?.Invoke();
        //     _gameEventHandler.InvokeOnUiElementClicked();
        // }
        //
        // private void OnPlayerStartMoving()
        // {
        //     var stepSpeedDuration = _playerEntryPoint.PlayerSettingSo.StepSpeed - _playerPerkSystem.GetPerkValueByType(PerkType.StepSpeed);
        //     
        //     _distanceText.SetText($"Distance\n{TextMeshProExtension.ConvertToDistance(_playerEntryPoint.PlayerStatistic.Distance)}");
        //     _energyLine.fillAmount = 0f;
        //     _energyLine.DOFillAmount(1f, stepSpeedDuration).SetEase(Ease.Linear);
        // }
        //
        // private void OnSomeSkillUpgraded(PerkType _)
        // {
        //     _bitText.ConvertToBits(_playerEntryPoint.PlayerStatistic.Bits);
        // }
        //
        // private void OnCollectablesChanged(int value)
        // {
        //     _playerEntryPoint.PlayerStatistic.ChangeBits(value);
        //     _bitText.ConvertToBits(_playerEntryPoint.PlayerStatistic.Bits);
        // }
    }
}