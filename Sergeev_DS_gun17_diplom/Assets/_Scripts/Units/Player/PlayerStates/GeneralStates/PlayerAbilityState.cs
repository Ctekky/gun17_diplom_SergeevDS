using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Player
{
    public class PlayerAbilityState : PlayerState
    {
        protected bool IsAbilityDone;
        protected Movement Movement {
            get => _movement ?? Unit.GetUnitComponent<Movement>(ref _movement);
        }
        protected CollisionChecks CollisionChecks {
            get => _collisionChecks ?? Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);
        }
        private Movement _movement;
        private CollisionChecks _collisionChecks;
        private bool _isGrounded;
        public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
            if(CollisionChecks) _isGrounded = CollisionChecks.Grounded;
        }

        public override void Enter()
        {
            base.Enter();
            IsAbilityDone = false;
        }

        public override void Exit()
        {
            base.Exit();
            
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(IsAbilityDone)
            {
                if (_isGrounded && Movement?.CurrentVelocity.y < 0.01f)
                {
                    StateMachine.ChangeState(Player.IdleState);
                }
                else
                {
                    StateMachine.ChangeState(Player.InAirState);
                }
            }    
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}

