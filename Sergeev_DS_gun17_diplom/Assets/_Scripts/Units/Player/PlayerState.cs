using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Player
{
    public class PlayerState
    {
        protected readonly Unit Unit;
        protected readonly Player Player;
        protected readonly PlayerStateMachine StateMachine;
        protected readonly PlayerData PlayerData;

        protected bool IsExitingState;
        protected bool IsAnimationEnd;
        protected float StartTime;
        private readonly string _animBoolName;

        protected PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
        {
            Player = player;
            StateMachine = stateMachine;
            PlayerData = playerData;
            _animBoolName = animBoolName;
            Unit = player.Unit;
        }

        public virtual void Enter()
        {
            DoChecks();
            Player.Animator.SetBool(_animBoolName, true);
            StartTime = Time.time;
            IsAnimationEnd = false;
            IsExitingState = false;
        }
        public virtual void Exit()
        {
            Player.Animator.SetBool(_animBoolName, false);
            IsExitingState = true;
        }
        public virtual void LogicUpdate() { }
        public virtual void PhysicsUpdate()
        {
            DoChecks();
        }
        public virtual void DoChecks() { }
        public virtual void AnimationTrigger() { }
        public virtual void AnimationEndTrigger() => IsAnimationEnd = true;
    }
}
