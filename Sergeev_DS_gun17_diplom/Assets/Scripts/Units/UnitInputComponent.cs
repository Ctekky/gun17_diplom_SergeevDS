using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace Metroidvania.Unit
{
    public class UnitInputComponent : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D _rigidbody;
        protected Vector2 _movement;       
        public ref Vector2 MoveDirection => ref _movement;
        private bool _isFacingRight = true;
        private Dictionary<string, FieldInfo> _events;

        [Header("Проверка нахождения на земле")]
        [SerializeField] private bool _isOnGround;
        [SerializeField, Range(-5f, 5f)] private float _checkGroundOffsetY = -1.8f;
        [SerializeField, Range(0, 5f)] private float _checkGroundRadius = 0.3f;

        public bool GetSetOnGround { get => _isOnGround; set { _isOnGround = value; } }

        public EventHandle JumpEventHandler;
        protected void CallOnJumpEvent() => JumpEventHandler?.Invoke();

        /*
        protected virtual void Awake()
        {
            _events = new Dictionary<string, FieldInfo>();
            var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public).Where(t => t.FieldType == typeof(EventHandle));
            foreach (var field in fields)
                _events.Add(field.Name, field);
        }
        protected void CallEventHandler(string name)
        {
            var field = _events[name];
            var fieldExp = Expression.Field(Expression.Constant(this), field);
            var delegates = Expression.Convert(fieldExp, typeof(MulticastDelegate));
            var methodInfo = typeof(MulticastDelegate).GetMethod(nameof(MulticastDelegate.GetInvocationList));
            var getInvocExpr = Expression.Call(delegates, methodInfo);
            var arrayExpr = Expression.Convert(getInvocExpr, typeof(Delegate[]));
            var result = Expression.Lambda<Func<FieldInfo, Delegate[]>>(arrayExpr, Expression.Parameter(typeof(FieldInfo))).Compile();
            foreach(var _event in result.Invoke(_events[name]))
            {
                _event.Method.Invoke(_event.Target, null);
            }
        }
        */
        public void UnitMovement(float direction, float speed)
        {
            var targetVelocity = new Vector2(direction * speed, _rigidbody.velocity.y);
            _rigidbody.velocity = targetVelocity;
            if (direction < 0 && _isFacingRight)
            {
                UninChangeDirection();
            }
            else if( direction > 0 && !_isFacingRight)
            {
                UninChangeDirection();
            }
        }
        protected void UninChangeDirection()
        {
            _isFacingRight = !_isFacingRight;
            Vector3 temporaryScale = transform.localScale;
            temporaryScale.x *= -1;
            transform.localScale = temporaryScale;
        }
        protected void UnitJump(float jumpForce)
        {
            if(_isOnGround)
                _rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + _checkGroundOffsetY), _checkGroundRadius);
            if (colliders.Length > 1)
            {
                _isOnGround = true;
            }
            else
            {
                _isOnGround = false;
            }
        }

        protected virtual void Update()
        {
            CheckGround();
        }
    }

}

