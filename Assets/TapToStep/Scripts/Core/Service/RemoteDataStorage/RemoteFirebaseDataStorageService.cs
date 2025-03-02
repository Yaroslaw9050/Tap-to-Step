using System;
using CompositionRoot.Enums;
using CompositionRoot.SO.Player.Logic;
using CompositionRoot.Static;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

namespace Core.Service.RemoteDataStorage
{
    public class RemoteFirebaseDataStorageService: IRemoteDataStorageService
    {
        private DatabaseReference _databaseReference;
        private const string DATABASE_KEY = "users";

        public void Initialise()
        {
            _databaseReference = FirebaseDatabase.DefaultInstance.GetReference(DATABASE_KEY);
        }

        public void CreateStartedFieldsForNewUser(string userId)
        {
            var userName = RandomNameGenerator.GetRandomName();
            _databaseReference.Child(userId).Child("userName").SetValueAsync(userName);
            _databaseReference.Child(userId).Child("bits").SetValueAsync(0);
            _databaseReference.Child(userId).Child("bestDistance").SetValueAsync(0.0);
            _databaseReference.Child(userId).Child("currentDistance").SetValueAsync(0.0);
        }

        public void SaveBaseUserData<T>(string userId, string key, T value)
        {
            _databaseReference.Child(userId).Child(key).SetValueAsync(value);
        }
        public async UniTask<string> LoadBaseUserDataAsync(string userId, string key)
        {
            try
            {
                var reference = _databaseReference.Child(userId).Child(key);
                var snapshot = await reference.GetValueAsync();

                if (!snapshot.Exists)
                {
                    Debug.LogError($"Key |{key}| not found for user {userId}.");
                    return default;
                }

                return snapshot.GetValue(true).ToString();
            }
            catch (Exception e)
            {
                Debug.LogError($"Load base info exception: {e.Message}");
                return null;
            }
        }
        
        public async UniTask<T> LoadBaseClassUserDataAsync<T>(string userId, string key)
        {
            try
            {
                var reference = _databaseReference.Child(userId).Child(key);
                var snapshot = await reference.GetValueAsync();

                if (!snapshot.Exists)
                {
                    Debug.LogError($"Key |{key}| not found for user {userId}.");
                    return default;
                }
                var json = snapshot.GetRawJsonValue();
                var result = JsonUtility.FromJson<T>(json);
                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"Load base info exception: {e.Message}");
                return default;
            }
        }

        public async UniTask SavePerkAsync(string userId, PlayerPerkData perkData)
        {
            var json = JsonUtility.ToJson(perkData);
            await _databaseReference.Child(userId).Child("perks").Child(perkData.upgradeType.ToString())
                .SetRawJsonValueAsync(json);
        }

        public async UniTask<PlayerPerkData> LoadPerkAsync(string userId, PerkType perkType)
        {
            try
            {
                var perkTypeString = perkType.ToString();
                var perkRef = _databaseReference.Child(userId).Child("perks").Child(perkTypeString);
                var snapshot = await perkRef.GetValueAsync();

                if (!snapshot.Exists)
                {
                    Debug.LogError($"Perk {perkTypeString} not found for user {userId}.");
                    return null;
                }

                var json = snapshot.GetRawJsonValue();
                var perkData = JsonUtility.FromJson<PlayerPerkData>(json);
                return perkData;
            }
            catch (Exception e)
            {
                Debug.LogError($"Load perk exception: {e.Message}");
                return null;
            }
        }
    }
}