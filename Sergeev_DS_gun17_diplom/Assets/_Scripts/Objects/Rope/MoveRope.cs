using UnityEngine;

public class MoveRope : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    
    public float moveSpeed = 150;
    public float leftAngle = -0.3f;
    public float rightAngle = 0.3f;
    private bool _movingClockwise = true;
    
    void Start()
    {
        _rb2d = GetComponentInChildren<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Move();
    }
    
    public void ChangeMoveDir()
    {
        if (transform.GetChild(0).rotation.z >= rightAngle)
        {
            _movingClockwise = false;
        }
        else if (transform.GetChild(0).rotation.z <= leftAngle)
        {
            _movingClockwise = true;
        }
    }
    public void Move()
    {
        ChangeMoveDir();
        if (_movingClockwise)
        {
            _rb2d.angularVelocity = moveSpeed * 50 * Time.fixedDeltaTime;
        }
        else if (!_movingClockwise)
        {
            _rb2d.angularVelocity = -1 * moveSpeed * 50 * Time.fixedDeltaTime;
        }
    }
}
