using System;

namespace CompositionRoot.SO.Player.Logic
{
    [Serializable]
    public class PlayerPerkData
    {
        public int CurrentLevel;
        
        public PlayerPerkData(int currentLevel)
        {
            CurrentLevel = currentLevel;
        }
    }
}