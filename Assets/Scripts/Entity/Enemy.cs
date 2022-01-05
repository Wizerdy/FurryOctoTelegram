using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class Enemy : Entity {
    [Header("Enemy")]
    [SerializeField] private float moveSpeed = 0.2f;
    [SerializeField] private float moveDelta = 0.2f;

    private float moveTimer = 0.0f;
    private float dontMoveTime = 0.0f;

    protected override void OnStart() {
        if (moveSpeed <= 0) { moveSpeed = 0.0001f; }

        moveTimer = 1.0f / moveSpeed;
        dontMoveTime = Random.Range(0, moveDelta);
    }

    protected override void OnUpdate() {
        if (rb.position.x < GameManager.instance.leftSide.position.x) {
            GameManager.instance.ChangeEnemyDirection(1);
        } else if (rb.position.x > GameManager.instance.rightSide.position.x) {
            GameManager.instance.ChangeEnemyDirection(-1);
        }

        AutoMove();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null) {
            if (bullet.side != side) {
                Dead();
            }
        }
    }

    private void AutoMove() {
        if (moveTimer > 0.0f) {
            moveTimer -= Time.deltaTime;
            if (moveTimer - dontMoveTime <= 0.0f) {
                MoveTo(GameManager.instance.enemyDirection);
            }

            if (moveTimer < 0.0f) {
                if (moveSpeed <= 0) { moveSpeed = 0.00001f; }
                moveTimer = 1.0f / moveSpeed;
                dontMoveTime = Random.Range(0, moveDelta);
            }
        }
    }
}
