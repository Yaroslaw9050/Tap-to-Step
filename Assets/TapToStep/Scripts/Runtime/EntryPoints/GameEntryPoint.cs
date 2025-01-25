using Runtime.Service.LocationGenerator;
using UnityEngine;

namespace TapToStep.Scripts.Runtime.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private PlayerBuilder _playerBuilder;
        [SerializeField] private LocationBuilder _locationBuilder;
        
        private async void Start()
        {
            await _locationBuilder.GenerateNewLocationAsync();
            _playerBuilder.CreatePlayer(Vector3.zero);
        }
    }
}