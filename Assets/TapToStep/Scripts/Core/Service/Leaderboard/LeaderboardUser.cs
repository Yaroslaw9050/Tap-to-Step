using System;

namespace Core.Service.Leaderboard
{
    [Serializable]
    public class LeaderboardUser
    {
        public string userName;
        public double currentDistance;
        public double bestDistance;
        public string userUniqueId;


        public LeaderboardUser(string userName, double currentDistance, double bestDistance, string userUniqueId)
        {
            this.userName = userName;
            this.currentDistance = currentDistance;
            this.userUniqueId = userUniqueId;
            this.bestDistance = bestDistance;
        }
    }
}