using System;
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

        public PlayerSetting ConvertToClass()
        {
            return new PlayerSetting(_turnSpeed, _stepLenght, _stepSpeed);
        }
    }

    [Serializable]
    public class PlayerSetting
    {
        private float turnSpeed;
        private float stepLenght;
        private float stepSpeed;

        public PlayerSetting(float turnSpeed, float stepLenght, float stepSpeed)
        {
            this.turnSpeed = turnSpeed;
            this.stepLenght = stepLenght;
            this.stepSpeed = stepSpeed;
        }
    }
}