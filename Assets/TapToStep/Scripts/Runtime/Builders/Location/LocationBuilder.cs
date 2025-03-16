using System.Collections.Generic;
using System.Threading;
using CompositionRoot.SO.Location.Logic;
using Core.Service.GlobalEvents;
using Core.Service.LocalUser;
using Cysharp.Threading.Tasks;
using Runtime.Location;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Runtime.Builders.Location
{
    public class LocationBuilder: MonoBehaviour, ILocationGenerator
    {
        [Header("LocationLikeChildToPlayer")]
        [SerializeField] private GameObject _welcomeTextPrefab;
        [SerializeField] private GameObject _staticBackgroundPrefab;
        
        [Header("Locations")] 
        [SerializeField] private GameObject _backgroundPrefab;
        [SerializeField] private List<SwitchedLocationSO> _supportedLocationPull;
        [SerializeField] private Transform _locationParent;

        private Transform _staticBackgroundTransform;
        private GameObject _welcomeModel;
        
        private Vector3 _locationGenerationPoint;
        private bool _isFirstGeneration = true;
        private int _backgroundCounter;
        private long _nexBackgroundSpawnTriggerDistance;

        private readonly List<GameObject> r_locationElementHoldersPull = new();
        private readonly List<GameObject> r_backgroundElementHoldersPull = new();
        private GlobalEventsHolder _globalEventsHolder;
        private LocalPlayerService _localPlayerService;
        private CancellationToken _token;

        public Transform StaticBackgroundTransform => _staticBackgroundTransform;
        private const int BACKGROUND_OFFSET = 1000;
        

        [Inject]
        public void Constructor(GlobalEventsHolder globalEventsHolder, LocalPlayerService localPlayerService)
        {
            _globalEventsHolder = globalEventsHolder;
            _localPlayerService = localPlayerService;
        }
        
        private void Start()
        {
            _nexBackgroundSpawnTriggerDistance = BACKGROUND_OFFSET;
            _globalEventsHolder.PlayerEvents.OnTouchedToEndOfLocation += PlayerTouchedToEndOfLocation;
        }

        private void OnDestroy()
        {
            _globalEventsHolder.PlayerEvents.OnTouchedToEndOfLocation -= PlayerTouchedToEndOfLocation;
        }

        private void PlayerTouchedToEndOfLocation(float playerZPosition)
        {
            GenerateNewLocationAsync(_token).Forget();

            if (playerZPosition >= _nexBackgroundSpawnTriggerDistance)
            {
                _nexBackgroundSpawnTriggerDistance += BACKGROUND_OFFSET;
                CreateBackground();
            }
        }

        public async UniTask GenerateNewLocationAsync(CancellationToken token)
        {
            _token = token;
            var randomLocationIndex = Random.Range(0, _supportedLocationPull.Count);
            
            if (_isFirstGeneration)
            {
                _isFirstGeneration = !_isFirstGeneration;
                CreateWelcomeText();
                CreateBackground();
                CreateBackground();
                await CreateLocationAsync(_supportedLocationPull[0], token);
                await CreateLocationAsync(_supportedLocationPull[randomLocationIndex], token);
                _staticBackgroundTransform = Instantiate(_staticBackgroundPrefab).transform;
            }
            else
            {
                await CreateLocationAsync(_supportedLocationPull[randomLocationIndex], token);
                CheckAndRemoveOldLocation();
                RemoveWelcomeText();
            }
        }

        private async UniTask CreateLocationAsync(SwitchedLocationSO locationSo, CancellationToken token)
        {
            var elementQueue = locationSo.GetLocations();
            var startLocationSpawnPosition = _locationGenerationPoint;
            var locationElementsHolder = new GameObject($"{locationSo.name}");
            locationElementsHolder.transform.SetParent(_locationParent);
            
            while (elementQueue.Count > 0)
            {
                if (token.IsCancellationRequested) break;
                
                var temp = Instantiate(elementQueue.Dequeue(), startLocationSpawnPosition, Quaternion.identity).
                    GetComponent<SprintLocationStorage>();
                
                temp.Initialise(_localPlayerService.GeneralPlayerLevel, locationElementsHolder.transform);
                await UniTask.NextFrame();

                startLocationSpawnPosition = 
                    new Vector3(
                        startLocationSpawnPosition.x,
                        startLocationSpawnPosition.y, 
                        startLocationSpawnPosition.z + locationSo.LocationOffset);
            }
            r_locationElementHoldersPull.Add(locationElementsHolder);
            _locationGenerationPoint = startLocationSpawnPosition;
        }

        private void CreateWelcomeText()
        {
            _welcomeModel = Instantiate(_welcomeTextPrefab, new Vector3(0f, 160f, 160f), Quaternion.identity);
        }

        private void CreateBackground()
        {
            var spawnPosition = Vector3.forward * (BACKGROUND_OFFSET * _backgroundCounter);
            var temp = Instantiate(_backgroundPrefab, spawnPosition, Quaternion.identity);
            temp.transform.SetParent(_locationParent);
            r_backgroundElementHoldersPull.Add(temp);
            _backgroundCounter++;
            CheckAndRemoveOldBackground();
        }

        private void CheckAndRemoveOldBackground()
        {
            if (r_backgroundElementHoldersPull.Count > 2)
            {
                var element = r_backgroundElementHoldersPull[0];
                r_backgroundElementHoldersPull.RemoveAt(0); 
                Destroy(element);
            }
        }

        private void CheckAndRemoveOldLocation()
        {
            if (r_locationElementHoldersPull.Count > 3)
            {
                var element = r_locationElementHoldersPull[0];
                r_locationElementHoldersPull.RemoveAt(0); 
                Destroy(element);
            }
        }

        private void RemoveWelcomeText()
        {
            if (_welcomeModel != null)
            {
                Destroy(_welcomeModel);
            }
        }
    }
}