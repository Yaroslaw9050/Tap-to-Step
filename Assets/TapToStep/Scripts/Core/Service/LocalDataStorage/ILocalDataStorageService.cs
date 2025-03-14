namespace Core.Service.LocalDataStorage
{
    public interface ILocalDataStorageService
    {
        public bool HasKey(string key);
        
        public void SaveString(string key, string value);

        public bool TryLoadString(string key, out string value);

        public void SaveInt(string key, int value);
        public bool TryLoadInt(string key, out int value);
    }
}