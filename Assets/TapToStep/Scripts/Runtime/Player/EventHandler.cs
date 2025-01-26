using System;
using Runtime.InteractedObjects.Obstacles;
using UnityEngine.EventSystems;

namespace Runtime.Player
{
    public class EventHandler
    {
        public event Action<ObstacleType> OnTouchedToObstacle;
        public event Action<MoveDirection> OnMoveButtonTouched;
        public event Action OnPlayerStartMoving;
        public event Action OnPlayerDied;

        public void TouchedToObstacle(ObstacleType obstacleType)
        {
            OnTouchedToObstacle?.Invoke(obstacleType);
        }

        public void MoveButtonTouched(MoveDirection movingDirection)
        {
            OnMoveButtonTouched?.Invoke(movingDirection);
        }

        public void StartMoving()
        {
            OnPlayerStartMoving?.Invoke();
        }

        public void Died()
        {
            OnPlayerDied?.Invoke();
        }
    }
}