using DG.Tweening;
using UnityEngine;

namespace UI.Views
{
    public class TutorialView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        private bool _isTutorialActive;
        
        public void ShowView()
        {
            _isTutorialActive = true;
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(0.2f,0.5f);
        }

        public void HideView()
        {
            if(_isTutorialActive == false) return;
            
            _isTutorialActive = false;
            _canvasGroup.DOFade(0f, 0.5f);
        }
    }
}