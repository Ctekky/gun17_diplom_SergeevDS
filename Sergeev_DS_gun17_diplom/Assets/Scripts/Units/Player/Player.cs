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
        #endregion

        #region Components
        public Animator Animator { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        public Rigidbody2D RigidBody { get; private set; }
        [SerializeField]
        private PlayerData playerData;
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
        }
        private void Start()
        {
            Animator = GetComponent<Animator>();
            InputHandler = GetComponent<PlayerInputHandler>();
            RigidBody = GetComponent<Rigidbody2D>();

            FacingDirection = 1;
            StateMachine.Initialize(IdleState);
        }
        private void Update()
        {
            CurrentVelocity = RigidBody.velocity;
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
            RigidBody.velocity = workVector;
            CurrentVelocity = workVector;
        }
        #endregion

        #region Check Func
        public void CheckFlip(int inputX)
        {
            if (inputX != 0 && inputX != FacingDirection) Flip();
        }
        #endregion

        #region Other Func
        private void Flip()
        {
            FacingDirection *= -1;
            transform.Rotate(0f, 180f, 0f);
        }
        #endregion

    }
}