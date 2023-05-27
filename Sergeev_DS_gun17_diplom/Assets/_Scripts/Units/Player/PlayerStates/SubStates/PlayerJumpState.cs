using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerJumpState : PlayerAbilityState
    {
        private int jumpsLeft;
        public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
            jumpsLeft = playerData.jumpCount;
        }
        public override void Enter()
        {
            base.Enter();
            player.InputHandler.UseJumpInput();
            Movement?.SetVelocityY(playerData.jumpVelocity);
            isAbilityDone = true;
            jumpsLeft--;
            player.InAirState.SetIsJumping();
        }
        public bool CanJump()
        {
            if (jumpsLeft > 0) return true;
            else return false;
        }
        public void ResetJumps() => jumpsLeft = playerData.jumpCount;
        public void DecreaseJumps() => jumpsLeft--;
    }
}
