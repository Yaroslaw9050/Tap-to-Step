using Core.Extension;
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

        public void Init()
        {
            _restartButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
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