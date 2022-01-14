using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class Enemy : Entity {
    [Header("Enemy")]
    public int score;

    public GameObject[] corpse;
    public System.Nullable<Vector2> place;
    public bool inPlace = false;

    private Coroutine rtn_delayMoveTo = null;

    protected override void OnStart() {
        if (place != null) {
            inPlace = false;
            SetDestination(place.Value, 0.2f);
        }
    }

    protected override void OnUpdate() { }

    public void SetPlace(Vector2 position) {
        place = position;
    }

    public void SetDestination(Vector2 position, float maxDeltaTime) {
        //if (!GameManager.effects[9]) { rb.position = position; OnArrive(); return; }

        if (rtn_delayMoveTo != null) { StopCoroutine(rtn_delayMoveTo); }
        float time = Random.Range(0, maxDeltaTime);
        rtn_delayMoveTo = StartCoroutine(Tools.Delay(SetDestination, position, time));
    }

    public void SetDestination(Vector2 position) {
        destination = position;
    }

    public void MoveTo(Vector2 direction, float maxDeltaTime) {
        if (!GameManager.effects[9]) { rb.position += direction; OnArrive(); return; }

        if (rtn_delayMoveTo != null) { StopCoroutine(rtn_delayMoveTo); }
        float time = Random.Range(0, maxDeltaTime);
        rtn_delayMoveTo = StartCoroutine(Tools.Delay(MoveTo, direction, time));
    }

    protected override void OnSetMove(Vector2 direction) {
        if (rb.position.x < GameManager.instance.leftSide.position.x) {
            EnemyManager.instance.ChangeDirection(EnemyManager.Side.RIGHT);
        } else if (rb.position.x > GameManager.instance.rightSide.position.x) {
            EnemyManager.instance.ChangeDirection(EnemyManager.Side.LEFT);
        }
    }

    protected override void OnArrive() {
        if (!inPlace) { inPlace = true; }

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
            }
            return;
        }
        if (collision.gameObject.CompareTag("Player")) {
            GameManager.instance.GameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            GameManager.instance.GameOver();
        }
    }

    public void CorpseExplosion()
    {
        for (int i = 0; i < corpse.Length; i++)
        {
            float randX = Random.Range(-1f, 1f);
            float randY = Random.Range(-1f, 1f);
            Vector3 randomVector3 = new Vector3(transform.position.x + randX, transform.position.y + randY, transform.position.z);
            GameObject trash = Instantiate(corpse[i], randomVector3, transform.rotation, GameManager.instance.transform);
            //trash.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f)), ForceMode2D.Impulse);
            GameManager.instance.backgroundManager.AddToRoad(trash);
        }
    }

    protected override void OnDead() {
        try {
            GameManager.instance.enemyManager.OnEnemyDeath(this);
            GameManager.instance.AddScore(score);

            if (GameManager.effects[0]) {
                GameManager.instance.OrganExplosion(transform, 1);
                GameManager.instance.BloodExplosion(transform);
                CorpseExplosion();
                int rand = Random.Range(0, 2);
                if (rand % 2 == 0) {
                    SoundManager.i.Play("IceCrack");
                } else {
                    SoundManager.i.Play("BoneCrack");
                }
                GameManager.instance.cameraManager.OnEnemyDestroyed();
            }
        } catch(System.Exception e) {
            Debug.LogException(e);
        }
    }

    protected override void OnAttack() {
        base.OnAttack();
        if (animator != null) {
            animator.SetTrigger("AttackEnemy");
        }
    }

    private void OnDestroy() {
        EnemyManager.instance.enemyList.Remove(EnemyManager.instance.GetEnemy(this));
    }

    private void OnDrawGizmos() {
        if (destination != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(destination.Value, 0.1f);
        } else {
            Gizmos.color = Color.green;
            //Gizmos.DrawSphere(rb.position, 0.1f);
        }
    }

    public void ToggleAnimator(bool state) {
        GetComponent<Animator>().enabled = state;
        transform.GetChild(0).GetComponent<Animator>().enabled = state;
    }
}
