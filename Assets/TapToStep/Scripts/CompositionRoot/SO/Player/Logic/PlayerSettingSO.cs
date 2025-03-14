using System;
using UnityEngine;

namespace CompositionRoot.SO.Player.Logic
{
    [CreateAssetMenu(fileName = "PlayerSettingSO", menuName = "Player/Parameters", order = 1)]
    public class PlayerSettingSO : ScriptableObject
    {
        [SerializeField] private double _turnSpeed = 1;
        [SerializeField] private double _stepLenght = 1;
        [SerializeField] private double _stepSpeed = 1;
        public double StepLenght => _stepLenght;

        public double StepSpeed => _stepSpeed;

        public PlayerSetting ConvertToClass()
        {
            return new PlayerSetting(_turnSpeed, _stepLenght, _stepSpeed);
        }
    }

    [Serializable]
    public class PlayerSetting
    {
        private double turnSpeed;
        private double stepLenght;
        private double stepSpeed;

        public PlayerSetting(double turnSpeed, double stepLenght, double stepSpeed)
        {
            this.turnSpeed = turnSpeed;
            this.stepLenght = stepLenght;
            this.stepSpeed = stepSpeed;
        }
    }
}