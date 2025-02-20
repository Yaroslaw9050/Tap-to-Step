using System;
using CompositionRoot.SO.Player.Logic;
using Core.Service.Leaderboard;
using Runtime.EntryPoints.EventHandlers;
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
    private GameEventHandler _gameEventHandler;
    private LeaderboardService _leaderboardService;
    public PlayerSettingSO PlayerSettingSo => _playerSettingSo;
    public PlayerEntryPoint PlayerEntryPoint => _playerEntryPoint;

    [Inject]
    public void Constructor(GameEventHandler gameEventHandler, PlayerPerkSystem playerPerkSystem, LeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
        _gameEventHandler  = gameEventHandler;
        _playerPerkSystem = playerPerkSystem;
    }
    
    public void CreatePlayer(Vector3 position, Transform backgroundTransform)
    {
        _playerPerkSystem.LoadAllPerks();
        var yOffset = _playerPrefab.transform.position.y;
        position = new Vector3(position.x, position.y + yOffset, position.z);
        
        _playerEntryPoint = Instantiate(_playerPrefab, position, Quaternion.identity).GetComponent<PlayerEntryPoint>();
        _playerEntryPoint.Init(_gameEventHandler, _playerSettingSo, _playerPerkSystem, _leaderboardService);
        backgroundTransform.SetParent(_playerEntryPoint.transform);
    }

    public void DestroyPlayer()
    {
        _playerPerkSystem.SaveAllPerks();
        _playerEntryPoint.Destruct();
    }
}