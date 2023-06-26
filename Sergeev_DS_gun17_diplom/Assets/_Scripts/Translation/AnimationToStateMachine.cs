using UnityEngine;

namespace Metroidvania.Enemy
{
    public class AnimationToStateMachine : MonoBehaviour
    {
        public EnemyAttackState AttackState;
        private void AnimationTrigger()
        {
            AttackState.AnimationTrigger();
        }

        private void AnimationEndTrigger()
        {
            AttackState.AnimationEndTrigger();
        }
    }
}



