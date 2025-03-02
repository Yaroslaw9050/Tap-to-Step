using System;
using Core.Service.LocalUser;
using TMPro;
using UI.Views.Upgrades;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views.LeaderBoard
{
    public class UserInfoSubView: BaseView
    {
        [Header("Inputs:")]
        [SerializeField] private TMP_InputField _userNameField;
        
        [Header("Texts:")]
        [SerializeField] private TextMeshProUGUI _userRankText;
        [SerializeField] private TextMeshProUGUI _userDistanceText;
        [SerializeField] private TextMeshProUGUI _userUniqIDText;

        [Header("Buttons:")]
        [SerializeField] private Button _saveNameButton;
        
        private LocalPlayerService _localPlayerService;
        
        [Inject]
        public void Constructor(LocalPlayerService localPlayerService)
        {
            _localPlayerService = localPlayerService;           
        }
        
        protected override void SubscribeToEvents()
        {
            _localPlayerService.PlayerModel.UserName.Subscribe(ReactNameUpdated).AddTo(_disposable);
            _localPlayerService.PlayerModel.UserId.Subscribe(ReactUserIdUpdated).AddTo(_disposable);
        }

        protected override void UnSubscribeFromEvents()
        {
            
        }

        private void ReactNameUpdated(string newName)
        {
            _userNameField.SetTextWithoutNotify(newName);
        }

        private void ReactUserIdUpdated(string newUserId)
        {
            _userUniqIDText.SetText(newUserId);
        }
    }
}