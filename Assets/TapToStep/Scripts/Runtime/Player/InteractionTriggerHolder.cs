using CompositionRoot.Constants;
using Core.Service.GlobalEvents;
using Core.Service.LocalUser;
using Runtime.InteractedObjects.Collectables;
using Runtime.InteractedObjects.Obstacles;
using UnityEngine;

namespace Runtime.Player
{
    public class InteractionTriggerHolder : MonoBehaviour
    {
        private GlobalEventsHolder _globalEventsHolder;
        private LocalPlayerService _localPlayerService;

        public void Initialise(GlobalEventsHolder globalEventsHolder, LocalPlayerService localPlayerService)
        {
            _globalEventsHolder = globalEventsHolder;
            _localPlayerService = localPlayerService;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(InteractionAssets.INTERACTION_TAG))
            {
                if (other.TryGetComponent<Bit>(out var coin))
                {
                    _localPlayerService.AddBits(coin.Value);
                    _globalEventsHolder.InvokeOnCollectablesChanged();
                    coin.Collect();
                    return;
                }

                if (other.TryGetComponent<ObstacleTrigger>(out var trigger))
                {
                    switch (trigger.Obstacle.ObstacleType)
                    {
                        case ObstacleType.OneTouch:
                            trigger.Collect();
                            _globalEventsHolder.PlayerEvents.InvokeOnDied();
                            break;
                    }
                }
            }
            else if (other.CompareTag(InteractionAssets.FINISH_TAG))
            {
                _globalEventsHolder.PlayerEvents.InvokeOnTouchedToEndOfLocation(transform.position.z);
            }
        }
    }
}