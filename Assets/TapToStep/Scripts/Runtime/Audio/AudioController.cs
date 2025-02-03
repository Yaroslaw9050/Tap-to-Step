using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.EntryPoints.EventHandlers;
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
        
        private bool _isInitialized;
        private GameEventHandler _gameEventHandler;
        private CancellationTokenSource _cts;
        
        private readonly List<AudioSource> r_stepsAudioSources = new(10);

        [Inject]
        public void Constructor(GameEventHandler gameEventHandler)
        {
            _gameEventHandler = gameEventHandler;
        }
        
        public void Init()
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
            _gameEventHandler.OnPlayerStartMoving += () =>
            {
                PlayShortSound(_stepMixer, _stepClip, Random.Range(0.9f, 1.1f));
            };

            _gameEventHandler.OnPlayerDied += () =>
            {
                PlayShortSound(_vfxMixer, _playerDiedClip);
            };

            _gameEventHandler.OnCollectablesChanged += _ =>
            {
                PlayShortSound(_vfxMixer, _bitCollectedClip);
            };
            
            _gameEventHandler.OnUiElementClicked += () =>
            {
                PlayShortSound(_uiMixer, _uiClickClip);
            };

            _gameEventHandler.OnSomeSkillUpgraded += _ =>
            {
                PlayShortSound(_uiMixer, _levelUpClip);
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
            var stepAudio = new GameObject("GlobalAudioSource");
            stepAudio.transform.SetParent(transform);

            for (var i = 0; i < r_stepsAudioSources.Capacity; i++)
            {
                var temp = stepAudio.AddComponent<AudioSource>();
                temp.playOnAwake = false;
                r_stepsAudioSources.Add(temp);
            }
        }

        private void PlayShortSound(AudioMixerGroup audioMixerGroup,
            AudioClip clip, float pitch = 1f, float volume = 1f)
        {
            foreach (var audioSource in r_stepsAudioSources)
            {
                if(audioSource.isPlaying) continue;
                audioSource.outputAudioMixerGroup = audioMixerGroup;
                audioSource.clip = clip;
                audioSource.volume = volume;
                audioSource.pitch = pitch;
                audioSource.Play();
                return;
            }
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