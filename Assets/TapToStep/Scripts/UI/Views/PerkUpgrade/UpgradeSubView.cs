using System;
using CompositionRoot.Enums;
using Core.Service.GlobalEvents;
using Core.Service.Leaderboard;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Upgrades
{
    public class UpgradeSubView : MonoBehaviour
    {
        [SerializeField] private PerkType _perkType;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Button _upgradeButton;
        
        private GlobalEventsHolder _globalEventsHolder;

        public PerkType PerkType => _perkType;
        public event Action<PerkType> OnUpgradeButtonPressed;

        public void Init(GlobalEventsHolder globalEventsHolder)
        {
            _globalEventsHolder = globalEventsHolder;
            _upgradeButton.onClick.AddListener(() =>  OnUpgradeButtonPressed?.Invoke(_perkType));
        }

        public void UpdateElementsData(int level, int cost)
        {
            if (level == -1)
            {
                _levelText.SetText("MAX");
                _costText.SetText("Max level");
                _upgradeButton.interactable = false;
                _progressSlider.value = 1f;
                return;
            }
            
            _levelText.text = level.ToString();
            _costText.text = $"upgrade \\n <size=80%>({cost} bits) </size>";
            //_upgradeButton.interactable = _leaderboardService.SystemReady;
        }

        public void PlayPurchaseAnimation()
        {
            _upgradeButton.interactable = false;

            _progressSlider.DOValue(1f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                _upgradeButton.interactable = true;
                _progressSlider.value = 0f;
                
                _globalEventsHolder.InvokeSomePlayerSkillUpgraded(_perkType);
            });
        }
    }
}