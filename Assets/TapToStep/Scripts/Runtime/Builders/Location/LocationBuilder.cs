using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Builders.Location;
using SO.Location.Logic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Service.LocationGenerator
{
    public class LocationBuilder: MonoBehaviour, ILocationGenerator
    {
        [Header("Locations")]
        [SerializeField] private List<SwitchedLocationSO> _supportedLocationPull;
        [SerializeField] private Transform _locationParent;
        
        private List<GameObject> _locationElementHoldersPull;
        private Vector3 _locationGenerationPoint;
        private bool _isFirstGeneration = true;
        
        public async UniTask GenerateNewLocationAsync()
        {
            var randomLocationIndex = Random.Range(0, _supportedLocationPull.Count);
            
            if (_isFirstGeneration)
            {
                _isFirstGeneration = !_isFirstGeneration;
                await CreateLocationAsync(_supportedLocationPull[0], 20);
                await CreateLocationAsync(_supportedLocationPull[randomLocationIndex], 20);
            }
            else
            {
                await CreateLocationAsync(_supportedLocationPull[randomLocationIndex]);
                RemoveOldLocation();
            }
        }

        private async UniTask CreateLocationAsync(SwitchedLocationSO locationSo, int millisDelay = 500)
        {
            var elementQueue = locationSo.GetLocations();
            var startLocationSpawnPosition = _locationGenerationPoint;
            var locationElementsHolder = new GameObject($"{nameof(locationSo)}");
            locationElementsHolder.transform.SetParent(_locationParent);
            
            while (elementQueue.Count > 0)
            {
                var temp = Instantiate(elementQueue.Dequeue(), startLocationSpawnPosition, Quaternion.identity);
                temp.transform.SetParent(locationElementsHolder.transform);
                await UniTask.Delay(millisDelay);

                startLocationSpawnPosition = 
                    new Vector3(
                        startLocationSpawnPosition.x,
                        startLocationSpawnPosition.y, 
                        startLocationSpawnPosition.z + locationSo.LocationOffset);
            }
            
            _locationElementHoldersPull.Add(locationElementsHolder);
            _locationGenerationPoint = startLocationSpawnPosition;
        }

        private void RemoveOldLocation()
        {
            if (_locationElementHoldersPull.Count > 3)
            {
                var element = _locationElementHoldersPull[0];
                _locationElementHoldersPull.RemoveAt(0); 
                Destroy(element);
            }
        }
    }
}