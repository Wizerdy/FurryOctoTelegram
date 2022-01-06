using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class Enemy : Entity {
    [Header("Enemy")]
    [SerializeField] private float moveSpeed = 0.2f;
    [SerializeField] private float moveDelta = 0.2f;
    public int score;

    private float moveTimer = 0.0f;
    private float dontMoveTime = 0.0f;

    protected override void OnStart() {
        if (moveSpeed <= 0) { moveSpeed = 0.0001f; }

        moveTimer = 1.0f / moveSpeed;
        dontMoveTime = Random.Range(0, moveDelta);
    }

    protected override void OnUpdate() { }

    protected override void OnMove(Vector2 direction) {
        if (rb.position.x < GameManager.instance.leftSide.position.x) {
            EnemyManager.instance.ChangeDirection(EnemyManager.Side.RIGHT);
        } else if (rb.position.x > GameManager.instance.rightSide.position.x) {
            EnemyManager.instance.ChangeDirection(EnemyManager.Side.LEFT);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null) {
            if (bullet.side != side) {
                Dead();
                GameManager.instance.AddScore(score);
            }
            return;
        }
        if (collision.gameObject.CompareTag("Player")) {
            // YOU LOSE
            Debug.Break();
        }
    }

    private void OnDestroy() {
        EnemyManager.instance.enemyList.Remove(EnemyManager.instance.GetEnemy(this));
    }
}
