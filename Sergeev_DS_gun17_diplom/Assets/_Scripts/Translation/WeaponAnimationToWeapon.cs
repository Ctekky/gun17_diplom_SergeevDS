using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Combat.Weapon
{
    public class WeaponAnimationToWeapon : MonoBehaviour
    {
        [SerializeField]
        private Weapon weapon;

        private void AnimatonEndTrigger()
        {
            weapon.AnimationEndTrigger();
        }
        private void AnimationStartMovementTrigger()
        {
            weapon.AnimationStartMovementTrigger();
        }
        private void AnimationEndMovementTrigger()
        {
            weapon.AnimationEndMovementTrigger();
        }
        private void AnimationTurnOffFlipTrigger()
        {
            weapon.AnimationTurnOffFlipTrigger();
        }
        private void AnimationTurnOnFlipTrigger()
        {
            weapon.AnimationTurnOnFlipTrigger();
        }
        private void AnimationActionTrigger()
        {
            weapon.AnimationActionTrigger();
        }
    }
}


