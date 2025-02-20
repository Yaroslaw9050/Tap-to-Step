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


        public void ChangeBits(int value)
        {
            _bits += value;
            
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

        public void SetDistance(float currentDistance)
        {
            _distance = currentDistance;
        }

        public void SaveAllData()
        {
            PlayerPrefs.SetInt("Bits", _bits);
            PlayerPrefs.Save();
        }

        public void LoadAllDataToVariables()
        {
            _bits = PlayerPrefs.GetInt("Bits", 0);
        }
    }
}