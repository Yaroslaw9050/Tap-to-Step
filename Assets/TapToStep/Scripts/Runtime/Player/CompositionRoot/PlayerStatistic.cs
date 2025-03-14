using System;
using UnityEngine;

namespace Runtime.Player.CompositionRoot
{
    [Serializable]
    public class PlayerStatistic
    {
        [SerializeField] private ulong _bits;
        [SerializeField] private double _distance;
        
        public ulong Bits => _bits;

        public double Distance => _distance;


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
        public void UpdateDistance(double value)
        {
            _distance += value;
            SaveAllData();
        }

        public void SaveAllData()
        {
            PlayerPrefs.SetFloat("Distance", (float)_distance);
            PlayerPrefs.Save();
        }

        public void LoadAllDataToVariables()
        {
            _distance = PlayerPrefs.GetFloat("Distance", 0);
        }
    }
}