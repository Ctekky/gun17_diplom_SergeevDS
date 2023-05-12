using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{

    public class PlayerState
    {
        protected Player player;
        protected PlayerStateMachine stateMachine;
        protected PlayerData playerData;

        protected bool isAnimationEnd;
        protected float startTime;
        private string animBoolName;

        public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
        {
            this.player = player;
            this.stateMachine = stateMachine;
            this.playerData = playerData;
            this.animBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            DoChecks();
            player.Animator.SetBool(animBoolName, true);
            startTime = Time.time;
            isAnimationEnd = false;

        }
        public virtual void Exit()
        {
            player.Animator.SetBool(animBoolName, false);
        }
        public virtual void LogicUpdate() { }
        public virtual void PhysicsUpdate()
        {
            DoChecks();

        }
        public virtual void DoChecks() { }
        public virtual void AnimationTrigger() { }
        public virtual void AnimationEndTrigger() => isAnimationEnd = true;
    }
}
