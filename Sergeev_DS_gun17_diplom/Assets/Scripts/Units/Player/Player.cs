using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.Structs;

namespace Metroidvania.Player
{
    public class Player : MonoBehaviour
    {
        #region StateMachineVars
        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerInAirState InAirState { get; private set; }
        public PlayerLandState LandState { get; private set; }
        public PlayerWallSlideState WallSlideState { get; private set; }
        public PlayerWallGrabState WallGrabState { get; private set; }
        public PlayerWallClimbState WallClimbState { get; private set; }
        public PlayerWallJumpState WallJumpState { get; private set; }
        public PlayerRopeGrabdState RopeGrabState { get; private set; }
        public PlayerRopeClimbState RopeClimbState { get; private set; }
        public PlayerRopeSwingState RopeSwingState { get; private set; }
        public PlayerCrouchIdleState CrouchIdleState { get; private set; }
        public PlayerCrouchMoveState CrouchMoveState { get; private set; }
        public PlayerRollState RollState { get; private set; }
        public PlayerLedgeClimbState LedgeClimbState { get; private set; }
        public PlayerAttackState PrimaryAttackState { get; private set; }
        public PlayerAttackState SecondaryAttackState { get; private set; }

        #endregion

        #region Components
        public Animator Animator { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        public BoxCollider2D PlayerBodyCollider { get; private set; }
        public Rigidbody2D RB { get; private set; }
        [SerializeField]
        private PlayerData playerData;
        #endregion

        #region CheckVars
        [SerializeField]
        private Transform groundCheck;
        [SerializeField]
        private Transform wallCheck;
        [SerializeField]
        private Transform headCheck;
        [SerializeField]
        private Transform ledgeCheck;
        public bool IsTouchingRope { get; private set; }
        public Transform CurrentRope { get; private set; }
        [SerializeField]
        private bool isTouchingRope;
        private float startCollisionTime;
        private List<RopeLinks> ropeLinksCollisions;

        #endregion

        #region OtherVars
        public Vector2 CurrentVelocity { get; private set; }
        public int FacingDirection { get; private set; }
        private Vector2 workVector;
        #endregion

        #region Unity Func
        private void Awake()
        {
            StateMachine = new PlayerStateMachine();
            IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
            MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
            JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
            InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
            LandState = new PlayerLandState(this, StateMachine, playerData, "land");
            WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
            WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
            WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
            WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
            RopeClimbState = new PlayerRopeClimbState(this, StateMachine, playerData, "ropeClimb");
            RopeGrabState = new PlayerRopeGrabdState(this, StateMachine, playerData, "ropeGrab");
            RopeSwingState = new PlayerRopeSwingState(this, StateMachine, playerData, "ropeGrab");
            CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
            CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");
            RollState = new PlayerRollState(this, StateMachine, playerData, "roll");
            LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
            PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
            SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");

        }
        private void Start()
        {
            Animator = GetComponent<Animator>();
            InputHandler = GetComponent<PlayerInputHandler>();
            RB = GetComponent<Rigidbody2D>();
            ropeLinksCollisions = new List<RopeLinks>();
            PlayerBodyCollider = GetComponent<BoxCollider2D>();
            FacingDirection = 1;
            StateMachine.Initialize(IdleState);
        }
        private void Update()
        {
            CurrentVelocity = RB.velocity;
            StateMachine.CurrentState.LogicUpdate();
        }
        private void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }
        #endregion

        #region Set Func
        public void SetVelocityX(float velocity)
        {
            workVector.Set(velocity, CurrentVelocity.y);
            RB.velocity = workVector;
            CurrentVelocity = workVector;
        }
        public void SetVelocityY(float velocity)
        {
            workVector.Set(CurrentVelocity.x, velocity);
            RB.velocity = workVector;
            CurrentVelocity = workVector;
        }

        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            workVector.Set(angle.x * velocity * direction, angle.y * velocity);
            RB.velocity = workVector;
            CurrentVelocity = workVector;
        }
        public void SetVelocity(float velocity, Vector2 direction)
        {
            workVector = direction * velocity;
            RB.velocity = workVector;
            CurrentVelocity = workVector;
        }
        public void SetVelocityZero()
        {
            RB.velocity = Vector2.zero;
            CurrentVelocity = Vector2.zero;
        }
        #endregion

        #region Check Func
        public void CheckFlip(int inputX)
        {
            if (inputX != 0 && inputX != FacingDirection) Flip();
        }
        public bool CheckIfGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.groundLayer);
        }
        public bool CheckIfTouchWall()
        {
            return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.groundLayer);
        }
        public bool CheckIfTouchingLedge()
        {
            return Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.groundLayer);
        }

        public bool CheckIfTouchWallBack()
        {
            return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.groundLayer);
        }

        public bool CheckForHeadTouch()
        {
            return Physics2D.OverlapCircle(headCheck.position, playerData.groundCheckRadius, playerData.groundLayer);
        }

        #endregion

        #region Other Func

        public void GetDamage(AttackDetails attackDetails)
        {
            Debug.Log("Player damage " + attackDetails.damage.ToString());
        }
        public Vector2 DetermineCornerPositon()
        {
            RaycastHit2D hitX = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.groundLayer);
            float distanceX = hitX.distance;
            workVector.Set(distanceX * FacingDirection, 0f);
            RaycastHit2D hitY = Physics2D.Raycast(ledgeCheck.position + (Vector3)(workVector), Vector2.down, ledgeCheck.position.y - wallCheck.position.y, playerData.groundLayer);
            float distanceY = hitY.distance;
            workVector.Set(wallCheck.position.x + (distanceX * FacingDirection), ledgeCheck.position.y - distanceY);
            return workVector;
        }

        private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
        private void AnimationEndTrigger() => StateMachine.CurrentState.AnimationEndTrigger();
        private void Flip()
        {
            FacingDirection *= -1;
            transform.Rotate(0f, 180f, 0f);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<RopeLinks>() != null)
            {
                ropeLinksCollisions.Add(collision.GetComponent<RopeLinks>());
                CurrentRope = collision.transform;
                isTouchingRope = true;
                IsTouchingRope = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<RopeLinks>() != null)
            {
                ropeLinksCollisions.Remove(collision.GetComponent<RopeLinks>());
                if (ropeLinksCollisions.Count == 0)
                {
                    isTouchingRope = false;
                    IsTouchingRope = false;
                }
            }
        }
        public void SetColliderHeight(float height)
        {
            Vector2 center = PlayerBodyCollider.offset;
            workVector.Set(PlayerBodyCollider.size.x, height);
            center.y += (height - PlayerBodyCollider.size.y) / 2;
            PlayerBodyCollider.size = workVector;
            PlayerBodyCollider.offset = center;
        }
        public void PlayerMoveRope()
        {
            if(ropeLinksCollisions.Count != 0)
            {
                ropeLinksCollisions[0].EnableMoveScript(new Vector2(playerData.ropeSwingVelocity, 0f) * InputHandler.NormalizedInputX);
            }
        }
        public void SetPlayerLayer(LayerMask layer) => gameObject.layer = (int)Mathf.Log(layer.value, 2);
        #endregion
    }
}