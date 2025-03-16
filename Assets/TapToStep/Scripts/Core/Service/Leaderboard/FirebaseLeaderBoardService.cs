using System;
using System.Collections.Generic;
using System.Linq;
using CompositionRoot.Constants;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

namespace Core.Service.Leaderboard
{
    public sealed class FirebaseLeaderBoardService: ILeaderboardService
    {
        private DatabaseReference _databaseReference;
        private LeaderboardUser _leaderboardUser;

        private string _userId;
        private long _totalUsers;
        
        private const string DATABASE_KEY = "leaderboard";
        
        
        public void Initialise()
        {
            _databaseReference = FirebaseDatabase.DefaultInstance.GetReference(DATABASE_KEY);
        }

        public async UniTask CheckAllUserFieldsAsync(string userId)
        {
            var result = await CheckUserHasAllDataAsync(userId);
            if (result == false)
            {
                await CreateNewUserDataAsync(userId);
            }
        }

        public async UniTask SaveUserDataAsync<T>(string userId, string key, T value)
        {
            await _databaseReference.Child(userId).Child(key)
                .SetValueAsync(value);
        }

        public async UniTask<string> LoadUserDataAsync(string userId, string key)
        {
            try
            {
                var task = _databaseReference.Child(userId).Child(key).GetValueAsync();
                await task;
                if (task.IsCompleted == false)
                {
                    Debug.LogError($"Request user faulted: {task.Exception?.Message}");
                    return string.Empty;
                }
                var snapshot = task.Result;
                return snapshot.GetValue(true).ToString();
                
            }
            catch (Exception e)
            {
                Debug.LogError($"Any user data not found! | {e.Message}");
                return string.Empty;
            }
        }

        private async UniTask<bool> CheckUserHasAllDataAsync(string userId)
        {
            try
            {
                var task = _databaseReference.Child(userId).GetValueAsync();
                await task;
                if (task.IsCompleted == false)
                {
                    Debug.LogError($"Request user faulted: {task.Exception?.Message}");
                    return task.IsCompleted;
                }
                var snapshot = task.Result;
                if (snapshot is not { HasChildren: true }) return false;
                if(snapshot.HasChild(DatabaseKeyAssets.BEST_DISTANCE_KEY)) return true;
            }
            catch (Exception)
            {
                Debug.Log("Any user data not found!");
                return false;
            }

            return false;
        }
        
        private async UniTask CreateNewUserDataAsync(string userID)
        {
           await _databaseReference.Child(userID).Child(DatabaseKeyAssets.BEST_DISTANCE_KEY)
               .SetValueAsync(0.0);
        }
        
        public async UniTask<List<LeaderboardUser>> GetTop100UserByDistanceAsync()
        {
            var allUsers = await GetAllUsersSortedByDistanceAsync();
            var top100Users = allUsers.Take(100).ToList();
            return top100Users;
        }
        
        private async UniTask<List<LeaderboardUser>> GetAllUsersSortedByDistanceAsync()
        {
            var snapshot = await _databaseReference.OrderByChild(DatabaseKeyAssets.BEST_DISTANCE_KEY).GetValueAsync();
            if (snapshot.Exists == false) return null;
            
            var users = new List<LeaderboardUser>();
            
            foreach (var child in snapshot.Children)
            {
                var useId = child.Key;
                if (child.Value is Dictionary<string, object> user)
                {
                    var newElement = new LeaderboardUser(
                        useId,
                        user.TryGetValue(DatabaseKeyAssets.USER_NAME_KEY, out var userName) ? userName.ToString() : "Unknown",
                        user.TryGetValue(DatabaseKeyAssets.BEST_DISTANCE_KEY, out var bestDistance) ? double.Parse(bestDistance.ToString()) : 0.0
                    );
                    users.Add(newElement);
                }
            }
            return users.OrderByDescending(u => u.bestDistance).ToList();
        }
    }
}