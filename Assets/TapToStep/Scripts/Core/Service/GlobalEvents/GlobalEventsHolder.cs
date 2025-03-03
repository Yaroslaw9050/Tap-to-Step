using System;
using CompositionRoot.Enums;

namespace Core.Service.GlobalEvents
{
    public sealed class GlobalEventsHolder
    {
        public PlayerEventsHolder PlayerEvents { get; } = new();
        public UIEventsHolder UIEvents { get; } = new();

        public event Action OnCollectablesChanged;
        
        public event Action<PerkType> OnSomeSkillUpgraded;
        
        public event Action OnNickNameChanged;

        public void InvokeOnCollectablesChanged()
        {
            OnCollectablesChanged?.Invoke();
        }

        public void InvokeSomePlayerSkillUpgraded(PerkType upgradeType)
        {
            OnSomeSkillUpgraded?.Invoke(upgradeType);
        }

        public void InvokeNickNameChanged()
        {
            OnNickNameChanged?.Invoke();
        }
    }
}