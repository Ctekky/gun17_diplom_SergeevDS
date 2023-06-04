using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Metroidvania.BaseUnit
{
    public class Movement : UnitComponent
    {
        public Action onFlipped;
        private Rigidbody2D Rb { get; set; }
        public int FacingDirection { get; private set; }
        private Vector2 _workVector;
        public Vector2 CurrentVelocity { get; private set; }
        public bool CanSetVelocity { get; set; }

        protected override void Awake()
        {
            base.Awake();
            Rb = GetComponentInParent<Rigidbody2D>();
            if (Rb == null) Debug.LogError("There is no Rigidbody2D in parents!");
            FacingDirection = 1;
            CanSetVelocity = true;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            CurrentVelocity = Rb.velocity;
        }
        #region Set Func
        public void SetVelocityX(float velocity)
        {
            _workVector.Set(velocity, CurrentVelocity.y);
            SetFinalVelocity();
        }
        public void SetVelocityY(float velocity)
        {
            _workVector.Set(CurrentVelocity.x, velocity);
            SetFinalVelocity();
        }
        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            _workVector.Set(angle.x * velocity * direction, angle.y * velocity);
            SetFinalVelocity();
        }
        public void SetVelocity(float velocity, Vector2 direction)
        {
            _workVector = direction * velocity;
            SetFinalVelocity();
        }
        public void SetVelocityZero()
        {
            _workVector = Vector2.zero;
            SetFinalVelocity();
        }
        private void SetFinalVelocity()
        {
            if(CanSetVelocity)
            {
                Rb.velocity = _workVector;
                CurrentVelocity = _workVector;
            }
        }
        #endregion
        public void CheckFlip(int inputX)
        {
            if (inputX != 0 && inputX != FacingDirection) Flip();
        }
        public void Flip()
        {
            FacingDirection *= -1;
            Rb.transform.Rotate(0f, 180f, 0f);
            onFlipped?.Invoke();
        }
    }

}

