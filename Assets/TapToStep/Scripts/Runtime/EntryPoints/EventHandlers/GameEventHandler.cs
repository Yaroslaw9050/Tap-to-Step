using System;
using CompositionRoot.Enums;

namespace Runtime.EntryPoints.EventHandlers
{
    public class GameEventHandler
    {
        public event Action<int> OnCollectablesChanged;
        public event Action OnPlayerStartMoving;
        public event Action OnPlayerDied;
        public event Action OnUiElementClicked;
        public event Action<float> OnPlayerTouchedToEndOfLocation;
        public event Action<bool> OnPlayerScreenCastStatusChanged;
        public event Action<bool> OnMenuViewStatusChanged;
        public event Action<PerkType> OnSomeSkillUpgraded;
        public event Action OnPlayerResumed;

        public void InvokeOnCollectablesChanged(int value)
        {
            OnCollectablesChanged?.Invoke(value);
        }
        
        public void InvokeOnPlayerStartMoving()
        {
            OnPlayerStartMoving?.Invoke();
        }

        public void InvokeOnPlayerDied()
        {
            OnPlayerDied?.Invoke();
            InvokeOnPlayerScreenCastStatus(false);
        }

        public void InvokeOnUiElementClicked()
        {
            OnUiElementClicked?.Invoke();
        }

        public void InvokeOnPlayerScreenCastStatus(bool isActive)
        {
            OnPlayerScreenCastStatusChanged?.Invoke(isActive);
        }

        public void InvokeOnMenuViewStatus(bool isActive)
        {
            OnMenuViewStatusChanged?.Invoke(isActive);
            InvokeOnPlayerScreenCastStatus(!isActive);
        }

        public void InvokeSomePlayerSkillUpgraded(PerkType upgradeType)
        {
            OnSomeSkillUpgraded?.Invoke(upgradeType);
        }

        public void InvokeOnPlayerTouchedToEndOfLocation(float playerZPosition)
        {
            OnPlayerTouchedToEndOfLocation?.Invoke(playerZPosition);
        }

        public void InvokeOnGameResumed()
        {
            OnPlayerResumed?.Invoke();
        }
    }
}