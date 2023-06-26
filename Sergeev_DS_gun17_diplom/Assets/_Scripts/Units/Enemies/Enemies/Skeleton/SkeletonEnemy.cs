using UnityEngine;

namespace Metroidvania.Enemy
{
    public class SkeletonEnemy : BaseEnemy
    {
        public SkeletonIdleState IdleState { get; private set; }
        public SkeletonMoveState MoveState { get; private set; }
        public SkeletonChasingPlayerState ChasingPlayerState { get; private set; }
        public SkeletonLookForPlayerState LookForPlayerState { get; private set; }
        public SkeletonMeleeAttackState MeleeAttackState1 { get; private set; }
        public SkeletonMeleeAttackState MeleeAttackState2 { get; private set; }

        [SerializeField] private Transform meleeAttackPosition;
        [SerializeField] private Transform playerCheckPosition;

        public override void Awake()
        {
            base.Awake();
            IdleState = new SkeletonIdleState(this, StateMachine, enemyData, "idle", this);
            MoveState = new SkeletonMoveState(this, StateMachine, enemyData, "move", this);
            ChasingPlayerState = new SkeletonChasingPlayerState(this, StateMachine, enemyData, "move", this);
            LookForPlayerState = new SkeletonLookForPlayerState(this, StateMachine, enemyData, "idle", this);
            MeleeAttackState1 = new SkeletonMeleeAttackState(this, StateMachine, enemyData, "meleeAttack_1",
                meleeAttackPosition, this);
            MeleeAttackState2 = new SkeletonMeleeAttackState(this, StateMachine, enemyData, "meleeAttack_2",
                meleeAttackPosition, this);
        }

        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(MoveState);
        }

        public override bool CheckPlayerInMaxRange()
        {
            var position = transform.position;
            return Physics2D.OverlapCircle(position, enemyData.maxAggroDistance, enemyData.playerLayer);
        }


        public Transform GetPlayerPosition()
        {
            if (!CheckPlayerInMaxRange()) return null;
            var position = transform.position;
            var overlapCircle = Physics2D.OverlapCircle(position, enemyData.maxAggroDistance, enemyData.playerLayer);
            return overlapCircle.transform;
        }

        public override void OnDrawGizmos()
        {
            Gizmos.DrawLine(playerCheckPosition.position, Vector2.right);
            Gizmos.DrawWireSphere(transform.position, enemyData.maxAggroDistance);
        }
    }
}