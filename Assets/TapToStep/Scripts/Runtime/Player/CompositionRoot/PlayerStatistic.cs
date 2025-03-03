using System;
using UnityEngine;

namespace Runtime.Player.CompositionRoot
{
    [Serializable]
    public class PlayerStatistic
    {
        [SerializeField] private ulong _bits;
        [SerializeField] private float _distance;
        
        public ulong Bits => _bits;

        public float Distance => _distance;


        public void ChangeBits(ulong value)
        {
            _bits += value;
            
            PlayerPrefs.Save();
        }

        public void ResetDistance()
        {
            _distance = 0;
            SaveAllData();
        }
        public void UpdateDistance(float value)
        {
            _distance += value;
            SaveAllData();
        }

        public void SaveAllData()
        {
            PlayerPrefs.SetFloat("Distance", _distance);
            PlayerPrefs.Save();
        }

        public void LoadAllDataToVariables()
        {
            _distance = PlayerPrefs.GetFloat("Distance", 0);
        }
    }
}