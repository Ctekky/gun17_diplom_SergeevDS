using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.Player;
using Metroidvania.BaseUnit;

namespace Metroidvania.Combat.Weapon
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Animator baseAnimator;
        [SerializeField] protected WeapondData weapondData;

        protected PlayerAttackState state;

        protected int attackCounter;
        protected Unit unit;

        protected virtual void Awake()
        {
            gameObject.SetActive(false);
        }
        public virtual void EnterWeapon()
        {
            gameObject.SetActive(true);
            if (attackCounter >= weapondData.amountOfAttacks) attackCounter = 0;
            baseAnimator.SetBool("attack", true);
            baseAnimator.SetInteger("attackCounter", attackCounter);
        }

        public virtual void ExitWeapon()
        {
            baseAnimator.SetBool("attack", false);
            attackCounter++;
            gameObject.SetActive(false);
        }

        #region Animation Triggers
        public virtual void AnimationEndTrigger()
        {
            state.AnimationEndTrigger();
        }
        public virtual void AnimationStartMovementTrigger()
        {
            state.SetPlayerVelocity(weapondData.movementSpeed[attackCounter]);
        }
        public virtual void AnimationEndMovementTrigger()
        {
            state.SetPlayerVelocity(0f);
        }
        public virtual void AnimationTurnOffFlipTrigger()
        {
            state.CheckFlip(false);
        }

        public virtual void AnimationTurnOnFlipTrigger()
        {
            state.CheckFlip(true);
        }
        public virtual void AnimationActionTrigger() { }
        #endregion

        public void InitializeWeapon(PlayerAttackState state, Unit unit)
        {
            this.state = state;
            this.unit = unit;
        }
    }

}

