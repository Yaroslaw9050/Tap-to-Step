using DG.Tweening;
using UnityEngine;

namespace Runtime.InteractedObjects.Collectables
{
    public class Coin : MonoBehaviour
    {
        [Range(1, 100)]
        [SerializeField] private int _value = 5;
        
        [Header("Animations")] 
        [SerializeField] private Transform _targetTransform;
        [Range(0.5f, 5f)]
        [SerializeField] private float _rotationDuration = 2f;

        private Tween _rotationTween;
        
        public int Value => _value;


        public void Init()
        {
            var rotateValue = Vector3.up * 360f;
            _rotationTween = 
                _targetTransform.DOLocalRotate(rotateValue, _rotationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
            
            _rotationTween.Play();
        }

        public void Collect()
        {
            _rotationTween.Pause();
            _rotationTween.Kill();
            Destroy(gameObject);
        }
    }
}