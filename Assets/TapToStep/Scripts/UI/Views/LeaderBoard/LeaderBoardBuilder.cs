using System;
using System.Collections.Generic;
using System.Threading;
using Core.Service.Leaderboard;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;


namespace UI.Views.LeaderBoard
{
    public class LeaderBoardBuilder : MonoBehaviour
    {
        [SerializeField] private BoardElement _boardElement;
        [SerializeField] private RectTransform _boardElementsParent;

        [Header("User data")]
        [SerializeField] private TextMeshProUGUI _userRankText;
        [SerializeField] private TextMeshProUGUI _userNameText;
        [SerializeField] private TextMeshProUGUI _userDistanceText;
        
        private readonly List<BoardElement> r_boardElements = new(100);
        private LeaderboardService _leaderboardService;
        private CancellationTokenSource _cts;
        
        [Inject]
        public void Constructor(LeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }
        
        public void CreateBoard(Action onCompleted)
        {
            if(_leaderboardService.SystemReady == false) return;
            
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            
            CreateBoardAsync(onCompleted).Forget();
        }

        public void DestroyBoard()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            for (var i = 0; i < r_boardElements.Count; i++)
            {
                Destroy(r_boardElements[i].gameObject);
            }
            r_boardElements.Clear();
        }

        private async UniTask CreateBoardAsync(Action onCompleted)
        {
            var (top100Users, myCard, myRank) = await _leaderboardService.RequestAllLeaderboardAsync();
            
            _userNameText.SetText(myCard.userName);
            _userRankText.SetText((myRank).ToString());
            _userDistanceText.SetText(ConvertToDistance(myCard.distance));
            
            for (var i = 0; i < top100Users.Count; i++)
            {
                if(_cts.Token.IsCancellationRequested) break;
               
                var user = top100Users[i];
                var element = Instantiate(_boardElement, _boardElementsParent);
                element.Init(i+1, user.userName, user.distance);
                r_boardElements.Add(element);
                await UniTask.DelayFrame(1, cancellationToken: _cts.Token);
            }
            
            onCompleted?.Invoke();
        }
        
        private  string ConvertToDistance(double distance)
        {
            var meters = (int)distance;
            var centimeters = (int)((distance - meters) * 100); 
            
            var result = "";
            
            if (meters >= 1000)
            {
                int kilometers = meters / 1000; 
                meters = meters % 1000; 
                result += $"{kilometers}km";
                
                if (meters > 0)
                {
                    result += $" {meters}m";
                }
            }
            else
            {
                if (meters > 0)
                {
                    result += $"{meters}m";
                }
                
                if (centimeters > 0)
                {
                    result += $" {centimeters}cm";
                }
            }

            return result.Trim();
        }
    }
}