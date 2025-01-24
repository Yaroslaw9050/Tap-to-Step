using System.Collections.Generic;
using UnityEngine;

namespace SO.Location.Logic
{
    [CreateAssetMenu(fileName = "LocationSO", menuName = "Location/SwitchedLocation", order = 1)]
    public class SwitchedLocationSO : ScriptableObject, ILocation
    {
        public GameObject StartTransitionPrefab;
        public GameObject[] MiddleLocationPrefabs;
        public GameObject EndTransitionPrefab;

        public float LocationOffset;
        
        public Queue<GameObject> GetLocations(int length)
        {
            if (length <= 0)
            {
                Debug.LogError("Location length equal 0! You can't use this!");
                return new Queue<GameObject>(0);
            }

            var elementsQueue = new Queue<GameObject>(length + 2);
            
            elementsQueue.Enqueue(StartTransitionPrefab);
            for (var i = 0; i < length; i++)
            {
                var middleElement = MiddleLocationPrefabs[Random.Range(0, MiddleLocationPrefabs.Length)];
                elementsQueue.Enqueue(middleElement);
            }
            elementsQueue.Enqueue(EndTransitionPrefab);

            return elementsQueue;
        }
    }
}