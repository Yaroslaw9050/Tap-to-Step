using System;

namespace Runtime.EntryPoints.EventHandlers
{
    public class GlobalEventHandler
    {
        public event Action<int> OnCollectablesChanged;
        public event Action OnPlayerStartMoving;
        public event Action OnPlayerDied;

        public event Action OnUiElementClicked;

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
        }

        public void InvokeOnUiElementClicked()
        {
            OnUiElementClicked?.Invoke();
        }
    }
}