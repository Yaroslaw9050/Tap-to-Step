using Runtime.Builders.Coins;
using UnityEngine;

namespace Runtime.Location
{
    public class SprintLocationStorage: MonoBehaviour
    {
        [SerializeField] private ObstacleBuilder _obstacleBuilder;
        [SerializeField] private BitBuilder _bitBuilder;

        public void Initialise(int playerLevel, Transform parent)
        {
            transform.SetParent(parent);
            if(_obstacleBuilder == null || _bitBuilder == null) return;
            
            _bitBuilder.Initialise();
            _obstacleBuilder.Initialise(playerLevel);
        }
    }
}