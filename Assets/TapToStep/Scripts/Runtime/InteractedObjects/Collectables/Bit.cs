using DG.Tweening;
using UnityEngine;

namespace Runtime.InteractedObjects.Collectables
{
    public class Bit : MonoBehaviour
    {
        [Range(1, 100)]
        [SerializeField] private ushort _value = 5;

        private Tween _collectTween;
        
        public ushort Value => _value;


        public void Init()
        {
            _collectTween = transform.DOScale(0f, 0.25f).SetEase(Ease.InFlash);
            _collectTween.Pause();
            _collectTween.OnComplete(() =>
            {
                _collectTween.Pause();
                _collectTween.Kill();
                Destroy(gameObject);
            });
        }

        public void Collect()
        {
            _collectTween.Play();
        }
    }
}