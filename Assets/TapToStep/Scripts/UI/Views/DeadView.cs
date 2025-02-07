using Core.Service.Leaderboard;
using Cysharp.Threading.Tasks;
using Runtime.EntryPoints.EventHandlers;
using Runtime.Player;
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

        private GameEventHandler _gameEventHandler;
        private LeaderboardService _leaderboardService;
        private PlayerEntryPoint _playerEntryPoint;
        
        [Inject]
        public void Constructor(GameEventHandler gameEventHandler,
            LeaderboardService leaderboardService)
        {
            _gameEventHandler = gameEventHandler;
            _leaderboardService = leaderboardService;
        }

        public override void ShowView(float duration = 0.5f)
        {
            base.ShowView(duration);
            _leaderboardService.UpdateUserDistanceAsync(_playerEntryPoint.PlayerStatistic.Distance).Forget();
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
        }

        public void Destruct()
        {
            _restartButton.onClick.RemoveAllListeners();
        }
    }
}