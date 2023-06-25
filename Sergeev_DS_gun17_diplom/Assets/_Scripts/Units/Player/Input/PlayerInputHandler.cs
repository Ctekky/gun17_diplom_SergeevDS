using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Metroidvania.Player
{
    public class PlayerInputHandler : MonoBehaviour, PlayerInputActions.IGameplayActions, PlayerInputActions.IUIActions
    {
        private Vector2 RawMovementInput { get; set; }
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
        private bool _isInCharMenu = false;
        private bool _isInCraftMenu = false;
        private bool _isInOptionMenu = false;

        private PlayerInputActions _playerInputActions;
        public event Action PressedCharacterUI;
        public event Action PressedCraftUI;
        public event Action PressedOptionsUI;
        public event Action ClosedMenu;
        public event Action SwitchedAmmo;
        public event Action<PotionSlotNumber> UsedPotion;
        public event Action PressedInteract;

        private void OnEnable()
        {
            if (_playerInputActions != null) return;
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Gameplay.SetCallbacks(this);
            _playerInputActions.UI.SetCallbacks(this);
        }

        private void Start()
        {
            var count = Enum.GetValues(typeof(CombatInputs)).Length;
            AttackInputs = new bool[count];
            SetGameplay();
        }

        private void Update()
        {
            CheckJumpInputHoldTime();
        }

        public void SetGameplay()
        {
            _playerInputActions.Gameplay.Enable();
            _playerInputActions.UI.Disable();
            _playerInputActions.MainMenu.Disable();
        }

        public void SetMainMenu()
        {
            _playerInputActions.Gameplay.Disable();
            _playerInputActions.UI.Disable();
            _playerInputActions.MainMenu.Enable();
        }

        public void SetUI()
        {
            _playerInputActions.Gameplay.Disable();
            _playerInputActions.UI.Enable();
            _playerInputActions.MainMenu.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            RawMovementInput = context.ReadValue<Vector2>();
            NormalizedInputX = Mathf.RoundToInt(RawMovementInput.x);
            NormalizedInputY = Mathf.RoundToInt(RawMovementInput.y);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                JumpInputStop = false;
                JumpInput = true;
                _jumpInputStartTime = Time.time;
            }

            if (context.canceled)
            {
                JumpInputStop = true;
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                InteractInput = true;
                PressedInteract?.Invoke();
            }

            if (context.canceled)
            {
                InteractInput = false;
            }
        }

        public void OnRoll(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                RollInput = true;
            }

            if (context.canceled)
            {
                RollInput = false;
            }
        }

        public void OnPrimaryAttack(InputAction.CallbackContext context)
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

        public void OnSecondaryAttack(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                SecondaryAttackStarted = true;
            }

            if (context.performed)
            {
                SecondaryAttackStarted = false;
                AttackInputs[(int)CombatInputs.Secondary] = true;
            }

            if (context.canceled)
            {
                SecondaryAttackStarted = false;
                AttackInputs[(int)CombatInputs.Secondary] = false;
            }
        }

        public void OnChangeWeapon(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ChangeWeaponInput = true;
            }
        }

        public void OnCloseMenu(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            ClosedMenu?.Invoke();
            _isInCraftMenu = false;
            _isInCharMenu = false;
            _isInOptionMenu = false;
            SetGameplay();
        }

        void PlayerInputActions.IUIActions.OnCharacterMenu(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (_isInCharMenu)
            {
                ClosedMenu?.Invoke();
                _isInCharMenu = false;
                SetGameplay();
            }
            else
            {
                PressedCharacterUI?.Invoke();
                _isInCraftMenu = false;
                _isInCharMenu = true;
                _isInOptionMenu = false;
            }
        }

        void PlayerInputActions.IUIActions.OnCraftMenu(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (_isInCraftMenu)
            {
                ClosedMenu?.Invoke();
                _isInCraftMenu = false;
                SetGameplay();
            }
            else
            {
                PressedCraftUI?.Invoke();
                _isInCraftMenu = true;
                _isInCharMenu = false;
                _isInOptionMenu = false;
            }
        }

        public void OnOptionMenu(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (_isInOptionMenu)
            {
                ClosedMenu?.Invoke();
                _isInOptionMenu = false;
                SetGameplay();
            }
            else
            {
                PressedCraftUI?.Invoke();
                _isInCraftMenu = false;
                _isInCharMenu = false;
                _isInOptionMenu = true;
            }
        }

        void PlayerInputActions.IGameplayActions.OnCharacterMenu(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            PressedCharacterUI?.Invoke();
            SetUI();
            _isInCharMenu = true;
        }

        void PlayerInputActions.IGameplayActions.OnCraftMenu(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            PressedCraftUI?.Invoke();
            SetUI();
            _isInCraftMenu = true;
        }

        public void OnOptionsMenu(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            PressedOptionsUI?.Invoke();
            SetUI();
            _isInOptionMenu = true;
        }

        public void OnSwitchAmmo(InputAction.CallbackContext context)
        {
            if (context.performed)
                SwitchedAmmo?.Invoke();
        }

        public void OnPotion_1(InputAction.CallbackContext context)
        {
            if (context.performed)
                UsedPotion?.Invoke(PotionSlotNumber.First);
        }

        public void OnPotion_2(InputAction.CallbackContext context)
        {
            if (context.performed)
                UsedPotion?.Invoke(PotionSlotNumber.Second);
        }

        public void OnPotion_3(InputAction.CallbackContext context)
        {
            if (context.performed)
                UsedPotion?.Invoke(PotionSlotNumber.Third);
        }

        public void OnPotion_4(InputAction.CallbackContext context)
        {
            if (context.performed)
                UsedPotion?.Invoke(PotionSlotNumber.Fourth);
        }

        public void UseJumpInput() => JumpInput = false;
        public void UseRollInput() => RollInput = false;
        public void UseChangeWeaponInput() => ChangeWeaponInput = false;
        public void UseSecondaryAttackInput() => AttackInputs[(int)CombatInputs.Secondary] = false;
        public void UseSecondaryAttackPerformedInput() => SecondaryAttackStarted = false;

        private void CheckJumpInputHoldTime()
        {
            if (Time.time >= _jumpInputStartTime + inputHoldTime)
            {
                JumpInput = false;
            }
        }
    }
}