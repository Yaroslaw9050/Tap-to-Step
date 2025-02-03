using System.Linq;
using CompositionRoot.Enums;
using CompositionRoot.SO.Player.Logic;
using UnityEngine;

namespace Runtime.Player.Perks
{
    public class PlayerPerkSystem : MonoBehaviour
    {
        [SerializeField] private PlayerPerkSO[] _supportedUpgrades;

        public int GetPerkLevel(PerkType perkType)
        {
            var perk = GetPerkByType(perkType);
            return perk.CurrentLevel;
        }
        public int GetPerkPrice(PerkType type)
        {
            var perk = GetPerkByType(type);
            return perk.UpgradeCost;
        }

        public bool TryUpgradePerk(PerkType type)
        {
            var perk = GetPerkByType(type);
            if (perk.UpgradePerk())
            {
                SaveToMemory(perk.UpgradeType);
                return true;
            }

            return false;
        }

        public void SaveAllPerks()
        {
            foreach (var perkSo in _supportedUpgrades)
            {
                SaveToMemory(perkSo.UpgradeType);
            }
        }

        public void LoadAllPerks()
        {
            foreach (var perk in _supportedUpgrades)
            {
                perk.Reset();
                LoadFromMemory(perk.UpgradeType);
            }
        }

        public float GetPerkValueByType(PerkType type)
        {
            var perk = GetPerkByType(type);
            return perk.CurrentLevel / 10f;
        }

        private void SaveToMemory(PerkType type)
        {
            var perk = GetPerkByType(type);
            var data = new PlayerPerkData(perk.MaxLevel, perk.CurrentLevel, perk.StartPrice, perk.CostPerOneLevel);
            var serializedData = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(type.ToString(), serializedData);
        }

        private void LoadFromMemory(PerkType type)
        {
            if (PlayerPrefs.HasKey(type.ToString()) == false)
            {
                return;
            }
            
            var json = PlayerPrefs.GetString(type.ToString());
            var data = JsonUtility.FromJson<PlayerPerkData>(json);
            var perk = GetPerkByType(type);
            perk.WriteData(data);
        }

        private PlayerPerkSO GetPerkByType(PerkType type)
        {
            return _supportedUpgrades.First(upgrade => upgrade.UpgradeType == type);
        }
    }
}