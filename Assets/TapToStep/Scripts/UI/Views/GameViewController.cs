using CompositionRoot.SO.Player.Logic;
using Runtime.EntryPoints.EventHandlers;
using UnityEngine;

namespace UI.Views
{
    public class GameViewController : MonoBehaviour
    {
        [Header("Views")]
        [SerializeField] private GameView _gameView;
        [SerializeField] private MainMenuView _mainMenuView;
        [SerializeField] private DeadView _deadView;
        [SerializeField] private TutorialView _tutorialView;
        [SerializeField] private LeaderBoardView _leaderBoardView;

        private GlobalEventHandler _globalEventHandler;
        private PlayerSettingSO _playerSettingSo;
        
        public void Init(GlobalEventHandler globalEventHandler, PlayerSettingSO playerSetting)
        {
            _playerSettingSo = playerSetting;
            _globalEventHandler = globalEventHandler;
            _gameView.Init(_globalEventHandler, playerSetting);
            _deadView.Init(_globalEventHandler);
            
            _tutorialView.ShowView();
            _gameView.ShowView();
            _deadView.HideView();
            
            _globalEventHandler.OnPlayerStartMoving += PlayerStartMoving;
            _globalEventHandler.OnPlayerDied += OnPlayerDied;
        }

        private void PlayerStartMoving()
        {
            _tutorialView.HideView();
        }

        private void OnPlayerDied()
        {
            _gameView.HideView();
            _deadView.ShowView();
        }
    }
}