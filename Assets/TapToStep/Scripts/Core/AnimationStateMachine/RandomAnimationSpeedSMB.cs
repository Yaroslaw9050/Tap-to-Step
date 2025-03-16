using UnityEngine;

namespace Core.AnimationStateMachine
{
    public class RandomAnimationSpeedSmb : StateMachineBehaviour
    {
        [SerializeField] private float _minSpeed = 0.9f;
        [SerializeField] private float _maxSpeed = 1.2f;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator == null) return;
            var randomSpeed = Random.Range(_minSpeed, _maxSpeed);
            animator.speed = randomSpeed;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.speed = 1f;
        }
    }
}
