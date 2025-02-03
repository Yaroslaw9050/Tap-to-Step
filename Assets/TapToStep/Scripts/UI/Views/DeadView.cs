using Runtime.EntryPoints.EventHandlers;
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
        
        [Inject]
        public void Constructor(GameEventHandler gameEventHandler)
        {
            _gameEventHandler = gameEventHandler;
        }
        
        public void Init()
        {
            _restartButton.onClick.AddListener(() =>
            {
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