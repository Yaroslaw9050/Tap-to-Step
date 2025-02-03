using DG.Tweening;
using MPUIKIT;
using UnityEngine;
using DOTween = DG.Tweening.DOTween;

namespace Core.Extension.UI
{
    public class GradientAutoRotation : MonoBehaviour
    {
        [SerializeField] private MPImage[] _buttonGradients;
        [Range(0.5f, 10f)]
        [SerializeField] private float _duration = 1f;

        private Tween _rotationTween;
        private float _rotationValue;
        
        public void PlayAnimation()
        {
            if (_rotationTween != null)
            {
                _rotationTween.Play();
                return;
            }
            
            _rotationTween = DOTween.To(() => _rotationValue, x => {
                    _rotationValue = x;
                    UpdateGradients();
                }, 360f, _duration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }

        public void StopAnimation()
        {
            _rotationTween?.Pause();
        }

        private void UpdateGradients()
        {
            foreach (var buttonGradient in _buttonGradients)
            {
                var temp = buttonGradient.GradientEffect;
                temp.Rotation = _rotationValue;
                
                buttonGradient.GradientEffect = temp;
            }
        }
    }
}