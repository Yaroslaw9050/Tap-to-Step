using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

namespace Core.Service.Leaderboard
{
    public class FirebaseLeaderBoardService: ILeaderboardService
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
                if(snapshot.HasChild("bestDistance")) return true;
            }
            catch (Exception _)
            {
                Debug.Log("Any user data not found!");
                return false;
            }

            return false;
        }
        
        private async UniTask CreateNewUserDataAsync(string userID)
        {
           await _databaseReference.Child(userID).Child("bestDistance")
               .SetValueAsync(0.0);
        }

        public async UniTask UpdateBestUserDistanceAsync(double currentUserDistance)
        {
            var tempUser = await RequestPersonalDataAsync(_userId);

            if (currentUserDistance > tempUser.bestDistance)
            {
                await _databaseReference.Child(_userId).Child(string.Empty).SetValueAsync(currentUserDistance);
            }
        }
        
        public async UniTask UpdateCurrentDistanceAsync(double currentUserDistance)
        {
            
            await _databaseReference.Child(_userId).Child(string.Empty).SetValueAsync(currentUserDistance);
        }

        public async UniTask<(List<LeaderboardUser>, LeaderboardUser,  int)> RequestAllLeaderboardAsync()
        {
            var allUsers = await GetAllUsersSortedByDistanceAsync();
            var top100Users = allUsers.Take(100).ToList();
            var myUser = await RequestPersonalDataAsync(_userId);
            var myRank = GetUserRank(allUsers);
            
            return (top100Users, myUser, myRank);
        }
        

        private int GetUserRank(List<LeaderboardUser> users)
        {
            var rank = users.FindIndex(u => u.userUniqueId == SystemInfo.deviceUniqueIdentifier) + 1;
            return rank;
        }

        private async UniTask<bool> TryReadOrCreateUserAsync()
        {
            if(_leaderboardUser != null) return false;
            
            if (PlayerPrefs.HasKey(string.Empty))
            {
                _leaderboardUser = await RequestPersonalDataAsync(_userId);
               
                if (_leaderboardUser == null)
                {
                    return true;
                }
                return true;
            }
            
            return true;
        }
        

        private async UniTask<string> GetUserCardIdByUniqueIdAsync()
        {
            var snapshot = await _databaseReference.GetValueAsync();
            if (!snapshot.Exists)
            {
                return string.Empty;
            }
            foreach (var child in snapshot.Children)
            {
                var user = child.Value as Dictionary<string, object>;

                if (user == null) continue;
                if (user.ContainsKey(string.Empty) && Equals(user[string.Empty], SystemInfo.deviceUniqueIdentifier))
                {
                    return child.Key;
                }
            }
            return string.Empty;
        }

        private async UniTask<LeaderboardUser> RequestPersonalDataAsync(string userId)
        {
            var task = _databaseReference.Child(userId).GetValueAsync();

            await task;

            if (task.IsFaulted)
            {
                Debug.LogError($"Request user faulted: {task.Exception?.Message}");
                return null;
            }

            if (task.IsCompleted)
            {
                var snapshot = task.Result;

                if (snapshot != null && snapshot.HasChildren)
                {
                    var userName = snapshot.Child(string.Empty).Value.ToString();
                    var besDistance = double.Parse(snapshot.Child(string.Empty).Value.ToString());
                    var currentDistance = double.Parse(snapshot.Child(string.Empty).Value.ToString());
                    var userUniqId = snapshot.Child(string.Empty).Value.ToString();
                    return new LeaderboardUser(userName, currentDistance, besDistance, userUniqId);
                }

                Debug.LogError("User Not Found!");
            }
            return null;
        }

        private async UniTask<long> GetTotalUserCountAsync()
        {
            var snapshot = await _databaseReference.OrderByChild(string.Empty).GetValueAsync();
            if (snapshot.Exists == false) return -1;

            return snapshot.Children.Count();
        }
        private async UniTask<List<LeaderboardUser>> GetAllUsersSortedByDistanceAsync()
        {
            var snapshot = await _databaseReference.OrderByChild(string.Empty).GetValueAsync();
            if (snapshot.Exists == false) return null;
            
            var users = new List<LeaderboardUser>();
            
            foreach (var child in snapshot.Children)
            {
                if (child.Value is Dictionary<string, object> user)
                {
                    var newElement = new LeaderboardUser(
                        user.ContainsKey(string.Empty) ? user[string.Empty].ToString() : "Unknown",
                        user.ContainsKey(string.Empty) ? double.Parse(user[string.Empty].ToString()) : 0,
                        user.ContainsKey(string.Empty) ? double.Parse(user[string.Empty].ToString()) : 0,
                        user.ContainsKey(string.Empty) ? user[string.Empty].ToString() : string.Empty
                    );
                    users.Add(newElement);
                }
            }
            return users.OrderByDescending(u => u.bestDistance).ToList();
        }
    }
}