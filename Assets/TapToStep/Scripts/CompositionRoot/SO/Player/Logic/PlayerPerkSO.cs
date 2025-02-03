using System;
using UnityEngine;
using CompositionRoot.Enums;

namespace CompositionRoot.SO.Player.Logic
{
    [CreateAssetMenu(fileName = "PlayerUpgradeSO", menuName = "Player/Upgrades", order = 1)]
    public class PlayerPerkSO: ScriptableObject
    {
        [SerializeField] private PerkType _upgradeType;
        [Range(1, 1000)]
        [SerializeField] private int _costPerOneLevel;
        [Range(1, 1000)]
        [SerializeField] private int _startPrice;
        [Range(1, 100)]
        [SerializeField] private int _maxLevel;
        
        private int _currentLevel;

        public int StartPrice => _startPrice;
        public PerkType UpgradeType => _upgradeType;
        public int CostPerOneLevel => _costPerOneLevel;
        public int MaxLevel => _maxLevel;
        public int CurrentLevel => _currentLevel;
        public int UpgradeCost
        {
            get
            {
                var level = _currentLevel + 1;
                return level * _costPerOneLevel + _startPrice;
            }
        }

        public bool UpgradePerk()
        {
            if (_currentLevel < _maxLevel)
            {
                _currentLevel++;
                return true;
            }
            return false;
        }

        public void WriteData(PlayerPerkData data)
        {
            _maxLevel = data.MaxLevel;
            _currentLevel = data.CurrentLevel;
            _startPrice = data.StartPrice;
            _costPerOneLevel = data.CostPerOneLevel;
        }

        public void Reset()
        {
            _currentLevel = 0;
        }
    }
}