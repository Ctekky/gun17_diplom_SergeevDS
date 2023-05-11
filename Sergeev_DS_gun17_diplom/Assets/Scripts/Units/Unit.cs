using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Metroidvania.Unit
{
    public class Unit : MonoBehaviour
    {
        [SerializeField]
        private bool _inAnimation;
        private Animator _animator;
        private UnitInputComponent _inputs;
        private UnitStatsComponent _stats;
        private ColliderComponent[] _colliders;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _inputs = GetComponent<UnitInputComponent>();
            _stats = GetComponent<UnitStatsComponent>();
            _colliders = GetComponentsInChildren<ColliderComponent>();
            if (_inputs == null) return;
            _inputs.JumpEventHandler += OnJump;
        }

        private void OnJump()
        {
            _inAnimation = true;
            _animator.SetBool("Jump", true);

        }
        private void FixedUpdate()
        {
            OnMove();
        }

        private void OnAnimationEnd_UnityEvent(AnimationEvent data)
        {
            _inAnimation = false;

        }
        private void OnJumpEndEvent_UnityEvent(AnimationEvent data)
        {
            _inAnimation = false;
            _animator.SetBool("Jump", false);
        }
        private void OnMove()
        {
            if (_inAnimation) return;
            _animator.SetFloat("Move", Mathf.Abs(_inputs.MoveDirection.x));
            _inputs.UnitMovement(_inputs.MoveDirection.x, _stats.GetSetMoveSpeed);
        }
    }

}
