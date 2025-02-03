using System;

namespace CompositionRoot.SO.Player.Logic
{
    [Serializable]
    public class PlayerPerkData
    {
        public int CostPerOneLevel;
        public int StartPrice;
        public int MaxLevel;
        public int CurrentLevel;
        
        public PlayerPerkData(int maxLevel, int currentLevel, int startPrice, int costPerOneLevel)
        {
            MaxLevel = maxLevel;
            CurrentLevel = currentLevel;
            StartPrice = startPrice;
            CostPerOneLevel = costPerOneLevel;
        }
    }
}