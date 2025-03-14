using Core.Extension.UI;
using Core.Service.LocalUser;
using TMPro;
using UI.Models;
using UI.ViewModels;
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
        [SerializeField] private TextMeshProUGUI _changeNamePriceText;
        [SerializeField] private TextMeshProUGUI _userRankText;
        [SerializeField] private TextMeshProUGUI _userBestUserDistanceText;
        [SerializeField] private TextMeshProUGUI _userUniqIDText;

        [Header("Buttons:")]
        [SerializeField] private Button _saveNameButton;
        
        private LocalPlayerService _localPlayerService;
        private LeaderBoardViewModel _viewModel;
        
        [Inject]
        public void Constructor(LocalPlayerService localPlayerService, LeaderBoardViewModel leaderBoardViewModel)
        {
            _localPlayerService = localPlayerService;
            _viewModel = leaderBoardViewModel;
        }
        
        protected override void SubscribeToEvents()
        {
            _userNameField.onValueChanged.AddListener(value => _viewModel.UserNameValueUpdated.Execute(value));
            _saveNameButton.onClick.AddListener(() => _viewModel.SaveUserNameCommand.Execute());
            
            _localPlayerService.PlayerModel.UserName.Subscribe(ReactNameUpdated).AddTo(_disposable);
            _localPlayerService.PlayerModel.UserId.Subscribe(ReactUserIdUpdated).AddTo(_disposable);
            _localPlayerService.PlayerModel.BestDistance.Subscribe(ReactBestDistanceUpdated).AddTo(_disposable);
        }

        protected override void UnSubscribeFromEvents()
        {
            _userNameField.onValueChanged.RemoveAllListeners();
            _saveNameButton.onClick.RemoveAllListeners();
        }
        
        public override void ShowView(float duration = 0.5f)
        {
            base.ShowView(duration);
            _changeNamePriceText.SetText($"save ({PriceModel.CHANGE_NAME_PRICE} bits)");
        }

        private void ReactNameUpdated(string newName)
        {
            _userNameField.onValueChanged.Invoke(newName);
            _userNameField.SetTextWithoutNotify(newName);
        }

        private void ReactUserIdUpdated(string newUserId)
        {
            _userUniqIDText.SetText(newUserId);
        }

        private void ReactBestDistanceUpdated(double newBestDistance)
        {
            _userBestUserDistanceText.SetText(ValueConvertor.ToDistance(newBestDistance));
        }
    }
}