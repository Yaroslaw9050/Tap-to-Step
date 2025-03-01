using System;

namespace Core.Service.GlobalEvents
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
}