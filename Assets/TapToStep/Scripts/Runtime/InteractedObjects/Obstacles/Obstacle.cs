using UnityEngine;

namespace Runtime.InteractedObjects.Obstacles
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private ObstacleType _obstacleType;

        public ObstacleType ObstacleType => _obstacleType;

        public void Init()
        {
            
        }

        public void Collect()
        {
            Destroy(gameObject);
        }
    }
}