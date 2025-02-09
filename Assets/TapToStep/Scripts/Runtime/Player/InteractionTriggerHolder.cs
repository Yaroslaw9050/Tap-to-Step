using Runtime.InteractedObjects.Collectables;
using Runtime.InteractedObjects.Obstacles;
using UnityEngine;

namespace Runtime.Player
{
    public class InteractionTriggerHolder : MonoBehaviour
    {
        private PlayerEventHandler _playerEventHandler;

        public void Initialize(PlayerEventHandler playerEventHandler)
        {
            _playerEventHandler = playerEventHandler;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Interaction"))
            {
                if (other.TryGetComponent<Bit>(out var coin))
                {
                    _playerEventHandler.InvokeTouchedToCollectables(coin.Value);
                    coin.Collect();
                    return;
                }

                if (other.TryGetComponent<Obstacle>(out var obstacle))
                {
                    switch (obstacle.ObstacleType)
                    {
                        case ObstacleType.OneTouch:
                            obstacle.Collect();
                            _playerEventHandler.InvokeDied();
                            break;
                    }
                }
            }
            else if (other.CompareTag("Finish"))
            {
                _playerEventHandler.InvokeTouchedToEndOfLocation(transform.position.z);
            }
        }
    }
}