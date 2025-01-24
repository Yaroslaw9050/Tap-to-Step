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

        [Header("MIN mast be less (<) than MAX")]
        [Range(1, 20)]
        public int MinElements;
        
        [Range(1, 100)]
        public int MaxElements;
        
        public Queue<GameObject> GetLocations()
        {
            if (MinElements >= MaxElements)
            {
                Debug.LogError("Min elements mast be less than max elements!");
                return new Queue<GameObject>(0);
            }

            var length = Random.Range(MinElements, MaxElements);

            var elementsQueue = new Queue<GameObject>( length + 2);
            
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