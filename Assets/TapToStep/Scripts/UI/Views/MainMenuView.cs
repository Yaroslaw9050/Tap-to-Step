using System;
using Core.Extension.UI;
using Runtime.Audio;
using Runtime.EntryPoints.EventHandlers;
using Runtime.Player;
using Runtime.Player.Perks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views.Upgrades
{
    public class MainMenuView : BaseView
    {
        [Header("Sub-Views")]
        [SerializeField] private UpgradeHolderSubView _upgradeHolderSubView;
        
        [Header("Other settings")]
        [SerializeField] private Button _backButton;
        [SerializeField] private GradientAutoRotation _gradientAutoRotation;

        private PlayerPerkSystem _playerPerkSystem;
        private GameEventHandler _gameEventHandler;
        private PlayerEntryPoint _playerEntryPoint;
        public event Action OnBackButtonClicked;
        
        [Inject]
        public void Constructor(GameEventHandler gameEventHandler, PlayerPerkSystem playerPerkSystem,
            AudioController audioController)
        {
            _gameEventHandler = gameEventHandler;
            _playerPerkSystem = playerPerkSystem;
        }
        
        public void Int(PlayerEntryPoint playerEntryPoint)
        {
            _playerEntryPoint = playerEntryPoint;
            _upgradeHolderSubView.Init(_gameEventHandler, _playerPerkSystem, _playerEntryPoint);
            
            _backButton.onClick.AddListener(BackButtonCLicked);
        }

        public override void ShowView(float duration = 0.5f)
        {
            base.ShowView(duration);
            _gradientAutoRotation.PlayAnimation();
            _upgradeHolderSubView.DisplayActualValues();
        }

        public override void HideView(float duration = 0f)
        {
            base.HideView(duration);
            _gradientAutoRotation.StopAnimation();
        }

        private void BackButtonCLicked()
        {
            _gameEventHandler.InvokeOnUiElementClicked();
            OnBackButtonClicked?.Invoke();
        }
    }
}