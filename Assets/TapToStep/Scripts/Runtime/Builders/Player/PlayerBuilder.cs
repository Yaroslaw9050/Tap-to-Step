using System;
using CompositionRoot.SO.Player.Logic;
using Runtime.EntryPoints.EventHandlers;
using Runtime.Player;
using UnityEngine;
using Zenject;

public class PlayerBuilder : MonoBehaviour
{
    [SerializeField] private PlayerSettingSO _playerSettingSo;
    [SerializeField] private GameObject _playerPrefab;
    
    private PlayerEntryPoint _playerEntryPoint;
    private GameEventHandler _gameEventHandler;
    public PlayerSettingSO PlayerSettingSo => _playerSettingSo;

    [Inject]
    public void Constructor(GameEventHandler gameEventHandler)
    {
        _gameEventHandler  = gameEventHandler;
    }
    
    public void CreatePlayer(Vector3 position)
    {
        _playerSettingSo.LoadFromMemory();
        
        var yOffset = _playerPrefab.transform.position.y;
        position = new Vector3(position.x, position.y + yOffset, position.z);
        _playerEntryPoint = Instantiate(_playerPrefab, position, Quaternion.identity).GetComponent<PlayerEntryPoint>();
        _playerEntryPoint.Init(_gameEventHandler, _playerSettingSo);
    }

    private void OnDestroy()
    {
        _playerEntryPoint.Destruct();
    }

    private void OnApplicationQuit()
    {
        _playerSettingSo.SaveToMemory();
    }
}