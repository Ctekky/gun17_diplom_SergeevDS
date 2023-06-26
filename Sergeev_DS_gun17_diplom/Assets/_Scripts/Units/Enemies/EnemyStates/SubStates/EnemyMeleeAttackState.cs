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
            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(AttackPosition.position,
                EnemyData.meleeAttackRadius, EnemyData.playerLayer);
            foreach (Collider2D collider in detectedObjects)
            {
                IDamageable damageable = collider.GetComponent<IDamageable>();
                var finalDamage = UnitStats.DoDamage(EnemyData.meleeAttackDamage.GetValue());
                if (damageable != null) damageable.Damage(finalDamage);
                IKnockbackable knockbackable = collider.GetComponent<IKnockbackable>();
                if (knockbackable != null)
                    knockbackable.Knockback(EnemyData.knockbackAngle, EnemyData.knockbackStrength,
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