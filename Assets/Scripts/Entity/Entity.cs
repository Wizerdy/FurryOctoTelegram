using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    [SerializeField] protected int side;

    [Header("Attack")]
    [SerializeField] protected List<Bullet> bullets;
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float bulletSpeed = 1f;
    [SerializeField] protected Vector2 bulletDirection = Vector2.up;
    [SerializeField] protected float attackSpeed = 1f;
    protected float attackCooldown = 0f;
    protected Rigidbody2D rb = null;

    #region Properties
    public int Side { get { return side; } }
    public bool CanAttack { get { return !(attackCooldown > 0.0f); } }
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
        OnMove(direction);
    }

    protected virtual void OnMove(Vector2 direction) { }

    public void Attack() {
        if (bullets == null || bullets.Count <= 0 || attackCooldown > 0f) { return; }
        if (attackSpeed > 0f) { attackCooldown = 1f / attackSpeed; }

        int bulletIndex = Random.Range(0, bullets.Count - 1);
        //Quaternion rotation = Quaternion.LookRotation(bulletDirection, Vector3.up);
        Quaternion rotation = Quaternion.LookRotation(transform.forward, bulletDirection);
        Bullet lastBullet = Instantiate(bullets[bulletIndex], transform.position, rotation);
        lastBullet.side = side;
        lastBullet.bulletSpeed = bulletSpeed;
    }
}
