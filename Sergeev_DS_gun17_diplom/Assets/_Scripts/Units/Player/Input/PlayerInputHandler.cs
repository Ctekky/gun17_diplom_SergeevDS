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
        public bool SecondaryAttackStarted { get; private set; }


        [SerializeField] private float inputHoldTime = 0.2f;
        private float _jumpInputStartTime;

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
                AttackInputs[(int)CombatInputs.Primary] = true;
            }
            if (context.canceled)
            {
                AttackInputs[(int)CombatInputs.Primary] = false;
            }
        }
        public void OnSecondaryAttackInput(InputAction.CallbackContext context)
        {
            if( context.started)
            {
                SecondaryAttackStarted = true;
            }
            if (context.performed)
            {
                SecondaryAttackStarted = false;
                AttackInputs[(int)CombatInputs.Secondary] = true;
            }
            if(context.canceled)
            {
                SecondaryAttackStarted = false;
                AttackInputs[(int)CombatInputs.Secondary] = false;
            }    
        }
        public void OnChangeWeapon(InputAction.CallbackContext context)
        {
            if(context.performed )
            {
                ChangeWeaponInput = true;
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
                _jumpInputStartTime = Time.time;
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
        public void UseChangeWeaponInput() => ChangeWeaponInput = false;
        public void UseSecondaryAttackInput() => AttackInputs[(int)CombatInputs.Secondary] = false;
        public void UseSecondaryAttackPerfomedInput() => SecondaryAttackStarted = false;
        private void CheckJumpInputHoldTime()
        {
            if(Time.time >= _jumpInputStartTime + inputHoldTime)
            {
                JumpInput = false;
            }
        }
    }

}
