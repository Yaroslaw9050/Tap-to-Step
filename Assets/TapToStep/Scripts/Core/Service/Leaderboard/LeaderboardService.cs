using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

namespace Core.Service.Leaderboard
{
    public class LeaderboardService: MonoBehaviour
    {
        private DatabaseReference _databaseReference; 
        private LeaderboardUser _leaderboardUser;
        
        private string _userId;
        private long _totalUsers;
        private bool _systemReady;

        public bool SystemReady => _systemReady;

        private const string USER_ID_KEY = "User_";
        private const string USER_NAME_KEY = "userName";
        private const string USER_DISTANCE_KEY = "distance";
        private const string USER_UNIQUE_ID_KEY = "uniqueId";
        
        private const string DATABASE_KEY = "Leaderboard";
        
        
        public async UniTask InitAsync()
        {
            if(_systemReady) return;
           
            _databaseReference = FirebaseDatabase.DefaultInstance.GetReference(DATABASE_KEY);
            _totalUsers = await GetTotalUserCountAsync();
            if (_totalUsers == -1)
            {
                Debug.LogError("Leaderboard service error | Problem getting leaderboard users.");
                return;
            }
            _systemReady = await TryReadOrCreateUserAsync();
            if(_systemReady == false) return;
            Debug.Log("Leaderboard Initialized!");
        }

        public async UniTask RenameUserAsync(string newUserName)
        {
            if(_systemReady == false) return;
            await _databaseReference.Child(_userId).Child(USER_NAME_KEY).SetValueAsync(newUserName);
        }

        public async UniTask UpdateUserDistanceAsync(double currentUserDistance)
        {
            if(_systemReady == false) return;
            var tempUser = await RequestPersonalDataAsync(_userId);

            if (currentUserDistance > tempUser.distance)
            {
                await _databaseReference.Child(_userId).Child(USER_DISTANCE_KEY).SetValueAsync(currentUserDistance);
            }
        }

        public async UniTask<(List<LeaderboardUser>, LeaderboardUser,  int)> RequestAllLeaderboardAsync()
        {
            if(_systemReady == false) return default;
            
            var allUsers = await GetAllUsersSortedByDistanceAsync();
            var top100Users = allUsers.Take(100).ToList();
            var myUser = await RequestPersonalDataAsync(_userId);
            var myRank = GetUserRank(allUsers);
            
            return (top100Users, myUser, myRank);
        }

        public async UniTask<double> RequestUserDistanceFromLeaderboardAsync()
        {
            if(_systemReady == false) return -1;
            var user = await RequestPersonalDataAsync(_userId);
            return user.distance;
        }
        

        private int GetUserRank(List<LeaderboardUser> users)
        {
            var rank = users.FindIndex(u => u.userUniqueId == SystemInfo.deviceUniqueIdentifier) + 1;
            return rank;
        }

        private async UniTask<bool> TryReadOrCreateUserAsync()
        {
            if(_leaderboardUser != null) return false;
            
            if (PlayerPrefs.HasKey(USER_ID_KEY))
            {
                _userId = PlayerPrefs.GetString(USER_ID_KEY);
                _leaderboardUser = await RequestPersonalDataAsync(_userId);
               
                if (_leaderboardUser == null)
                {
                    Debug.LogError($"Personal user data has some issue: {_userId}" );
                    return false;
                }

                return true;
            }
            else
            {
                _userId = await CreateNewUserAsync();
                PlayerPrefs.SetString(USER_ID_KEY, _userId);
                PlayerPrefs.Save();
                return true;
            }
        }

        private async UniTask<string> CreateNewUserAsync()
        {
            var uniqId = SystemInfo.deviceUniqueIdentifier;
            _leaderboardUser = new LeaderboardUser($"Player_{_totalUsers}", 0.0, uniqId);
            var newUserID = $"{USER_ID_KEY}{_totalUsers}";
            
            await _databaseReference.Child(newUserID).Child(USER_NAME_KEY).SetValueAsync(_leaderboardUser.userName);
            await _databaseReference.Child(newUserID).Child(USER_DISTANCE_KEY).SetValueAsync(_leaderboardUser.distance);
            await _databaseReference.Child(newUserID).Child(USER_UNIQUE_ID_KEY).SetValueAsync(_leaderboardUser.userUniqueId);
            
            return newUserID;
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
                    var userName = snapshot.Child(USER_NAME_KEY).Value.ToString();
                    var distance = double.Parse(snapshot.Child(USER_DISTANCE_KEY).Value.ToString());
                    var userUniqId = snapshot.Child(USER_UNIQUE_ID_KEY).Value.ToString();
                    return new LeaderboardUser(userName, distance, userUniqId);
                }

                Debug.LogError("User Not Found!");
            }
            return null;
        }

        private async UniTask<long> GetTotalUserCountAsync()
        {
            var snapshot = await _databaseReference.OrderByChild(USER_DISTANCE_KEY).GetValueAsync();
            if (snapshot.Exists == false) return -1;

            return snapshot.Children.Count();
        }
        private async UniTask<List<LeaderboardUser>> GetAllUsersSortedByDistanceAsync()
        {
            var snapshot = await _databaseReference.OrderByChild(USER_DISTANCE_KEY).GetValueAsync();
            if (snapshot.Exists == false) return null;
            
            var users = new List<LeaderboardUser>();
            
            foreach (var child in snapshot.Children)
            {
                if (child.Value is Dictionary<string, object> user)
                {
                    var newElement = new LeaderboardUser(
                        user.ContainsKey(USER_NAME_KEY) ? user[USER_NAME_KEY].ToString() : "Unknown",
                        user.ContainsKey(USER_DISTANCE_KEY)
                            ? double.Parse(user[USER_DISTANCE_KEY].ToString())
                            : 0,
                        user.ContainsKey(USER_UNIQUE_ID_KEY) ? user[USER_UNIQUE_ID_KEY].ToString() : string.Empty
                    );
                    users.Add(newElement);
                }
            }
            return users.OrderByDescending(u => u.distance).ToList();
        }
    }
}