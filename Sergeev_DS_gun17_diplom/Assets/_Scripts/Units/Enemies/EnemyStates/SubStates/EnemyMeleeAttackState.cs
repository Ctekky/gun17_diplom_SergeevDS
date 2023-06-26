using UnityEngine;
using Metroidvania.Interfaces;

namespace Metroidvania.Enemy
{
    public class EnemyMeleeAttackState : EnemyAttackState
    {
        protected EnemyMeleeAttackState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, Transform attackPosition) : base(enemy, stateMachine, enemyData, animBoolName,
            attackPosition)
        {
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
            var detectedObjects = Physics2D.OverlapCircleAll(AttackPosition.position,
                EnemyData.meleeAttackRadius, EnemyData.playerLayer);
            foreach (var collider in detectedObjects)
            {
                var damageable = collider.GetComponent<IDamageable>();
                var finalDamage = UnitStats.DoDamage(EnemyData.meleeAttackDamage.GetValue());
                damageable?.Damage(finalDamage);
                var knockbackable = collider.GetComponent<IKnockbackable>();
                knockbackable?.Knockback(EnemyData.knockbackAngle, EnemyData.knockbackStrength,
                    Movement.FacingDirection);
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
        }
    }
}