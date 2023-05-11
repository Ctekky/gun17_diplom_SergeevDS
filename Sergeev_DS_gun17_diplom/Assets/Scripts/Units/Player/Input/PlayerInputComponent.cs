using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Unit.Player
{
    public class PlayerInputComponent : UnitInputComponent
    {
        private Animator _animator;
        private PlayerInputActions _playerControls;
        private UnitStatsComponent _unitStats;
        private void Awake()
        {
            _unitStats = GetComponent<UnitStatsComponent>();
            _playerControls = new PlayerInputActions();
            _playerControls.Gameplay.Jump.performed += OnJump;
        }

        private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            CallOnJumpEvent();
            UnitJump(_unitStats.GetSetJumpForce);
        }
        private void OnEnable()
        {
            _playerControls.Gameplay.Enable();
        }
        private void OnDisable()
        {
            _playerControls.Gameplay.Jump.performed -= OnJump;
            _playerControls.Gameplay.Disable();
        }
        private void OnDestroy()
        {
            _playerControls.Dispose();
        }
        protected override void Update()
        {
            base.Update();
            var direction = _playerControls.Gameplay.Move.ReadValue<Vector2>();
            _movement = new Vector2(direction.x, direction.y);
            //UnitMovement(direction.x, _unitStats.GetSetMoveSpeed);
        }
    }
}

