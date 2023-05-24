using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class AnimationToStateMachine : MonoBehaviour
    {
        public EnemyAttackState attackState;
        private void AnimationTrigger()
        {
            attackState.AnimationTrigger();
        }

        private void AnimationEndTrigger()
        {
            attackState.AnimationEndTrigger();
        }
    }
}



