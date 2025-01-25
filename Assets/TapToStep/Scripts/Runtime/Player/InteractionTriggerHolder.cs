using Runtime.Builders.Location;
using UnityEngine;

namespace Runtime.Player
{
    public class InteractionTriggerHolder : MonoBehaviour
    {
        private ILocationGenerator _locationGenerator;
        private EventHandler _eventHandler;

        public void Initialize(ILocationGenerator locationGenerator, EventHandler eventHandler)
        {
            _locationGenerator = locationGenerator;
            _eventHandler = eventHandler;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Interaction"))
            {
                // if (other.TryGetComponent<LocationEndTrigger>(out _))
                // {
                //     _locationGenerator.GenerateNewLocationAsync();
                //     return;
                // }
                //
                // if (other.TryGetComponent<Obstacle>(out var element))
                // {
                //     _eventHandler.TouchedToObstacle(element.ObstacleType);
                //     Debug.Log($"Touched to obstacle by type: {element.ObstacleType}");
                //     return;
                // }
            }
        }
    }
}