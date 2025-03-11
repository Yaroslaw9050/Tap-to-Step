using System;
using System.Collections.Generic;
using System.Threading;
using Core.Service.GlobalEvents;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;
using Random = UnityEngine.Random;

namespace Runtime.Audio
{
    public class AudioController : MonoBehaviour
    {
        [Header("Audio Mixers")]
        [SerializeField] private AudioMixerGroup _stepMixer;
        [SerializeField] private AudioMixerGroup _vfxMixer;
        [SerializeField] private AudioMixerGroup _uiMixer;
        [SerializeField] private AudioMixerGroup _musicMixer;

        [Header("Music")] 
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioClip[] _musicClips;

        [Header("Audio Clips")] 
        [SerializeField] private AudioClip _stepClip;

        [SerializeField] private AudioClip _bitCollectedClip;
        [SerializeField] private AudioClip _playerDiedClip;
        [SerializeField] private AudioClip _uiClickClip;
        [SerializeField] private AudioClip _levelUpClip;
        [SerializeField] private AudioClip _nickNameChangedClip;

        private bool _isInitialized;
        private GlobalEventsHolder _globalEventsHolder;
        private AudioSource _stepAudioSource;
        private AudioSource _vfxAudioSource;
        private AudioSource _uiAudioSource;
        private CancellationTokenSource _cts;

        public AudioSource MusicSource => _musicSource;
        

        [Inject]
        public void Constructor(GlobalEventsHolder globalEventsHolder)
        {
            _globalEventsHolder = globalEventsHolder;
        }
        
        public void Initialise()
        {
            if(_isInitialized) return;
            
            _cts = new CancellationTokenSource();

            InitBackgroundMusic();
            InitGlobalAudioSourceSystem();
            
            SubscribeToEvents();
            _isInitialized = true;
        }
        

        private void SubscribeToEvents()
        {
            _globalEventsHolder.PlayerEvents.OnStartMoving += () =>
            {
                PlayShortSound(_stepAudioSource, _stepMixer, _stepClip, Random.Range(0.9f, 1.1f));
            };

            _globalEventsHolder.PlayerEvents.OnDied += () =>
            {
                PlayShortSound(_vfxAudioSource, _vfxMixer, _playerDiedClip);
            };

            _globalEventsHolder.OnCollectablesChanged += () =>
            {
                PlayShortSound(_vfxAudioSource, _vfxMixer, _bitCollectedClip);
            };
            
            _globalEventsHolder.UIEvents.OnClickedOnAnyElements += () =>
            {
                PlayShortSound(_uiAudioSource, _uiMixer, _uiClickClip);
            };

            _globalEventsHolder.OnSomeSkillUpgraded += _ =>
            {
                PlayShortSound(_uiAudioSource, _uiMixer, _levelUpClip);
            };
        }

        private void InitBackgroundMusic()
        {
            if(_musicClips.Length == 0) return;
            
            _musicSource.outputAudioMixerGroup = _musicMixer;
            _musicSource.playOnAwake = false;
            
            _musicSource.clip = _musicClips[Random.Range(0, _musicClips.Length)];
            _musicSource.Play();

            OnMusicEndedAsync(InitBackgroundMusic).Forget();
        }

        private void InitGlobalAudioSourceSystem()
        {
            _stepAudioSource = gameObject.AddComponent<AudioSource>();
            _vfxAudioSource = gameObject.AddComponent<AudioSource>();
            _uiAudioSource = gameObject.AddComponent<AudioSource>();
        }

        private void PlayShortSound(AudioSource targetAudioSource, AudioMixerGroup audioMixerGroup,
            AudioClip clip, float pitch = 1f, float volume = 1f)
        {
            targetAudioSource.outputAudioMixerGroup = audioMixerGroup;
            targetAudioSource.volume = volume;
            targetAudioSource.pitch = pitch;
            targetAudioSource.PlayOneShot(clip, volume);
        }

        private async UniTask OnMusicEndedAsync(Action onComplete)
        {
            while (true)
            {
                await UniTask.WaitForSeconds(1f, cancellationToken: _cts.Token);
                if(_cts.IsCancellationRequested) break;
                if(_musicSource.isPlaying) continue;
                
                onComplete?.Invoke();
                return;
            }
        }
    }
}