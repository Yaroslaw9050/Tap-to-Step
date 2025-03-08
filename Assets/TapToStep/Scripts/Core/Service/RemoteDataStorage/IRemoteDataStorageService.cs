using CompositionRoot.Enums;
using CompositionRoot.SO.Player.Logic;
using Cysharp.Threading.Tasks;

namespace Core.Service.RemoteDataStorage
{
    public interface IRemoteDataStorageService
    {
        public void Initialise();
        public void CreateStartedFieldsForNewUser(string userId);
        public UniTask SaveBaseUserDataAsync<T>(string userId, string key, T value);
        public UniTask<string> LoadBaseUserDataAsync(string userId, string key);
        public UniTask<T> LoadBaseClassUserDataAsync<T>(string userId, string key);
        public UniTask SavePerkAsync(string userId, PlayerPerkData perkData);
        public UniTask<PlayerPerkData> LoadPerkAsync(string userId, PerkType perkType);

    }
}