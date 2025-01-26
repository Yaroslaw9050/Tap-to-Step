using System.Collections.Generic;
using Runtime.InteractedObjects.Collectables;
using UnityEngine;

namespace TapToStep.Scripts.Runtime.Builders.Coins
{
    public class CoinBuilder : MonoBehaviour
    {
        [SerializeField] private Coin[] _coinsPrefabPull;
        [SerializeField] private Transform _coinSpawnPointHolder;
        [SerializeField] private float _yOffset;
        
        private readonly List<Transform> r_points = new();
        
        private void Start()
        {
            Initialise();
            Generate();
        }
        
        private void Initialise()
        {
            for (var i = 0; i < _coinSpawnPointHolder.childCount; i++)
            {
                r_points.Add(_coinSpawnPointHolder.GetChild(i));
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
                point.localRotation = Quaternion.Euler(0, Random.Range(-45f, 45f), 0);
                var temp = Instantiate(_coinsPrefabPull[Random.Range(0, _coinsPrefabPull.Length)], point);
                temp.Init();
            }
        }
    }
}