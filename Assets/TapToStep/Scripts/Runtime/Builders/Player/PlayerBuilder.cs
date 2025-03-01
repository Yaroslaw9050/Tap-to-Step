using TapToStep.Scripts.Core.Service.LocalUser;
using CompositionRoot.SO.Player.Logic;
using Core.Service.GlobalEvents;
using Runtime.Player;
using Runtime.Player.Perks;
using UnityEngine;
using Zenject;

public class PlayerBuilder : MonoBehaviour
{
    [SerializeField] private PlayerSettingSO _playerSettingSo;
    [SerializeField] private GameObject _playerPrefab;
    
    private PlayerPerkSystem _playerPerkSystem;
    private PlayerEntryPoint _playerEntryPoint;
    private GlobalEventsHolder _globalEventsHolder;
    private LocalPlayerService _localPlayerService;
    public PlayerSettingSO PlayerSettingSo => _playerSettingSo;
    public PlayerEntryPoint PlayerEntryPoint => _playerEntryPoint;

    [Inject]
    public void Constructor(GlobalEventsHolder globalEventsHolder, 
        PlayerPerkSystem playerPerkSystem, LocalPlayerService localPlayerService)
    {
        _globalEventsHolder  = globalEventsHolder;
        _playerPerkSystem = playerPerkSystem;
        _localPlayerService = localPlayerService;
    }
    
    public void CreatePlayer(Vector3 spawnPosition, Transform backgroundTransform)
    {
        _playerPerkSystem.LoadAllPerks();
        spawnPosition = CalculateSpawnPosition(spawnPosition);

        _playerEntryPoint = Instantiate(_playerPrefab, spawnPosition, Quaternion.identity).GetComponent<PlayerEntryPoint>();
        _playerEntryPoint.Init(_globalEventsHolder, _playerSettingSo, _playerPerkSystem, _localPlayerService);
        
        backgroundTransform.SetParent(_playerEntryPoint.transform);
    }

    public void DestroyPlayer()
    {
        _playerPerkSystem.SaveAllPerks();
        _playerEntryPoint.Destruct();
    }

    private Vector3 CalculateSpawnPosition(Vector3 position)
    {
        var yOffset = _playerPrefab.transform.position.y;
        position = new Vector3(position.x, position.y + yOffset, position.z);
        return position;
    }
}