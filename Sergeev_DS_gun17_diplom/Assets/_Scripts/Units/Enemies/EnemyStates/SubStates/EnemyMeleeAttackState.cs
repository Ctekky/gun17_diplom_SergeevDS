using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.Structs;
using Metroidvania.Interfaces;

namespace Metroidvania.Enemy
{
    public class EnemyMeleeAttackState : EnemyAttackState
    {
        public EnemyMeleeAttackState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, Transform attackPosition) : base(enemy, stateMachine, enemyData, animBoolName, attackPosition)
        {
        }
        public override void Enter()
        {
            base.Enter();
        }
        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, enemyData.meleeAttackRadius, enemyData.playerLayer);
            foreach (Collider2D collider in detectedObjects)
            {
                IDamageable damageable = collider.GetComponent<IDamageable>();
                if (damageable != null) damageable.Damage(enemyData.meleeAttackDamage);
                IKnockbackable knockbackable = collider.GetComponent<IKnockbackable>();
                if (knockbackable != null) knockbackable.Knockback(enemyData.knockbackAngle, enemyData.knockbackStrength, Movement.FacingDirection);
            }

        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (isExitingState) return;
        }
    }
}

