using System;
using Runtime.EntryPoints.EventHandlers;
using UnityEngine.EventSystems;

namespace Runtime.Player
{
    public class PlayerEventHandler
    {
        public event Action<MoveDirection> OnMoveButtonTouched;
        public event Action OnPlayerStartMoving;
        public event Action OnPlayerDied;
        
        private readonly GlobalEventHandler r_globalEventHandler;
        private readonly PlayerEntryPoint r_entryPoint;
        
        public PlayerEventHandler(PlayerEntryPoint entryPoint, GlobalEventHandler globalEventHandler)
        {
            r_entryPoint = entryPoint;
            r_globalEventHandler = globalEventHandler;   
        }
        
        public void InvokeTouchedToCollectables(int value)
        {
            r_globalEventHandler.InvokeOnCollectablesChanged(value);
        }

        public void InvokeMoveButtonTouched(MoveDirection movingDirection)
        {
            OnMoveButtonTouched?.Invoke(movingDirection);
        }

        public void InvokeStartMoving()
        {
            OnPlayerStartMoving?.Invoke();
            r_globalEventHandler.InvokeOnPlayerStartMoving();
        }

        public void InvokeDied()
        {
            OnPlayerDied?.Invoke();
            r_globalEventHandler.InvokeOnPlayerDied();
        }
    }
}