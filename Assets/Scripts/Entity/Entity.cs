using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    [SerializeField] protected int side;

    [Header("Attack")]
    [SerializeField] protected Bullet bullet;
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float bulletSpeed = 1f;
    [SerializeField] protected float attackSpeed = 1f;
    protected float attackCooldown = 0f;
    protected Rigidbody2D rb = null;

    #region Properties
    public int Side { get { return side; } }
    #endregion

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        OnStart();
    }

    void Update() {
        if (attackCooldown > 0f) {
            attackCooldown -= Time.deltaTime;
        }

        OnUpdate();
    }

    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnDead() { }

    public void Dead() {
        OnDead();
        Destroy(gameObject);
    }

    public void MoveTo(Vector2 direction) {
        rb.position += direction * speed * Time.deltaTime;
    }

    public void Attack() {
        if (bullet == null || attackCooldown > 0f) { return; }
        if (attackSpeed > 0f) { attackCooldown = 1f / attackSpeed; }

        Bullet lastBullet = Instantiate(bullet, transform.position, transform.rotation);
        lastBullet.side = side;
        lastBullet.bulletSpeed = bulletSpeed;
    }
}
