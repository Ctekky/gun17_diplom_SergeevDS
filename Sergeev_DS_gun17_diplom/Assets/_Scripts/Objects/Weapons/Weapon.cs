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

        protected PlayerAttackState attackState;
        protected PlayerAbilityState abilityState;

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
            attackState.AnimationEndTrigger();
        }
        public virtual void AnimationStartMovementTrigger()
        {
            attackState.SetPlayerVelocity(weapondData.movementSpeed[attackCounter]);
        }
        public virtual void AnimationEndMovementTrigger()
        {
            attackState.SetPlayerVelocity(0f);
        }
        public virtual void AnimationTurnOffFlipTrigger()
        {
            attackState.CheckFlip(false);
        }

        public virtual void AnimationTurnOnFlipTrigger()
        {
            attackState.CheckFlip(true);
        }
        public virtual void AnimationActionTrigger() { }
        #endregion

        public void InitializeWeapon(PlayerAttackState state, Unit unit)
        {
            attackState = state;
            this.unit = unit;
        }
        public void InitializeWeapon(PlayerAbilityState state, Unit unit)
        {
            abilityState = state;
            this.unit = unit;
        }
        public virtual void EnterWeaponSecondary()
        {
            gameObject.SetActive(true);
        }
        public virtual void ExitWeaponSecondary()
        {
            gameObject.SetActive(false);
        }
    }

}

