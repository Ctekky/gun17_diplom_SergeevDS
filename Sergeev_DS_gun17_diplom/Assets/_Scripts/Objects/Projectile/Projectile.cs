using Metroidvania.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Metroidvania.Combat.Projectile
{
    public class Projectile : MonoBehaviour
    {
        private Rigidbody2D rb;
        private float startPosX;
        private bool isGravityOn;
        private bool isHitGround;
        private float landTime;
        private IObjectPool<Projectile> pool;
        [SerializeField] private float gravity;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private ProjectileData projectileData;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            startPosX = transform.position.x;
            rb.gravityScale = 0f;
            isGravityOn = false;
        }
        public void SetPool(IObjectPool<Projectile> pool)
        {
            this.pool = pool;
        }
        public void SetupProjectile()
        {
            rb.velocity = transform.right * projectileData.speed;
        }

        public void SetupProjectile(Vector2 direction)
        {
            rb.velocity = direction;
            rb.gravityScale = gravity;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if( collision.gameObject.layer == (int)Mathf.Log(groundLayer.value, 2))
            {
                isHitGround = true;
                landTime = Time.time;
            }
            IDamageable damageable = collision.GetComponentInParent<IDamageable>();
            if (damageable != null && !isHitGround)
            {
                damageable.Damage(projectileData.damage);
                Destroy(transform.gameObject);
            }
        }
        private void Update()
        {
            if(!isHitGround && isGravityOn)
            {
                float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            if(isHitGround && Time.time >= landTime + projectileData.lifeTime)
            {
                if (projectileData.isRopeArrow)
                {
                    Instantiate(projectileData.arrowRopePrefab, transform.position, transform.rotation);
                }
                Destroy(transform.gameObject);
            }
        }
        private void FixedUpdate()
        {   if(isHitGround)
            {
                rb.gravityScale = 0f;
                rb.velocity = Vector2.zero;

            }
            if(Mathf.Abs(startPosX - transform.position.x) >= projectileData.travelDistance && !isGravityOn)
            {
                isGravityOn = true;
                rb.gravityScale = gravity;
            }
        }
    }
}


