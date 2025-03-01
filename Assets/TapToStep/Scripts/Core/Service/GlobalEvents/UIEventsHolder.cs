using System;

namespace Core.Service.GlobalEvents
{
    public sealed class UIEventsHolder
    {
        public event Action<bool> OnMainMenuOpening;
        public event Action OnClickedOnAnyElements;
        
        public void InvokeClickedOnAnyElements()
        {
            OnClickedOnAnyElements?.Invoke();
        }

        public void InvokeOnMainMenuIsOpen(bool isOpen)
        {
            OnMainMenuOpening?.Invoke(isOpen);
        }
    }
}