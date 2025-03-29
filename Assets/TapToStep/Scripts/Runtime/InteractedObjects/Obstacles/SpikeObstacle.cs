using UnityEngine;

namespace Runtime.InteractedObjects.Obstacles
{
    public class SpikeObstacle : Obstacle
    {
        public override void Collect()
        {
            Destroy(gameObject);
        }
        
        protected override void Init()
        {
            SetPosition();
            SetRandomRotation();
        }

        protected override void SetRandomRotation()
        {
            var yRotation = Random.Range(0f,1f) < 0.5f ? 0f: 180f;
            transform.localRotation = Quaternion.Euler(0f,yRotation,0f);
        }

        private void SetPosition()
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 
                                                  transform.localPosition.y + _yOffset, transform.localPosition.z);
        }
    }
}