using System;
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
    [SerializeField] protected Transform attackPoint;
    protected float attackCooldown = 0f;
    protected Rigidbody2D rb = null;

    [Header("Movement")]
    protected Nullable<Vector2> destination;
    [HideInInspector] public float speedFactor = 1f;
    protected Animator animator = null;

    #region Properties
    public int Side { get { return side; } }
    public bool CanAttack { get { return !(attackCooldown > 0.0f); } }
    #endregion

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        if (attackPoint == null) {
            attackPoint = transform;
        }

        if (animator == null) { animator = GetComponent<Animator>(); }
        OnStart();
    }

    void Update() {
        if (attackCooldown > 0f) {
            attackCooldown -= Time.deltaTime;
        }
        OnUpdate();
    }

    private void FixedUpdate() {
        UpdateMove();
    }

    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnDead() { }
    protected virtual void OnAttack() { }

    public void Dead() {
        OnDead();
        Destroy(gameObject);
    }

    public virtual void MoveTo(Vector2 direction) {
        //rb.position += direction * speed * Time.deltaTime;
        Vector2 startPos;
        if (destination == null || destination.Value == Vector2.zero) {
            startPos = rb.position;
        } else {
            startPos = destination.Value;
        }

        destination = startPos + direction;
        OnMove(direction);
    }

    protected virtual void UpdateMove() {
        if (destination != null) {
            Vector2 direction = (destination.Value - (Vector2)transform.position).normalized;
            rb.position += direction * speed * speedFactor * Time.deltaTime;
            if (Vector2.Distance(destination.Value, rb.position) < speed * speedFactor * Time.deltaTime) {
                rb.position = destination.Value;
                destination = null;
            }
        }
    }

    protected virtual void OnMove(Vector2 direction) { }

    public void Attack() {
        if (bullets == null || bullets.Count <= 0 || attackCooldown > 0f) { return; }
        if (attackSpeed > 0f) { attackCooldown = 1f / attackSpeed; }
        if (side == 2)
        {
            SoundManager.i.Play("EnemyThrow");
        } else
        {
            SoundManager.i.Play("PlayerThrow");
        }

        int bulletIndex = UnityEngine.Random.Range(0, bullets.Count - 1);
        //Quaternion rotation = Quaternion.LookRotation(bulletDirection, Vector3.up);
        Quaternion rotation = Quaternion.LookRotation(transform.forward, bulletDirection);
        Bullet lastBullet = Instantiate(bullets[bulletIndex], attackPoint.position, rotation);
        lastBullet.side = side;
        lastBullet.bulletSpeed = bulletSpeed;

        OnAttack();
    }
}
