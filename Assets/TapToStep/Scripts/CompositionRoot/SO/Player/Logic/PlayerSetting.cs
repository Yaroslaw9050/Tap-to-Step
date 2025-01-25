using UnityEngine;

namespace CompositionRoot.SO.Player.Logic
{
    [CreateAssetMenu(fileName = "PlayerSettingSO", menuName = "Player/Parameters", order = 1)]
    public class PlayerSettingSO : ScriptableObject
    {
        [SerializeField] private float _stepDistance = 1;
        [SerializeField] private float _stepTime = 1;
        [SerializeField] private float _distance;
        public float StepDistance => _stepDistance;

        public float StepTime => _stepTime;

        public float Distance
        {
            get => _distance;
            set
            {
                if(value > _distance)
                {
                    _distance = value;
                }
            }
        }
    }
}