using UnityEngine;

namespace CompositionRoot.SO.Player.Logic
{
    [CreateAssetMenu(fileName = "PlayerSettingSO", menuName = "Player/Parameters", order = 1)]
    public class PlayerSettingSO : ScriptableObject
    {
        [SerializeField] private float _turnSpeed = 1;
        [SerializeField] private float _stepLenght = 1;
        [SerializeField] private float _stepSpeed = 1;
        public float StepLenght => _stepLenght;

        public float StepSpeed => _stepSpeed;
    }
}