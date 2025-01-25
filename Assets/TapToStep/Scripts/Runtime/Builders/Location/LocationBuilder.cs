using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        [SerializeField] private GameObject _backgroundPrefab;
        [SerializeField] private List<SwitchedLocationSO> _supportedLocationPull;
        [SerializeField] private Transform _locationParent;
        
        private readonly List<GameObject> r_locationElementHoldersPull = new();
        private readonly List<GameObject> r_backgroundElementHoldersPull = new();
        
        private Vector3 _locationGenerationPoint;
        private bool _isFirstGeneration = true;
        private int _backgroundCounter;
        
        private const int BACKGROUND_OFFSET = 1000;

        private void Start()
        {
            GenerateNewLocationAsync().Forget();
        }

        public async UniTask GenerateNewLocationAsync()
        {
            var randomLocationIndex = Random.Range(0, _supportedLocationPull.Count);
            
            if (_isFirstGeneration)
            {
                _isFirstGeneration = !_isFirstGeneration;
                await CreateBackgroundAsync();
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
            var locationElementsHolder = new GameObject($"{locationSo.name}");
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
            r_locationElementHoldersPull.Add(locationElementsHolder);
            _locationGenerationPoint = startLocationSpawnPosition;
        }

        private void RemoveOldLocation()
        {
            if (r_locationElementHoldersPull.Count > 3)
            {
                var element = r_locationElementHoldersPull[0];
                r_locationElementHoldersPull.RemoveAt(0); 
                Destroy(element);
            }
        }

        private async UniTask CreateBackgroundAsync()
        {
            if(_backgroundPrefab == null) return;
            
            var spawnPosition = Vector3.forward * (BACKGROUND_OFFSET * _backgroundCounter);
            _backgroundCounter++;
            var temp = Instantiate(_backgroundPrefab, spawnPosition, Quaternion.identity);
            r_backgroundElementHoldersPull.Add(temp);
            await UniTask.NextFrame();
        }
    }
}