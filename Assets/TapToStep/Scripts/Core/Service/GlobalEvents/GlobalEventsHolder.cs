using System;
using CompositionRoot.Enums;

namespace Runtime.EntryPoints.EventHandlers
{
    public class PlayerEventsHolder
    {
        public event Action<float> OnTouchedToEndOfLocation;
        public event Action<bool> OnScreenInputStatusChanged;
        public event Action OnStartMoving;
        public event Action OnReborn;
        public event Action OnDied;
        
        public void InvokeScreenInputStatusChanged(bool isActive)
        {
            OnScreenInputStatusChanged?.Invoke(isActive);
        }
        
        public void InvokeOnTouchedToEndOfLocation(float playerZPosition)
        {
            OnTouchedToEndOfLocation?.Invoke(playerZPosition);
        }
        
        public void InvokeOnStartMoving()
        {
            OnStartMoving?.Invoke();
        }

        public void InvokeOnDied()
        {
            OnDied?.Invoke();
        }
        
        public void InvokeOnReborn()
        {
            OnReborn?.Invoke();
        }
    }

    public class UIEventsHolder
    {
        public event Action OnClickedOnAnyElements;
        
        public void InvokeClickedOnAnyElements()
        {
            OnClickedOnAnyElements?.Invoke();
        }
    }
    
    public class GlobalEventsHolder
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