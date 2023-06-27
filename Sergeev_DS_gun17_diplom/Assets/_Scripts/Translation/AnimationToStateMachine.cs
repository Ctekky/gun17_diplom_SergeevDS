using UnityEngine;

namespace Metroidvania.Enemy
{
    public class AnimationToStateMachine : MonoBehaviour
    {
        public EnemyAttackState AttackState;
        public EnemyTeleportInState TeleportInState;
        public EnemyTeleportOutState TeleportOutState;
        private void AnimationTrigger()
        {
            AttackState?.AnimationTrigger();
        }

        private void AnimationEndTrigger()
        {
            AttackState?.AnimationEndTrigger();
            TeleportInState?.AnimationEndTrigger();
            TeleportOutState?.AnimationEndTrigger();
        }
    }
}



