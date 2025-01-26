using UnityEngine;

namespace CompositionRoot.SO.Player.Logic
{
    [CreateAssetMenu(fileName = "PlayerSettingSO", menuName = "Player/Parameters", order = 1)]
    public class PlayerSettingSO : ScriptableObject
    {
        [SerializeField] private int  _coins = 0;
        [SerializeField] private float _stepDistance = 1;
        [SerializeField] private float _stepTime = 1;
        [SerializeField] private float _distance;
        public float StepDistance => _stepDistance;

        public float StepTime => _stepTime;

        public int Coins
        {
            get => _coins;
            set
            {
                if (value > 0)
                {
                    _coins = value;
                }
            }
        }

        public float Distance
        {
            get => _distance;
            set
            {
                if(value > _distance)
                {
                    _distance = value;
                }
            }
        }

        public void SaveToMemory()
        {
            PlayerPrefs.SetFloat("Distance", _distance);
            PlayerPrefs.SetFloat("StepTime", _stepTime);
            PlayerPrefs.SetFloat("StepDistance", _stepDistance);
            PlayerPrefs.SetInt("Coins", _coins);
            PlayerPrefs.Save();
        }

        public void LoadFromMemory()
        {
            if(PlayerPrefs.HasKey("Distance") == false) return;
            
            PlayerPrefs.GetFloat("Distance", _distance);
            PlayerPrefs.GetFloat("StepTime", _stepTime);
            PlayerPrefs.GetFloat("StepDistance", _stepDistance);
            PlayerPrefs.GetInt("Coins", _coins);
        }

        public void ClearData()
        {
            PlayerPrefs.DeleteKey("Distance");
            PlayerPrefs.DeleteKey("StepTime");
            PlayerPrefs.DeleteKey("StepDistance");
            PlayerPrefs.DeleteKey("Coins");
        }
    }
}