using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.InteractedObjects.Obstacles;
using UnityEngine;

namespace Runtime.Builders.Coins
{
    public class ObstacleBuilder : MonoBehaviour
    {
        [SerializeField] private Obstacle[] _obstaclePullPrefab;
       
        [SerializeField] private Transform _obstacleSpawnPointHolder;
        
        private readonly List<Transform> r_points = new();

        public void Initialise(int playerLevel)
        {
            if (_obstaclePullPrefab.Length == 0)
            {
                Debug.LogWarning("You are missing add some obstacles to pull!");
                return;
            }
            
            LoadSpawnPoints();
            GenerateAsync(playerLevel).Forget();
        }
        
        private void LoadSpawnPoints()
        {
            for (var i = 0; i < _obstacleSpawnPointHolder.childCount; i++)
            {
                r_points.Add(_obstacleSpawnPointHolder.GetChild(i));
            }
        }

        private async UniTaskVoid GenerateAsync(int playerLevel)
        {
            var numberOfPoints = Random.Range(2, r_points.Count);

            var selectedPoints = new List<Transform>(numberOfPoints);
            var tempList = new List<Transform>(r_points);

            for (var i = 0; i < numberOfPoints; i++)
            {
                var randomIndex = Random.Range(0, tempList.Count);
                selectedPoints.Add(tempList[randomIndex]);
                tempList.RemoveAt(randomIndex);
            }

            foreach (var point in selectedPoints)
            {
                point.localPosition = new Vector3(point.localPosition.x, point.localPosition.y, point.localPosition.z);
                var obstaclesVariant = GetObstaclesByPlayerLevel(playerLevel);
                Instantiate(obstaclesVariant[Random.Range(0, obstaclesVariant.Length)], point);
                await UniTask.NextFrame();
            }
        }

        private Obstacle[] GetObstaclesByPlayerLevel(int playerLevel)
        {
            var obstacles = new List<Obstacle>();
            foreach (var obstacle in _obstaclePullPrefab)
            {
                if (obstacle.SpawnInPlayerLevel <= playerLevel)
                {
                    obstacles.Add(obstacle);
                }
            }

            return obstacles.ToArray();
        }
    }
}