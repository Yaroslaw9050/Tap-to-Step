using UnityEngine;

namespace Runtime.InteractedObjects.Obstacles
{
    public class HammerObstacle : Obstacle
    {
        public override void Collect()
        {
            Destroy(gameObject);
        }

        protected override void Init()
        {
            SetRandomRotation();
            SetRandomPosition();
        }

        protected override void SetRandomRotation()
        {
            var randomValue = Random.value < 0.5f ? -90 : 90;
            transform.localRotation = Quaternion.Euler(0, randomValue, 0);
        }
    }
}