using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.BaseUnit
{
    public class Movement : UnitComponent
    {
        public Rigidbody2D RB { get; private set; }
        public int FacingDirection { get; private set; }
        private Vector2 workVector;
        public Vector2 CurrentVelocity { get; private set; }
        public bool CanSetVelocity { get; set; }

        protected override void Awake()
        {
            base.Awake();
            RB = GetComponentInParent<Rigidbody2D>();
            if (RB == null) Debug.LogError("There is no Rigidbody2D in parents!");
            FacingDirection = 1;
            CanSetVelocity = true;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            CurrentVelocity = RB.velocity;
        }
        #region Set Func
        public void SetVelocityX(float velocity)
        {
            workVector.Set(velocity, CurrentVelocity.y);
            SetFinalVelocity();
        }
        public void SetVelocityY(float velocity)
        {
            workVector.Set(CurrentVelocity.x, velocity);
            SetFinalVelocity();
        }
        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            workVector.Set(angle.x * velocity * direction, angle.y * velocity);
            SetFinalVelocity();
        }
        public void SetVelocity(float velocity, Vector2 direction)
        {
            workVector = direction * velocity;
            SetFinalVelocity();
        }
        public void SetVelocityZero()
        {
            workVector = Vector2.zero;
            SetFinalVelocity();
        }
        private void SetFinalVelocity()
        {
            if(CanSetVelocity)
            {
                RB.velocity = workVector;
                CurrentVelocity = workVector;
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
            RB.transform.Rotate(0f, 180f, 0f);
        }
    }

}

