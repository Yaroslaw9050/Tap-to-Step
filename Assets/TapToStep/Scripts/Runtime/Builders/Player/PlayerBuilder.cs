using CompositionRoot.SO.Player.Logic;
using Runtime.Player;
using UnityEngine;

public class PlayerBuilder : MonoBehaviour
{
    [SerializeField] private PlayerSettingSO _playerSettingSo;
    [SerializeField] private GameObject _playerPrefab;

    private PlayerEntryPoint _playerEntryPoint;

    public void CreatePlayer(Vector3 position)
    {
        var yOffset = _playerPrefab.transform.position.y;
        position = new Vector3(position.x, position.y + yOffset, position.z);
        _playerEntryPoint =Instantiate(_playerPrefab, position, Quaternion.identity).GetComponent<PlayerEntryPoint>();
        _playerEntryPoint.Init();
    }
}
