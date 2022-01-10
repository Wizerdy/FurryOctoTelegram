using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class Enemy : Entity {
    [Header("Enemy")]
    public int score;

    protected override void OnStart() { }

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
                GameManager.instance.OrganExplosion(transform, 3);
                Dead();
                GameManager.instance.AddScore(score);
                GameManager.instance.cameraManager.OnEnemyDestroyed();
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
