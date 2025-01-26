using System.Collections.Generic;
using Runtime.InteractedObjects.Obstacles;
using UnityEngine;

namespace TapToStep.Scripts.Runtime.Builders.Coins
{
    public class ObstacleBuilder : MonoBehaviour
    {
        [SerializeField] private Obstacle[] _obstaclePullPrefab;
        [SerializeField] private Transform _obstacleSpawnPointHolder;
        [SerializeField] private float _yOffset;
        
        private readonly List<Transform> r_points = new();
        
        private void Start()
        {
            Initialise();
            Generate();
        }
        
        private void Initialise()
        {
            for (var i = 0; i < _obstacleSpawnPointHolder.childCount; i++)
            {
                r_points.Add(_obstacleSpawnPointHolder.GetChild(i));
            }
        }

        private void Generate()
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
                var newXPoint = Random.Range(-1.9f, 1.9f);
                point.localPosition = new Vector3(newXPoint, point.localPosition.y + _yOffset, point.localPosition.z);
                point.localRotation = Quaternion.Euler(0, Random.Range(-30f, 45f), 0);
                var temp = Instantiate(_obstaclePullPrefab[Random.Range(0, _obstaclePullPrefab.Length)], point);
                temp.Init();
            }
        }
    }
}