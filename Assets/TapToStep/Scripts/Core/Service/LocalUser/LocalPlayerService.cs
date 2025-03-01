using System;
using CompositionRoot.Enums;
using CompositionRoot.SO.Player.Logic;
using Patterns.Models;
using Runtime.Player.Upgrade;
using UnityEngine;
using Zenject;

namespace Core.Service.LocalUser
{
    public sealed class LocalPlayerService : MonoBehaviour
    {
        [SerializeField] private PlayerSettingSO _startupPlayerSetting;
        
        private PlayerPerkSystem _playerPerkSystem;
        public PlayerModel PlayerModel { get; } = new();
        
        
        [Inject]
        public void Constructor(PlayerPerkSystem perkSystem)
        {
            _playerPerkSystem = perkSystem;
        }

        public void SetBits(ulong bits)
        {
            PlayerModel.Bits.Value = bits;
        }

        public void SetCurrentDistance(double currentDistance)
        {
            PlayerModel.CurrentDistance.Value = currentDistance;
        }
        
        public void SetBestDistance(double bestDistance)
        {
            PlayerModel.BestDistance.Value = bestDistance;
        }

        public void AddBits(ushort newValue)
        {
            PlayerModel.Bits.Value += newValue;
        }

        public void RemoveBits(uint removedValue)
        {
            PlayerModel.Bits.Value -= removedValue;
        }

        public void AddDistance(float distance)
        {
            var newDistance = Math.Round(distance, 1);
            PlayerModel.CurrentDistance.Value += newDistance;
        }

        public float GetStepLenght()
        {
            return _startupPlayerSetting.StepLenght + _playerPerkSystem.GetPerkValueByType(PerkType.StepLenght);
        }

        public float GetStepTime()
        {
            return _startupPlayerSetting.StepSpeed - _playerPerkSystem.GetPerkValueByType(PerkType.StepSpeed);
        }

        public float GetTurnSpeed()
        {
            return _playerPerkSystem.GetPerkValueByType(PerkType.TurnSpeed);
        }
    }
}