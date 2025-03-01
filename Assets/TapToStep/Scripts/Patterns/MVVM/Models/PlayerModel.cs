using System;
using UniRx;
using UnityEngine;

namespace Patterns.Models
{
    [Serializable]
    public class PlayerModel
    {
        [field: SerializeField] public ReactiveProperty<ulong> Bits { get; } = new(0);
        [field: SerializeField] public ReactiveProperty<double> BestDistance { get; } = new(0);
        [field: SerializeField] public ReactiveProperty<double> CurrentDistance { get; } = new(0);
    }
}