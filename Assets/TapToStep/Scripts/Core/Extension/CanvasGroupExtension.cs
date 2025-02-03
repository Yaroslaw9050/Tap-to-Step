using DG.Tweening;
using UnityEngine;

namespace Core.Extension
{
    public static class CanvasGroupExtension
    {
        public static void SetActive(this CanvasGroup group, bool isActive)
        {
            group.alpha = isActive ? 1 : 0;
            group.interactable = isActive;
            group.blocksRaycasts = isActive;
        }
        
        public static void SetActive(this CanvasGroup group, bool isActive, float fadeTime)
        {
            if (isActive == false)
            {
                group.interactable = false;
                group.blocksRaycasts = false;
            }
            
            group.DOFade(isActive ? 1 : 0, fadeTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (isActive == false) return;
                group.interactable = true;
                group.blocksRaycasts = true;
            });
        }
    }
}