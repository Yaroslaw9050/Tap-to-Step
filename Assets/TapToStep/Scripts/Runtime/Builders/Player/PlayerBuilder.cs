using CompositionRoot.SO.Player.Logic;
using Core.Service.GlobalEvents;
using Core.Service.LocalUser;
using Runtime.Player;
using UnityEngine;
using Zenject;

public class PlayerBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    
    private PlayerEntryPoint _playerEntryPoint;
    private GlobalEventsHolder _globalEventsHolder;
    private LocalPlayerService _localPlayerService;
    public PlayerEntryPoint PlayerEntryPoint => _playerEntryPoint;

    [Inject]
    public void Constructor(GlobalEventsHolder globalEventsHolder, LocalPlayerService localPlayerService)
    {
        _globalEventsHolder  = globalEventsHolder;
        _localPlayerService = localPlayerService;
    }
    
    public void CreatePlayer(Vector3 spawnPosition, Transform backgroundTransform)
    {
        spawnPosition = CalculateSpawnPosition(spawnPosition);

        _playerEntryPoint = Instantiate(_playerPrefab, spawnPosition, Quaternion.identity).GetComponent<PlayerEntryPoint>();
        _playerEntryPoint.Init(_globalEventsHolder, _localPlayerService);
        
        backgroundTransform.SetParent(_playerEntryPoint.transform);
    }

    public void DestroyPlayer()
    {
        _playerEntryPoint.Destruct();
    }

    private Vector3 CalculateSpawnPosition(Vector3 position)
    {
        var yOffset = _playerPrefab.transform.position.y;
        position = new Vector3(position.x, position.y + yOffset, position.z);
        return position;
    }
}