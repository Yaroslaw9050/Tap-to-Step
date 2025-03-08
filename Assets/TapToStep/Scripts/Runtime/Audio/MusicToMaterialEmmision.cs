using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Runtime.Audio
{
    public class MusicToMaterialEmmision : MonoBehaviour
    {
        [SerializeField] private Material[] targetMaterial;
        [SerializeField] private Color minEmission = Color.black; 
        [SerializeField] private Color maxEmission = Color.white; 
        [SerializeField] private int frequencyRange = 1; 
        [SerializeField] private float sensitivity = 3f; 
        [SerializeField] private float lerpSpeed = 6f;

        private AudioSource _audioSource;
        private float[] spectrumData = new float[512];
        private float currentIntensity = 2;

        public void Initialise(AudioSource musicSource)
        {
            _audioSource = musicSource;
            if (_audioSource != null && targetMaterial != null)
            {
                foreach (var mat in targetMaterial)
                {
                    mat.EnableKeyword("_EMISSION");
                }
                StartListening().Forget();
            }
        }

        private async UniTaskVoid StartListening()
        {
            while (true)
            {
                await UniTask.NextFrame(); 
                
                if (_audioSource != null && targetMaterial != null)
                {
                    _audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
                    
                    float sum = 0f;
                    for (int i = 0; i < frequencyRange; i++)
                    {
                        sum += spectrumData[i];
                    }
                    
                    float intensity = sum * sensitivity;
                    currentIntensity = Mathf.Lerp(currentIntensity, intensity, Time.deltaTime * lerpSpeed);
                    Color newEmission = new Color(currentIntensity, currentIntensity, currentIntensity);
                    
                    foreach (var mat in targetMaterial)
                    {
                        mat.SetColor("_EmissionColor", newEmission);
                    }
                }
            }
        }
    }
}