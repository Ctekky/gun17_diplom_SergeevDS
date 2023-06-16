using UnityEngine;
using Metroidvania.Player;
using Metroidvania.BaseUnit;
using Zenject;

namespace Metroidvania.Combat.Weapon
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Animator baseAnimator;
        [SerializeField] protected WeapondData weaponData;
        protected UnitStats UnitStats => _unitStats ? _unitStats : Unit.GetUnitComponent<UnitStats>(ref _unitStats);
        private UnitStats _unitStats;

        private PlayerAttackState _attackState;
        protected PlayerAbilityState AbilityState;

        protected int AttackCounter;
        protected Unit Unit;
        protected Player.Player Player;

        protected virtual void Awake()
        {
            Player = GetComponentInParent<Player.Player>();
            gameObject.SetActive(false);
        }
        public virtual void EnterWeapon()
        {
            gameObject.SetActive(true);
            if (AttackCounter >= weaponData.AmountOfAttacks) AttackCounter = 0;
            baseAnimator.SetBool("attack", true);
            baseAnimator.SetInteger("attackCounter", AttackCounter);
        }
        public virtual void ExitWeapon()
        {
            baseAnimator.SetBool("attack", false);
            AttackCounter++;
            gameObject.SetActive(false);
        }

        #region Animation Triggers
        public virtual void AnimationEndTrigger()
        {
            _attackState.AnimationEndTrigger();
        }
        public virtual void AnimationStartMovementTrigger()
        {
            _attackState.SetPlayerVelocity(weaponData.MovementSpeed[AttackCounter]);
        }
        public virtual void AnimationEndMovementTrigger()
        {
            _attackState.SetPlayerVelocity(0f);
        }
        public virtual void AnimationTurnOffFlipTrigger()
        {
            _attackState.CheckFlip(false);
        }

        public virtual void AnimationTurnOnFlipTrigger()
        {
            _attackState.CheckFlip(true);
        }
        public virtual void AnimationActionTrigger() { }
        #endregion

        public void InitializeWeapon(PlayerAttackState state, Unit unit)
        {
            _attackState = state;
            Unit = unit;
        }
        public void InitializeWeapon(PlayerAbilityState state, Unit unit)
        {
            AbilityState = state;
            Unit = unit;
        }
        public virtual void EnterWeaponSecondary()
        {
            gameObject.SetActive(true);
        }

        public virtual void EnterWeaponAim()
        {
            gameObject.SetActive(true);
        }

        public virtual void ExitWeaponAim()
        {
            gameObject.SetActive(false);
        }
        public virtual void ExitWeaponSecondary()
        {
            gameObject.SetActive(false);
        }
    }

}

