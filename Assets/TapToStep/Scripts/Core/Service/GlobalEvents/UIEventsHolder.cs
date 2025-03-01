using System;

namespace Core.Service.GlobalEvents
{
    public sealed class UIEventsHolder
    {
        public event Action OnClickedOnAnyElements;
        
        public void InvokeClickedOnAnyElements()
        {
            OnClickedOnAnyElements?.Invoke();
        }
    }
}