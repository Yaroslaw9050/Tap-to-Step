using DG.Tweening;
using UnityEngine;

namespace UI.Views
{
    public class TutorialView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        private bool _isTutorialActive = false;
        
        public void ShowView()
        {
            _isTutorialActive = true;
            _canvasGroup.alpha = 0.2f;
        }

        public void HideView()
        {
            if(_isTutorialActive == false) return;
            
            _isTutorialActive = false;
            _canvasGroup.DOFade(0f, 0.5f);
        }
    }
}