using Metroidvania.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.BaseUnit
{
    public class CollisionChecks : UnitComponent
    {
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private Transform headCheck;
        [SerializeField] private Transform ledgeCheckHorizontal;
        [SerializeField] private Transform ledgeCheckVertical;
        [SerializeField] private float groundCheckRadius;
        [SerializeField] private float wallCheckDistance;
        [SerializeField] private LayerMask groundLayer;
        private Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);
        private Movement _movement;
        public Transform GroundCheck
        {
            get
            {
                if (groundCheck) return groundCheck;
                Debug.LogError("No ground check on " + Unit.transform.parent.name);
                return null;
            }
            private set => groundCheck = value;
        }
        public Transform WallCheck
        {
            get => GenericCheckForNullError<Transform>.TryGet(wallCheck, transform.parent.name);
            private set => wallCheck = value;
        }
        public Transform HeadCheck
        {
            get => GenericCheckForNullError<Transform>.TryGet(headCheck, transform.parent.name);
            private set => headCheck = value;
        }
        public Transform LedgeCheckHorizontal
        {
            get => GenericCheckForNullError<Transform>.TryGet(ledgeCheckHorizontal, transform.parent.name);
            private set => ledgeCheckHorizontal = value;
        }
        public Transform LedgeCheckVertical
        {
            get => GenericCheckForNullError<Transform>.TryGet(ledgeCheckVertical, transform.parent.name);
            set => ledgeCheckVertical = value;
        }
        public float GroundCheckRadius { get => groundCheckRadius; private set => groundCheckRadius = value; }
        public float WallCheckDistance { get => wallCheckDistance; private set => wallCheckDistance = value; }
        public LayerMask GroundLayer { get => groundLayer; private set => groundLayer = value; }

        #region Check Func
        public bool Grounded => Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, groundLayer);
        public bool WallFront => Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, groundLayer);
        public bool LedgeHorizontal => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, groundLayer);
        public bool WallBack => Physics2D.Raycast(WallCheck.position, Vector2.right * -Movement.FacingDirection, wallCheckDistance, groundLayer);
        public bool Ceiling => Physics2D.OverlapCircle(HeadCheck.position, groundCheckRadius, groundLayer);
        public bool LedgeVertical => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, wallCheckDistance, groundLayer);

        #endregion
    }
}

