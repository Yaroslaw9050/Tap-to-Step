using Cysharp.Threading.Tasks;

namespace Core.Service.Leaderboard
{
    public interface ILeaderboardService
    {
        public void Initialise();
        public UniTask CheckAllUserFieldsAsync(string userId);
        public UniTask SaveUserDataAsync<T>(string userId, string key, T value);
       
        public UniTask<string> LoadUserDataAsync(string userId, string key);
    }
}