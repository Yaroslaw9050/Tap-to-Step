using System;
using UnityEngine;

namespace Runtime.Player.CompositionRoot
{
    [Serializable]
    public class PlayerStatistic
    {
        [SerializeField] private int _bits;
        [SerializeField] private float _distance;
        
        public int Bits => _bits;

        public float Distance => _distance;


        public void AddBits(int value)
        {
            if(value <= 0) return;
            _bits += value;
            
            PlayerPrefs.SetInt("Bits", _bits);
            PlayerPrefs.Save();
        }

        public void RemoveBits(int value)
        {
            if(value <= 0) return;

            _bits -= value;
            PlayerPrefs.SetInt("Bits", _bits);
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
            PlayerPrefs.SetInt("Bits", _bits);
            PlayerPrefs.SetFloat("Distance", _distance);
            PlayerPrefs.Save();
        }

        public void LoadAllDataToVariables()
        {
            _bits = PlayerPrefs.GetInt("Bits", 0);
            _distance = PlayerPrefs.GetFloat("Distance", 0);
        }
    }
}