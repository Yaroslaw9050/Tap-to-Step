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
        
        private readonly GameEventHandler r_gameEventHandler;
        private readonly PlayerEntryPoint r_entryPoint;
        
        public PlayerEventHandler(PlayerEntryPoint entryPoint, GameEventHandler gameEventHandler)
        {
            r_entryPoint = entryPoint;
            r_gameEventHandler = gameEventHandler;
        }
        
        public void InvokeTouchedToCollectables(int value)
        {
            r_gameEventHandler.InvokeOnCollectablesChanged(value);
        }

        public void InvokeMoveButtonTouched(MoveDirection movingDirection)
        {
            OnMoveButtonTouched?.Invoke(movingDirection);
        }

        public void InvokeStartMoving()
        {
            OnPlayerStartMoving?.Invoke();
            r_gameEventHandler.InvokeOnPlayerStartMoving();
        }

        public void InvokeDied()
        {
            OnPlayerDied?.Invoke();
            r_gameEventHandler.InvokeOnPlayerDied();
        }
    }
}