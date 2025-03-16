using UnityEngine;

namespace Runtime.InteractedObjects.Obstacles
{
    public class ObstacleTrigger: MonoBehaviour
    {
        [SerializeField] private Obstacle _obstacle;
        public Obstacle Obstacle => _obstacle;

        
        public void Collect()
        {
            _obstacle.Collect();
        }
    }
}