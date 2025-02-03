using Core.Extension;
using UnityEngine;

namespace UI.Views.Upgrades
{
    public class BaseView: MonoBehaviour
    {
        [SerializeField] private CanvasGroup _thisViewCanvasGroup;
        
        public virtual void ShowView(float duration = 0.5f)
        {
            _thisViewCanvasGroup.SetActive(true, duration);
        }

        public virtual void HideView(float duration = 0f)
        {
            _thisViewCanvasGroup.SetActive(false, duration);
        }
    }
}