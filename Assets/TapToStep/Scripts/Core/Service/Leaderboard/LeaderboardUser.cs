using System;

namespace Core.Service.Leaderboard
{
    [Serializable]
    public class LeaderboardUser
    {
        public string userName;
        public double distance;
        public string userUniqueId;


        public LeaderboardUser(string userName, double distance, string userUniqueId)
        {
            this.userName = userName;
            this.distance = distance;
            this.userUniqueId = userUniqueId;
        }
    }
}