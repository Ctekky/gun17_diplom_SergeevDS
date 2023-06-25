using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerJumpState : PlayerAbilityState
    {
        private int _jumpsLeft;
        public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
            _jumpsLeft = playerData.jumpCount;
        }
        public override void Enter()
        {
            base.Enter();
            Player.InputHandler.UseJumpInput();
            Movement?.SetVelocityY(PlayerData.jumpVelocity);
            Player.audioManager.PlaySFX((int)SFXSlots.WomanSigh);
            IsAbilityDone = true;
            _jumpsLeft--;
            Player.InAirState.SetIsJumping();
        }
        public bool CanJump()
        {
            if (_jumpsLeft > 0) return true;
            else return false;
        }
        public void ResetJumps() => _jumpsLeft = PlayerData.jumpCount;
        public void DecreaseJumps() => _jumpsLeft--;
    }
}
