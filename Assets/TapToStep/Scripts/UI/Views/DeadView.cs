using Core.Extension.UI;
using Core.Service.Leaderboard;
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Api;
using Runtime.EntryPoints.EventHandlers;
using Runtime.Player;
using TapToStep.Scripts.Core.Service.AdMob;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace UI.Views.Upgrades
{
    public class DeadView : BaseView
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _continueButton;

        [SerializeField] private TextMeshProUGUI _distanceText;

        private GameEventHandler _gameEventHandler;
        private LeaderboardService _leaderboardService;
        private PlayerEntryPoint _playerEntryPoint;
        private IMobileAdsService _mobileAdsService;

        private const string DEAD_KEY = "DeadCount";
        
        [Inject]
        public void Constructor(GameEventHandler gameEventHandler,
            LeaderboardService leaderboardService, IMobileAdsService mobileAdsService)
        {
            _mobileAdsService = mobileAdsService;
            _gameEventHandler = gameEventHandler;
            _leaderboardService = leaderboardService;
        }

        protected override void SubscribeToEvents()
        {
            
        }

        protected override void UnSubscribeFromEvents()
        {
            
        }

        public override void ShowView(float duration = 0.5f)
        {
            UpdateDeadCounter();
            base.ShowView(duration);
            _distanceText.SetText($"Distance: {ValueConvertor.ToDistance(_playerEntryPoint.PlayerStatistic.Distance)}");
            _leaderboardService.UpdateUserDistanceAsync(_playerEntryPoint.PlayerStatistic.Distance).Forget();
            _continueButton.interactable = false;
            _mobileAdsService.LoadContinueAd();
        }

        public void Init(PlayerEntryPoint playerEntryPoint)
        {
            _playerEntryPoint = playerEntryPoint;
            
            _restartButton.onClick.AddListener(() =>
            {
                _playerEntryPoint.PlayerStatistic.ResetDistance();
                _gameEventHandler.InvokeOnUiElementClicked();

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
            
            _mobileAdsService.OnShowInterstitialAd += InterstitialAdReady;
            _mobileAdsService.OnContinueAdRecorded += OnDeadAdRecorded;
        }

        public void Destruct()
        {
            _restartButton.onClick.RemoveAllListeners();
            _continueButton.onClick.RemoveAllListeners();
            
            _mobileAdsService.OnShowInterstitialAd -= InterstitialAdReady;
            _mobileAdsService.OnContinueAdRecorded -= OnDeadAdRecorded;
        }

        private void InterstitialAdReady(InterstitialAd ad)
        {
            _continueButton.interactable = true;
            
            _continueButton.onClick.RemoveAllListeners();
            _continueButton.onClick.AddListener(() =>
            {
                _mobileAdsService.ShowInterstitialAd(ad);
            });
        }

        private void OnDeadAdRecorded()
        {
            _gameEventHandler.InvokeOnGameResumed();
        }

        private void UpdateDeadCounter()
        {
            var deadCounter = 0;
            if (PlayerPrefs.HasKey(DEAD_KEY))
            { 
                deadCounter = PlayerPrefs.GetInt(DEAD_KEY);
            }
            
            deadCounter++;
            if (deadCounter >= 4)
            {
                _mobileAdsService.LoadAndShowDeadAd();
                deadCounter = 0;
            }
            PlayerPrefs.SetInt(DEAD_KEY, deadCounter);
            PlayerPrefs.Save();
        }
    }
}