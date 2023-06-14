using Metroidvania.Combat.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerAimState : PlayerAbilityState
    {
        private Weapon _weapon;
        public PlayerAimState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityZero();
            _weapon.EnterWeaponAim();
        }
        public override void Exit()
        {
            base.Exit();
            Player.InputHandler.UseSecondaryAttackPerfomedInput();
            Player.InputHandler.UseSecondaryAttackInput();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            _weapon.EnterWeaponAim();
            if (Player.InputHandler.AttackInputs[(int)CombatInputs.Secondary])
            {
                _weapon.EnterWeaponSecondary();
                StateMachine.ChangeState(Player.IdleState);
            } 
            else if (!Player.InputHandler.SecondaryAttackStarted)
            {
                _weapon.ExitWeaponAim();
                StateMachine.ChangeState(Player.IdleState);
            }
        }
        public void SetWeapon(Weapon weapon)
        {
            _weapon = weapon;
            weapon.InitializeWeapon(this, Unit);
        }
    }

}
