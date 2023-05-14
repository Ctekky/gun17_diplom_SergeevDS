using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        #endregion

        #region Components
        public Animator Animator { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        public Rigidbody2D RB { get; private set; }
        [SerializeField]
        private PlayerData playerData;
        #endregion

        #region CheckVars
        [SerializeField]
        private Transform groundCheck;
        [SerializeField]
        private Transform wallCheck;
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
        }
        private void Start()
        {
            Animator = GetComponent<Animator>();
            InputHandler = GetComponent<PlayerInputHandler>();
            RB = GetComponent<Rigidbody2D>();
            ropeLinksCollisions = new List<RopeLinks>();
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

        public bool CheckIfTouchWallBack()
        {
            return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.groundLayer);
        }
        #endregion

        #region Other Func
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
        public void PlayerMoveRope()
        {
            if(ropeLinksCollisions.Count != 0)
            {
                ropeLinksCollisions[0].EnableMoveScript(new Vector2(playerData.ropeSwingVelocity, 0f) * InputHandler.NormalizedInputX);
            }
        }
        #endregion
    }
}