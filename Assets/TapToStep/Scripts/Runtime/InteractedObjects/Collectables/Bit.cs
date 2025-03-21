using DG.Tweening;
using UnityEngine;

namespace Runtime.InteractedObjects.Collectables
{
    public class Bit : MonoBehaviour
    {
        [Range(1, 100)]
        [field: SerializeField] public ushort Value { get; private set; } = 5;
        
        [Range(0,100)]
        [field: SerializeField] public float SpawnChance { get; private set; }
        
        private Tween _collectTween;


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