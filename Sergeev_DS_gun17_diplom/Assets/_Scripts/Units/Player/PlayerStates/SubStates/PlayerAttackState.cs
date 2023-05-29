using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.Combat.Weapon;

namespace Metroidvania.Player
{
    public class PlayerAttackState : PlayerAbilityState
    {
        private Weapon weapon;
        private float velocityToSet;
        private bool setVelocity;
        private int inputX;
        private bool checkFlip;
        public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            setVelocity = false;
            weapon.EnterWeapon();
            Movement?.SetVelocityZero();
        }
        public override void Exit()
        {
            base.Exit();
            weapon.ExitWeapon();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            inputX = player.InputHandler.NormalizedInputX; 
            if(checkFlip) Movement?.CheckFlip(inputX);
            if (setVelocity) Movement?.SetVelocityX(velocityToSet * Movement.FacingDirection);
        }
        #region Animation Triggers
        public override void AnimationEndTrigger()
        {
            base.AnimationEndTrigger();
            isAbilityDone = true;
        }
        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
        }
        #endregion
        public void SetWeapon(Weapon weapon)
        {
            this.weapon = weapon;
            weapon.InitializeWeapon(this, unit);
        }
        public void SetPlayerVelocity(float velocity)
        {
            Movement?.SetVelocityX(velocity * Movement.FacingDirection);
            velocityToSet = velocity;
            setVelocity = true;
        }
        public void CheckFlip(bool value)
        {
            checkFlip = value;
        }
    }
}


