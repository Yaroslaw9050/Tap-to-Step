using UnityEngine;

namespace Runtime.InteractedObjects.Obstacles
{
    public abstract class Obstacle : MonoBehaviour
    {
        [Range(0, 1000)]
        [SerializeField] protected int _spawnInPlayerLevel = 5;
        [SerializeField] protected ObstacleType _obstacleType;
        [SerializeField] protected float _yOffset;

        public ObstacleType ObstacleType => _obstacleType;
        public int SpawnInPlayerLevel => _spawnInPlayerLevel;

        private void OnEnable()
        {
            Init();
        }

        public abstract void Collect();

        protected virtual void Init() { }
        
        protected virtual void SetRandomPosition()
        {
            transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-2f, 2f),
                transform.localPosition.y + _yOffset, transform.localPosition.z);
        }

        protected virtual void SetRandomRotation()
        {
            transform.localRotation = Quaternion.Euler(0, Random.Range(-30f, 45f), 0);
        }
    }
}