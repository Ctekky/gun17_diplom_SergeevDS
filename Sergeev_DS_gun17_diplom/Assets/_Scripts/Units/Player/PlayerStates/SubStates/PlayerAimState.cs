using Metroidvania.Combat.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerAimState : PlayerAbilityState
    {
        private Weapon weapon;
        public PlayerAimState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void AnimationEndTrigger()
        {
            base.AnimationEndTrigger();
        }

        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityZero();
        }

        public override void Exit()
        {
            base.Exit();
            player.InputHandler.UseSecondaryAttackPerfomedInput();
            player.InputHandler.UseSecondaryAttackInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary])
            {
                weapon.EnterWeaponSecondary();
                stateMachine.ChangeState(player.IdleState);
            } 
            else if (!player.InputHandler.SecondaryAttackStarted)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        public void SetWeapon(Weapon weapon)
        {
            this.weapon = weapon;
            weapon.InitializeWeapon(this, unit);
        }
    }

}
