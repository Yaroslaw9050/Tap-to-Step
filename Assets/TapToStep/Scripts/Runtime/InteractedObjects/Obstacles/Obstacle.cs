using UnityEngine;

namespace Runtime.InteractedObjects.Obstacles
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] protected ObstacleType _obstacleType;

        public ObstacleType ObstacleType => _obstacleType;

        private void OnEnable()
        {
            Init();
        }

        public virtual void Collect()
        {
            Destroy(gameObject);
        }

        protected virtual void Init()
        { }
    }
}