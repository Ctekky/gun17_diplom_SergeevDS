using Metroidvania.BaseUnit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerGroundedState : PlayerState
    {
        protected int inputX;
        protected int inputY;
        protected bool isHeadTouchingGround;
        protected Movement Movement {
            get => movement ?? unit.GetUnitComponent<Movement>(ref movement);
        }
        private CollisionChecks CollisionChecks {
            get => collisionChecks ?? unit.GetUnitComponent<CollisionChecks>(ref collisionChecks);
        }
        private Movement movement;
        private CollisionChecks collisionChecks;
        private bool jumpInput;
        private bool interactInput;
        private bool isGrounded;
        private bool isTouchingWall;
        private bool isTouchingLedge;
        private bool rollInput;
        public PlayerGroundedState(
            Player player,
            PlayerStateMachine stateMachine,
            PlayerData playerData,
            string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
            if(CollisionChecks != null)
            {
                isGrounded = CollisionChecks.Grounded;
                isTouchingWall = CollisionChecks.WallFront;
                isHeadTouchingGround = CollisionChecks.Ceiling;
                isTouchingLedge = CollisionChecks.LedgeHorizontal;
            }
            
        }

        public override void Enter()
        {
            base.Enter();
            player.JumpState.ResetJumps();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            inputX = player.InputHandler.NormalizedInputX;
            inputY = player.InputHandler.NormalizedInputY;
            jumpInput = player.InputHandler.JumpInput;
            rollInput = player.InputHandler.RollInput;
            interactInput = player.InputHandler.InteractInput;

            if (player.InputHandler.AttackInputs[(int)CombatInputs.primary] && !isHeadTouchingGround)
            {
                stateMachine.ChangeState(player.PrimaryAttackState);
            }
            else if (player.InputHandler.SecondaryAttackStarted && !isHeadTouchingGround && player.CurrentWeaponEquip == WeaponType.bow)
            {
                stateMachine.ChangeState(player.AimState);
            }
            else if(jumpInput && player.JumpState.CanJump() && !isHeadTouchingGround)
            {
                stateMachine.ChangeState(player.JumpState);
            }
            else if(!isGrounded)
            {
                player.InAirState.StartLastMomentJumpTimer();
                stateMachine.ChangeState(player.InAirState);
            }
            else if(isTouchingWall && interactInput && isTouchingLedge)
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
            else if(rollInput)
            {
                stateMachine.ChangeState(player.RollState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}

