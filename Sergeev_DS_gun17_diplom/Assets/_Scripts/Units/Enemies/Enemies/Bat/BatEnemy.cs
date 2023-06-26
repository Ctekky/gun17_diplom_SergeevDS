using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BatEnemy : BaseEnemy
    {
        public BatIdleState IdleState { get; private set; }
        public BatChasingPlayerState ChasingPlayerState { get; private set; }
        public BatMeleeAttackState MeleeAttackState { get; private set; }
        public BatReturnToStartingPositionState ReturnToStartingPositionState { get; private set; }
        [SerializeField] private Transform meleeAttackPosition;
        [SerializeField] private Transform playerCheckPosition;
        public Vector2 startingPosition;

        public override void Awake()
        {
            base.Awake();
            startingPosition = transform.position;
            IdleState = new BatIdleState(this, StateMachine, enemyData, "idle", this);
            MeleeAttackState =
                new BatMeleeAttackState(this, StateMachine, enemyData, "meleeAttack", meleeAttackPosition, this);
            ChasingPlayerState = new BatChasingPlayerState(this, StateMachine, enemyData, "idle", this);
            ReturnToStartingPositionState =
                new BatReturnToStartingPositionState(this, StateMachine, enemyData, "idle", startingPosition, this);
        }

        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(IdleState);
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
            var collider = Physics2D.OverlapCircle(position, enemyData.maxAggroDistance, enemyData.playerLayer);
            return collider.transform;
        }

        public bool CheckOnStartingPosition()
        {
            return Vector2.Distance(transform.position, startingPosition) <= 0.05f;
        }

        public override void OnDrawGizmos()
        {
            Gizmos.DrawLine(playerCheckPosition.position, Vector2.right);
            Gizmos.DrawWireSphere(transform.position, enemyData.maxAggroDistance);
        }
    }
}