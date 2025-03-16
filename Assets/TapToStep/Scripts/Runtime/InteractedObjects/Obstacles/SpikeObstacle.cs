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
        }

        private void SetPosition()
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 
                                                  transform.localPosition.y + _yOffset, transform.localPosition.z);
        }
    }
}