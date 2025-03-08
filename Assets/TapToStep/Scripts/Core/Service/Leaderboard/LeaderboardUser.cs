using System;

namespace Core.Service.Leaderboard
{
    [Serializable]
    public class LeaderboardUser
    {
        public string userId;
        public string userName;
        public double bestDistance;
        
        public LeaderboardUser(string userId, string userName, double bestDistance)
        {
            this.userId = userId;
            this.userName = userName;
            this.bestDistance = bestDistance;
        }
    }
}