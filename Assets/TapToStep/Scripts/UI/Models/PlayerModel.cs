using System;
using UniRx;

namespace UI.Models
{
    [Serializable]
    public class PlayerModel
    {
        public ReactiveProperty<string> UserId { get; } = new(string.Empty);
        public ReactiveProperty<string> UserName { get; } = new(string.Empty);
        public ReactiveProperty<ulong> Bits { get; } = new(0);
        public ReactiveProperty<double> BestDistance { get; } = new(0);
        public ReactiveProperty<double> CurrentDistance { get; } = new(0);
    }
}