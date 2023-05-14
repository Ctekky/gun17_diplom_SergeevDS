using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.Player;

public class RopeLinks : MonoBehaviour
{
    #region Variables

    LastRope lastRope;
    float ropeLinksMass;
    float ropeGravityScale;
    Rigidbody2D rb2d;
    bool movingRope = false;
    Vector2 originalPosition;
    int numberOfTransformsPerSegment;
    int transformIndexToAttachToInEachSegment;
    Vector2 contactPoint;
    float distance;
    float closestDistance;
    bool optimizations;
    bool enableRopeCollisions;
    bool staticRope;
    #endregion

    private void Start()
    {        
        rb2d = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
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
        if (optimizations) return;
        CheckForMovingRope();
    }
    private void CheckForMovingRope()
    {
        if (transform.parent.TryGetComponent<MoveRope>(out MoveRope moveRope))
        {
            if (moveRope.enabled == true)
            {
                rb2d.mass = 1;
                rb2d.gravityScale = 0;
                if (!movingRope)
                {
                    movingRope = true;
                    transform.rotation = Quaternion.identity;
                    transform.position = originalPosition;
                }
            }
            else
            {
                movingRope = false;
                rb2d.mass = ropeLinksMass;
                rb2d.gravityScale = ropeGravityScale;
            }
        }
    }
    public void EnableMoveScript(Vector2 force)
    {
        rb2d.AddForce(force, ForceMode2D.Impulse);
    }
    private void AdjustRopeSettings()
    {
        if (transform.parent.parent.TryGetComponent<CreateRope>(out CreateRope createRope))
        {
            ropeLinksMass = createRope.ropeLinksMass;
            ropeGravityScale = createRope.ropeLinksGravityScale;
            enableRopeCollisions = createRope.enableRopeCollisions;
        }
        if (enableRopeCollisions)
        {
            gameObject.layer = LayerMask.NameToLayer("Rope");
            Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
            GetComponent<CapsuleCollider2D>().isTrigger = false;
        }
        if(staticRope)
        {
            rb2d.bodyType = RigidbodyType2D.Static;
        }
    }
}
