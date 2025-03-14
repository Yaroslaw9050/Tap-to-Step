using UnityEngine;

namespace Core.Service.LocalDataStorage
{
    public class LocalDataStorageService: ILocalDataStorageService
    {
        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void SaveString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public bool TryLoadString(string key, out string value)
        {
            if (PlayerPrefs.HasKey(key))
            {
                value = PlayerPrefs.GetString(key);
                return true;
            }
            
            Debug.LogWarning($"Local data storage | {key} - invalid key!");
            
            value = null;
            return false;
        }
        
        public void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }
        
        public bool TryLoadInt(string key, out int value)
        {
            if (PlayerPrefs.HasKey(key))
            {
                value = PlayerPrefs.GetInt(key);
                return true;
            }
            
            Debug.LogWarning($"Local data storage | {key} - invalid key!");
            
            value = 0;
            return false;
        }
    }
}