using UnityEngine;
using CompositionRoot.Enums;
using CompositionRoot.Static;

namespace CompositionRoot.SO.Player.Logic
{
    [CreateAssetMenu(fileName = "PlayerUpgradeSO", menuName = "Player/Upgrades", order = 1)]
    public class PlayerPerkSO: RuntimeScriptableObject
    {
        [SerializeField] private PerkType _upgradeType;
        [Range(1, 1000)]
        [SerializeField] private int _costPerOneLevel;
        [Range(1, 1000)]
        [SerializeField] private int _startPrice;
        [Range(1, 100)]
        [SerializeField] private int _maxLevel;
        
        [SerializeField] private int _currentLevel;
        
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
            _costPerOneLevel = data.costPerOneLevel;
            _startPrice = data.startPrice;
            _maxLevel = data.maxLevel;
            _currentLevel = data.currentLevel;
        }

        public PlayerPerkData ToPlayerPerkData()
        {
            return new PlayerPerkData(this);
        }

        protected override void OnReset()
        {
            _currentLevel = 0;
        }
    }
}