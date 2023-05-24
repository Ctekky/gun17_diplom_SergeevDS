using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.Structs;
namespace Metroidvania.Enemy
{
    public class EnemyMeleeAttackState : EnemyAttackState
    {
        protected AttackDetails attackDetails;
        public EnemyMeleeAttackState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, Transform attackPosition) : base(enemy, stateMachine, enemyData, animBoolName, attackPosition)
        {
        }
        public override void Enter()
        {
            base.Enter();
            attackDetails.damage = enemyData.meleeAttackDamage;
            attackDetails.position = enemy.AliveGO.transform.position;
        }
        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, enemyData.meleeAttackRadius, enemyData.playerLayer);
            foreach (Collider2D collider in detectedObjects)
            {
                collider.transform.SendMessage("GetDamage", attackDetails);
            }
        }
    }
}

