using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.Structs;
using Metroidvania.BaseUnit;

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
        public PlayerAimState AimState { get; private set; }

        #endregion

        #region Components
        public Unit Unit { get; private set; }
        public Animator Animator { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        public BoxCollider2D PlayerBodyCollider { get; private set; }
        public Rigidbody2D RB { get; private set; }
        [SerializeField]
        private PlayerData playerData;
        #endregion

        #region OtherVars

        public bool IsTouchingRope { get => isTouchingRope; private set {isTouchingRope = value; }}
        public Transform CurrentRope { get; private set; }
        [SerializeField]
        private bool isTouchingRope;
        private float startCollisionTime;
        private List<RopeLinks> ropeLinksCollisions;
        public PlayerInventory Inventory { get; private set; }
        public WeaponType CurrentWeaponEquip { get => currentWeaponEquip; private set { currentWeaponEquip = value; }}
        private WeaponType currentWeaponEquip;

        #endregion
        private Vector2 workVector;
        #region Unity Func
        private void Awake()
        {
            Unit = GetComponentInChildren<Unit>();
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
            AimState = new PlayerAimState(this, StateMachine, playerData, "aim");

        }
        private void Start()
        {
            Animator = GetComponent<Animator>();
            InputHandler = GetComponent<PlayerInputHandler>();
            RB = GetComponent<Rigidbody2D>();
            ropeLinksCollisions = new List<RopeLinks>();
            PlayerBodyCollider = GetComponentInChildren<BoxCollider2D>();
            Inventory = GetComponent<PlayerInventory>();
            PrimaryAttackState.SetWeapon(Inventory.weapons[0]);
            AimState.SetWeapon(Inventory.weapons[0]);
            StateMachine.Initialize(IdleState);
            currentWeaponEquip = WeaponType.sword;
        }
        private void Update()
        {
            Unit.LogicUpdate();
            StateMachine.CurrentState.LogicUpdate();
            if (InputHandler.ChangeWeaponInput) ChangeWeapon();
        }
        private void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }
        #endregion
        
        private void ChangeWeapon()
        {
                var bowLayer = Animator.GetLayerIndex("Bow Layer");
                if (Animator.GetLayerWeight(bowLayer) == 1)
                {
                    Animator.SetLayerWeight(bowLayer, 0f);
                    PrimaryAttackState.SetWeapon(Inventory.weapons[0]);
                AimState.SetWeapon(Inventory.weapons[0]);
                currentWeaponEquip = WeaponType.sword;
                }
                else
                {
                    Animator.SetLayerWeight(bowLayer, 1f);
                    PrimaryAttackState.SetWeapon(Inventory.weapons[1]);
                AimState.SetWeapon(Inventory.weapons[1]);
                currentWeaponEquip = WeaponType.bow;
                }
            InputHandler.UseChangeWeaponInput();
        }

        #region Other Func
        private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
        private void AnimationEndTrigger() => StateMachine.CurrentState.AnimationEndTrigger();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<RopeLinks>() != null)
            {
                ropeLinksCollisions.Add(collision.GetComponent<RopeLinks>());
                CurrentRope = collision.transform;
                isTouchingRope = true;
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