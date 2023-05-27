using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Player
{
    public class PlayerAbilityState : PlayerState
    {
        protected bool isAbilityDone;
        protected Movement Movement {
            get => movement ?? unit.GetUnitComponent<Movement>(ref movement);
        }
        protected CollisionChecks CollisionChecks {
            get => collisionChecks ?? unit.GetUnitComponent<CollisionChecks>(ref collisionChecks);
        }
        private Movement movement;
        private CollisionChecks collisionChecks;
        private bool isGrounded;
        public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
            if(CollisionChecks) isGrounded = CollisionChecks.Grounded;
        }

        public override void Enter()
        {
            base.Enter();
            isAbilityDone = false;
        }

        public override void Exit()
        {
            base.Exit();
            
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(isAbilityDone)
            {
                if (isGrounded && Movement?.CurrentVelocity.y < 0.01f)
                {
                    stateMachine.ChangeState(player.IdleState);
                }
                else
                {
                    stateMachine.ChangeState(player.InAirState);
                }
            }    
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}

