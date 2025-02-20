using UnityEngine;

namespace Runtime.InteractedObjects.Obstacles
{
    public class SawObstacle : Obstacle
    {
        [SerializeField] private GameObject _parent;

        public override void Collect()
        {
            Destroy(_parent.gameObject);
        }
    }
}