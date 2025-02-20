using Patterns.Models;

namespace TapToStep.Scripts.Core.Service.LocalUser
{
    public class LocalPlayerService
    {
        public PlayerModel PlayerModel { get; }

        public void SetBits(ulong bits)
        {
            PlayerModel.Bits = bits;
        }

        public void SetCurrentDistance(double currentDistance)
        {
            PlayerModel.CurrentDistance = currentDistance;
        }
        
        public void SetBestDistance(double bestDistance)
        {
            PlayerModel.BestDistance = bestDistance;
        }

        public void AddBits(ushort newValue)
        {
            PlayerModel.Bits += newValue;
        }

        public void RemoveBits(ushort removedValue)
        {
            PlayerModel.Bits -= removedValue;
        }
    }
}