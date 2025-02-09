using UnityEngine;

namespace Runtime.Audio
{
    public class AudioAnimationTrigger: MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip;

        private void OnEnable()
        {
            _audioSource.clip = _audioClip;
            _audioSource.spatialBlend = 1;
        }
        
        public void Play()
        {
            _audioSource.Play();
        }
    }
}