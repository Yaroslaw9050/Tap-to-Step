using System;
using CompositionRoot.Enums;

namespace CompositionRoot.SO.Player.Logic
{
    [Serializable]
    public class PlayerPerkData
    {
        public PerkType upgradeType;
        public int costPerOneLevel;
        public int startPrice;
        public int maxLevel;
        public int currentLevel;
        
        public PlayerPerkData(PlayerPerkSO data)
        {
            upgradeType = data.UpgradeType;
            costPerOneLevel = data.CostPerOneLevel;
            startPrice = data.StartPrice;
            maxLevel = data.MaxLevel;
            currentLevel = data.CurrentLevel;
        }
        
        public int GetUpgradeTypeInt() => (int)upgradeType;
        public void SetUpgradeTypeInt(int value) => upgradeType = (PerkType)value;
    }
}