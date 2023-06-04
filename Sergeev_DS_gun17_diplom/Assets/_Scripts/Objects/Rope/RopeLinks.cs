using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.Player;

public class RopeLinks : MonoBehaviour
{
    #region Variables

    LastRope _lastRope;
    float _ropeLinksMass;
    float _ropeGravityScale;
    Rigidbody2D _rb2d;
    bool _movingRope = false;
    Vector2 _originalPosition;
    int _numberOfTransformsPerSegment;
    int _transformIndexToAttachToInEachSegment;
    Vector2 _contactPoint;
    float _distance;
    float _closestDistance;
    bool _optimizations;
    bool _enableRopeCollisions;
    bool _staticRope;
    [SerializeField] private Transform connectionPoint;
    public Transform ConnectionPoint
    {
        get => connectionPoint;
        private set => connectionPoint = value;
    }
    #endregion

    private void Start()
    {        
        _rb2d = GetComponent<Rigidbody2D>();
        _originalPosition = transform.position;
        GetComponent<HingeJoint2D>().anchor = new Vector2(0, -transform.parent.GetComponentInChildren<SpriteRenderer>().sprite.bounds.extents.y);
        if (transform.GetSiblingIndex() == 0)
        {
            GetComponents<HingeJoint2D>()[1].anchor = new Vector2(0, GetComponent<SpriteRenderer>().sprite.bounds.extents.y);
        }
        else
        {
            GetComponent<CapsuleCollider2D>().size = transform.parent.GetComponentInChildren<CapsuleCollider2D>().size;
            GetComponent<CapsuleCollider2D>().offset = transform.parent.GetComponentInChildren<CapsuleCollider2D>().offset;
        }
        AdjustRopeSettings();
        CheckForMovingRope();
    }

    private void Update()
    {
        if (_optimizations) return;
        CheckForMovingRope();
    }
    private void CheckForMovingRope()
    {
        if (transform.parent.TryGetComponent<MoveRope>(out MoveRope moveRope))
        {
            if (moveRope.enabled == true)
            {
                _rb2d.mass = 1;
                _rb2d.gravityScale = 0;
                if (!_movingRope)
                {
                    _movingRope = true;
                    transform.rotation = Quaternion.identity;
                    transform.position = _originalPosition;
                }
            }
            else
            {
                _movingRope = false;
                _rb2d.mass = _ropeLinksMass;
                _rb2d.gravityScale = _ropeGravityScale;
            }
        }
    }
    public void EnableMoveScript(Vector2 force)
    {
        _rb2d.AddForce(force, ForceMode2D.Impulse);
    }
    private void AdjustRopeSettings()
    {
        if (transform.parent.parent.TryGetComponent<CreateRope>(out CreateRope createRope))
        {
            _ropeLinksMass = createRope.ropeLinksMass;
            _ropeGravityScale = createRope.ropeLinksGravityScale;
            _enableRopeCollisions = createRope.enableRopeCollisions;
        }
        if (_enableRopeCollisions)
        {
            gameObject.layer = LayerMask.NameToLayer("Rope");
            Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
            GetComponent<CapsuleCollider2D>().isTrigger = false;
        }
        if(_staticRope)
        {
            _rb2d.bodyType = RigidbodyType2D.Static;
        }
    }
}
