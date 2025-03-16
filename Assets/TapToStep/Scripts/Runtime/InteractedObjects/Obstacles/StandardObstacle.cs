namespace Runtime.InteractedObjects.Obstacles
{
    public class StandardObstacle: Obstacle
    {
        protected override void Init()
        {
            SetRandomRotation();
            SetRandomPosition();
        }

        public override void Collect()
        {
            Destroy(gameObject);
        }
    }
}