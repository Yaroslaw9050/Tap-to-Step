using System;
using System.Collections.Generic;
using System.Threading;
using Core.Service.Leaderboard;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Zenject;


namespace UI.Views.LeaderBoard
{
    public class LeaderBoardBuilder : MonoBehaviour
    {
        [SerializeField] private BoardElement _boardElement;
        [SerializeField] private RectTransform _boardElementsParent;
        
        private readonly List<BoardElement> r_boardElements = new(100);
        private LeaderboardService _leaderboardService;
        private CancellationTokenSource _cts;
        
        [Inject]
        public void Constructor(LeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
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

        public async UniTask CreateBoardAsync(List<LeaderboardUser> top100Users, LeaderboardUser myCard)
        {
            if(_leaderboardService.SystemReady == false) return;
            
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            
            for (var i = 0; i < top100Users.Count; i++)
            {
                if(_cts.Token.IsCancellationRequested) break;
               
                var user = top100Users[i];
                var element = Instantiate(_boardElement, _boardElementsParent);
                element.Init(i+1, user.userName, user.distance, string.Equals(user.userUniqueId, myCard.userUniqueId));
                r_boardElements.Add(element);
                await UniTask.DelayFrame(1, cancellationToken: _cts.Token);
            }
        }
    }
}