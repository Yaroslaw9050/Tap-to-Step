using System;
using Core.Extension.UI;
using Core.Service.GlobalEvents;
using Core.Service.Leaderboard;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UI.Views.Upgrades;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views.LeaderBoard
{
    public class LeaderBoardView : BaseView
    {
        [SerializeField] private LeaderBoardBuilder _boardBuilder;
        [SerializeField] private Button _backButton;

        [Header("User data")]
        [SerializeField] private TMP_InputField _userNameField;
        [SerializeField] private TextMeshProUGUI _bits;
        [SerializeField] private TextMeshProUGUI _userRankText;
        [SerializeField] private TextMeshProUGUI _userDistanceText;
        [SerializeField] private TextMeshProUGUI _userUniqIDText;

        [SerializeField] private Button _saveNameButton;
        [SerializeField] private RectTransform _userContainerRect;

        private GlobalEventsHolder _globalEventsHolder;
        private LeaderboardService _leaderboardService;
        private PlayerBuilder _playerBuilder;

        public event Action OnBackButtonPressed;
        
        [Inject]
        public void Constructor(GlobalEventsHolder globalEventsHolder, LeaderboardService leaderboardService, PlayerBuilder playerBuilder)
        {
            _globalEventsHolder = globalEventsHolder;
            _leaderboardService = leaderboardService;
            _playerBuilder = playerBuilder;
        }
        
        public void Init()
        {
            _backButton.onClick.AddListener(BackButtonClicked);
        }

        protected override void SubscribeToEvents()
        {
            
        }

        protected override void UnSubscribeFromEvents()
        {
            
        }

        public override void ShowView(float duration = 0.5f)
        {
            base.ShowView(duration);
            DisplayLeaderboardAsync().Forget();
            _saveNameButton.interactable = false;
            _bits.SetText(_playerBuilder.PlayerEntryPoint.PlayerStatistic.Bits.ToString());

            _userNameField.onSelect.AddListener(OnFieldSelected);
            _userNameField.onDeselect.AddListener(OnDeselected);
            _userNameField.onSubmit.AddListener(OnDeselected);
            _userNameField.onEndEdit.AddListener(OnDeselected);
            
            _userNameField.onValueChanged.AddListener(OnUserNameChanged);
            _saveNameButton.onClick.AddListener(ChangeNameButtonClicked);
        }

        private void OnDeselected(string data)
        {
            _userContainerRect.anchorMin = new Vector2(0f, 0f);
            _userContainerRect.anchorMax = new Vector2(1f, 0f);
            _userContainerRect.anchoredPosition = new Vector2(0, -20);
        }

        private void OnFieldSelected(string data)
        {
            _userContainerRect.anchorMin = new Vector2(0f, 1f);
            _userContainerRect.anchorMax = new Vector2(1f, 1f);
            _userContainerRect.anchoredPosition = new Vector2(0, 0);
        }

        public override void HideView(float duration = 0.5f)
        {
            base.HideView(duration);
            _boardBuilder.DestroyBoard();
        }

        private void OnUserNameChanged(string newUserName)
        {
            if (string.IsNullOrEmpty(newUserName))
            {
                _saveNameButton.interactable = false;
                return;
            }
            _userNameField.SetTextWithoutNotify(newUserName);
            _saveNameButton.interactable = _playerBuilder.PlayerEntryPoint.PlayerStatistic.Bits >= 30;
        }

        private void ChangeNameButtonClicked()
        {
            ChangeUserNameAsync().Forget();
        }

        private void BackButtonClicked()
        {
            _globalEventsHolder.UIEvents.InvokeClickedOnAnyElements();
            OnBackButtonPressed?.Invoke();
        }

        private async UniTask DisplayLeaderboardAsync()
        {
            _thisViewCanvasGroup.interactable = false;
            
            var (top100Users, myCard, myRank) =  await _leaderboardService.RequestAllLeaderboardAsync();
            
            await _boardBuilder.CreateBoardAsync(top100Users, myCard);
            _userNameField.SetTextWithoutNotify(myCard.userName);
            _userRankText.SetText(myRank.ToString());
            _userDistanceText.SetText(ValueConvertor.ToDistance((float)myCard.distance));
            _userUniqIDText.SetText(SystemInfo.deviceUniqueIdentifier);
            
            _thisViewCanvasGroup.interactable = true;
        }

        private async UniTaskVoid ChangeUserNameAsync()
        {
            _globalEventsHolder.InvokeNickNameChanged();
            // TODO: Fix on collectable changed!
            _bits.SetText(_playerBuilder.PlayerEntryPoint.PlayerStatistic.Bits.ToString());
            _bits.DOColor(Color.magenta, 0.5f).OnComplete(() => _bits.DOColor(Color.white, 1f));
            
            _thisViewCanvasGroup.interactable = false;
           await _leaderboardService.RenameUserAsync(_userNameField.text);
           _boardBuilder.DestroyBoard();
           await DisplayLeaderboardAsync();
           _thisViewCanvasGroup.interactable = true;

        }
    }
}