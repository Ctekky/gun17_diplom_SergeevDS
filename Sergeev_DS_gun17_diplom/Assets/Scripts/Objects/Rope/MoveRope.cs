using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRope : MonoBehaviour
{
    #region Fields
    private Rigidbody2D rb2d;

    // Declare the parameters responsible for the rope movement
    public float moveSpeed = 150;
    public float leftAngle = -0.3f;
    public float rightAngle = 0.3f;
    private bool movingClockwise = true;
    #endregion

    #region Unity Methods
    void Start()
    {
        rb2d = GetComponentInChildren<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Move();
    }
    #endregion

    #region Custom Methods
    public void ChangeMoveDir()
    {
        if (transform.GetChild(0).rotation.z >= rightAngle)
        {
            movingClockwise = false;
        }
        else if (transform.GetChild(0).rotation.z <= leftAngle)
        {
            movingClockwise = true;
        }
    }
    public void Move()
    {
        ChangeMoveDir();
        if (movingClockwise)
        {
            rb2d.angularVelocity = moveSpeed * 50 * Time.fixedDeltaTime;
        }
        else if (!movingClockwise)
        {
            rb2d.angularVelocity = -1 * moveSpeed * 50 * Time.fixedDeltaTime;
        }
    }
    #endregion
}
