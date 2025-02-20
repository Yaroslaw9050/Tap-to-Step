using System;
using UnityEngine;

namespace Patterns.Models
{
    [Serializable]
    public class PlayerModel
    {
        [field: SerializeField] public ulong Bits { get; set; }
        [field: SerializeField] public double BestDistance { get; set; }
        [field: SerializeField] public double CurrentDistance { get; set; }
    }
}