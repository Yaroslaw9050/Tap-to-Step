using System;
using System.Collections.Generic;
using CompositionRoot.SO.Location.Logic;
using Cysharp.Threading.Tasks;
using Runtime.Builders.Location;
using Runtime.EntryPoints.EventHandlers;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Runtime.Service.LocationGenerator
{
    public class LocationBuilder: MonoBehaviour, ILocationGenerator
    {
        [Header("LocationLikeChildToPlayer")]
        [SerializeField] private GameObject _staticBackgroundPrefab;
        
        [Header("Locations")] 
        [SerializeField] private GameObject _backgroundPrefab;
        [SerializeField] private List<SwitchedLocationSO> _supportedLocationPull;
        [SerializeField] private Transform _locationParent;

        private Transform _staticBackgroundTransform;
        
        private Vector3 _locationGenerationPoint;
        private bool _isFirstGeneration = true;
        private int _backgroundCounter;
        private long _nexBackgroundSpawnTriggerDistance;

        private readonly List<GameObject> r_locationElementHoldersPull = new();
        private readonly List<GameObject> r_backgroundElementHoldersPull = new();
        public Transform StaticBackgroundTransform => _staticBackgroundTransform;
        private GameEventHandler _gameEventHandler;

        private const int BACKGROUND_OFFSET = 1000;
        

        [Inject]
        public void Constructor(GameEventHandler gameEventHandler)
        {
            _gameEventHandler = gameEventHandler;
        }
        
        private void Start()
        {
            _nexBackgroundSpawnTriggerDistance = BACKGROUND_OFFSET;
            _gameEventHandler.OnPlayerTouchedToEndOfLocation += PlayerTouchedToEndOfLocation;
        }

        private void OnDestroy()
        {
            _gameEventHandler.OnPlayerTouchedToEndOfLocation -= PlayerTouchedToEndOfLocation;
        }

        private void PlayerTouchedToEndOfLocation(float playerZPosition)
        {
            GenerateNewLocationAsync().Forget();

            if (playerZPosition >= _nexBackgroundSpawnTriggerDistance)
            {
                _nexBackgroundSpawnTriggerDistance += BACKGROUND_OFFSET;
                CreateBackgroundAsync().Forget();
            }
        }

        public async UniTask GenerateNewLocationAsync()
        {
            var randomLocationIndex = Random.Range(0, _supportedLocationPull.Count);
            
            if (_isFirstGeneration)
            {
                _isFirstGeneration = !_isFirstGeneration;
                await CreateBackgroundAsync();
                await CreateBackgroundAsync();
                await CreateLocationAsync(_supportedLocationPull[0], 20);
                await CreateLocationAsync(_supportedLocationPull[randomLocationIndex], 20);
                _staticBackgroundTransform = Instantiate(_staticBackgroundPrefab).transform;
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

        private void RemoveOldBackground()
        {
            if (r_backgroundElementHoldersPull.Count > 2)
            {
                var element = r_backgroundElementHoldersPull[0];
                r_backgroundElementHoldersPull.RemoveAt(0); 
                Destroy(element);
            }
        }

        private async UniTask CreateBackgroundAsync()
        {
            var spawnPosition = Vector3.forward * (BACKGROUND_OFFSET * _backgroundCounter);
            _backgroundCounter++;
            var temp = Instantiate(_backgroundPrefab, spawnPosition, Quaternion.identity);
            temp.transform.SetParent(_locationParent);
            r_backgroundElementHoldersPull.Add(temp);
            await UniTask.NextFrame();

            RemoveOldBackground();
        }
    }
}