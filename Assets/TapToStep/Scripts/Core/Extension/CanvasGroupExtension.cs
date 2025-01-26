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
            group.DOFade(isActive ? 1 : 0, fadeTime);
            group.interactable = isActive;
            group.blocksRaycasts = isActive;
        }
    }
}