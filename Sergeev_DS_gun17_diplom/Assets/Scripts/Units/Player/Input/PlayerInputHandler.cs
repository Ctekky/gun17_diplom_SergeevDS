using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Metroidvania;

namespace Metroidvania.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 RawMovementInput { get; private set; }
        public int NormalizedInputX { get; private set; }
        public int NormalizedInputY { get; private set; }
        public bool JumpInput { get; private set; }
        public bool JumpInputStop { get; private set; }
        public bool InteractInput { get; private set; }
        public bool RollInput { get; private set; }
        public bool[] AttackInputs { get; private set; }
        public bool ChangeWeaponInput { get; private set; }


        [SerializeField]
        private float inputHoldTime = 0.2f;
        private float jumpInputStartTime;

        private void Start()
        {
            int count = Enum.GetValues(typeof(CombatInputs)).Length;
            AttackInputs = new bool[count];
        }
        private void Update()
        {
            CheckJumpInputHoldTime();
        }

        public void OnPrimaryAttackInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                AttackInputs[(int)CombatInputs.primary] = true;
            }
            if (context.canceled)
            {
                AttackInputs[(int)CombatInputs.primary] = false;
            }
        }
        public void OnSecondaryAttackInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                AttackInputs[(int)CombatInputs.secondary] = true;
            }
            if (context.canceled)
            {
                AttackInputs[(int)CombatInputs.secondary] = false;
            }
        }
        public void OnChangeWeapon(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                ChangeWeaponInput = true;
            }
            if (context.canceled)
            {
                ChangeWeaponInput = false;
            }
        }
        public void OnMoveInput(InputAction.CallbackContext context)
        {
            RawMovementInput = context.ReadValue<Vector2>();
            NormalizedInputX = Mathf.RoundToInt(RawMovementInput.x);
            NormalizedInputY = Mathf.RoundToInt(RawMovementInput.y);
        }

        public void OnJumpInput(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                JumpInputStop = false;
                JumpInput = true;
                jumpInputStartTime = Time.time;
            }
            if(context.canceled)
            {
                JumpInputStop = true;
            }
        }
        public void OnInteractionInput(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                InteractInput = true;
            }
            if(context.canceled)
            {
                InteractInput = false;
            }
        }
        public void OnRollInput(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                RollInput = true;
            }
            if(context.canceled)
            {
                RollInput = false;
            }
        }
        public void UseJumpInput() => JumpInput = false;
        public void UseRollInput() => RollInput = false;
        private void CheckJumpInputHoldTime()
        {
            if(Time.time >= jumpInputStartTime + inputHoldTime)
            {
                JumpInput = false;
            }
        }
    }

}
