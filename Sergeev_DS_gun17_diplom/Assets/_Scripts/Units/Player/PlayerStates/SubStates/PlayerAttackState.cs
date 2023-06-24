using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.Combat.Weapon;

namespace Metroidvania.Player
{
    public class PlayerAttackState : PlayerAbilityState
    {
        private Weapon _weapon;
        private float _velocityToSet;
        private bool _setVelocity;
        private int _inputX;
        private bool _checkFlip;
        public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            _setVelocity = false;
            _weapon.EnterWeapon();
            Movement?.SetVelocityZero();
        }
        public override void Exit()
        {
            base.Exit();
            _weapon.ExitWeapon();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            _inputX = Player.InputHandler.NormalizedInputX; 
            if(_checkFlip) Movement?.CheckFlip(_inputX);
            if (_setVelocity) Movement?.SetVelocityX(_velocityToSet * Movement.FacingDirection);
        }
        #region Animation Triggers
        public override void AnimationEndTrigger()
        {
            base.AnimationEndTrigger();
            
            IsAbilityDone = true;
        }
        #endregion
        public void SetWeapon(Weapon weapon)
        {
            _weapon = weapon;
            weapon.InitializeWeapon(this, Unit);
        }
        public void SetPlayerVelocity(float velocity)
        {
            Movement?.SetVelocityX(velocity * Movement.FacingDirection);
            _velocityToSet = velocity;
            _setVelocity = true;
        }
        public void CheckFlip(bool value)
        {
            _checkFlip = value;
        }
    }
}


