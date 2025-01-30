using Core.Extension;
using Runtime.EntryPoints.EventHandlers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Views
{
    public class DeadView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _continueButton;

        private GameEventHandler _gameEventHandler;
        
        public void Init(GameEventHandler gameEventHandler)
        {
            _gameEventHandler = gameEventHandler;
            
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
        
        
        public void ShowView()
        {
            _canvasGroup.SetActive(true, 0.5f);
        }

        public void HideView()
        {
            _canvasGroup.SetActive(false, 0.5f);
        }
    }
}