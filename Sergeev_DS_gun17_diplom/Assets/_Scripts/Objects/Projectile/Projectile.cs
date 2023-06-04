using Metroidvania.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Metroidvania.Combat.Projectile
{
    public class Projectile : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private float _startPosX;
        private bool _isGravityOn;
        private bool _isHitGround;
        private float _landTime;
        private int _finalDamage;
        private IObjectPool<Projectile> _pool;
        [SerializeField] private float gravity;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private ProjectileData projectileData;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            _startPosX = transform.position.x;
            _rb.gravityScale = 0f;
            _isGravityOn = false;
        }
        public void SetPool(IObjectPool<Projectile> pool)
        {
            _pool = pool;
        }
        public void SetupProjectile(int damage)
        {
            _rb.velocity = transform.right * projectileData.speed;
            _finalDamage = projectileData.damage.GetValue() + damage;
        }

        public void SetupProjectile(Vector2 direction, int damage)
        {
            _rb.velocity = direction;
            _rb.gravityScale = gravity;
            _finalDamage = projectileData.damage.GetValue() + damage;

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if( collision.gameObject.layer == (int)Mathf.Log(groundLayer.value, 2))
            {
                _isHitGround = true;
                _landTime = Time.time;
            }
            var damageable = collision.GetComponentInParent<IDamageable>();
            if (damageable == null || _isHitGround) return;
            damageable.Damage(_finalDamage);
            Destroy(transform.gameObject);
        }
        private void Update()
        {
            if(!_isHitGround && _isGravityOn)
            {
                var velocity = _rb.velocity;
                var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            if(_isHitGround && Time.time >= _landTime + projectileData.lifeTime)
            {
                if (projectileData.isRopeArrow)
                {
                    var transform1 = transform;
                    Instantiate(projectileData.arrowRopePrefab, transform1.position, transform1.rotation);
                }
                Destroy(transform.gameObject);
            }
        }
        private void FixedUpdate()
        {   if(_isHitGround)
            {
                _rb.gravityScale = 0f;
                _rb.velocity = Vector2.zero;

            }
            if(Mathf.Abs(_startPosX - transform.position.x) >= projectileData.travelDistance && !_isGravityOn)
            {
                _isGravityOn = true;
                _rb.gravityScale = gravity;
            }
        }
    }
}

