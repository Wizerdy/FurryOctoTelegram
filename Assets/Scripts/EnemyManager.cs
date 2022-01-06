using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ToolsBoxEngine;

public class EnemyManager : MonoBehaviour {
    public enum Side { NONE, LEFT, RIGHT, UP, DOWN }
    public struct EnemyCell {
        public Vector2 cellPos;
        public Enemy enemy;

        public EnemyCell(Vector2 position, Enemy gameobject) {
            cellPos = position;
            enemy = gameobject;
        }
    }

    public static EnemyManager instance;

    [HideInInspector] public List<EnemyCell> enemyList;
    [HideInInspector] public List<Enemy> ufoList;
    public float enemySpeed;
    public float timeToMove = 1f;

    [Header("Attacks")]
    public float attackCooldown;
    private float attackTimer;

    private Vector2 enemyDirection = Vector2.right;
    private Vector2[] nextEnemyDirection;
    private float moveTimer;

    private void Awake() {
        instance = this;

        nextEnemyDirection = new Vector2[2];
        for (int i = 0; i < nextEnemyDirection.Length; i++) {
            nextEnemyDirection[i] = enemyDirection;
        }

        enemyList = new List<EnemyCell>();
    }

    void Start() {
        moveTimer = timeToMove;
        attackTimer = attackCooldown;
    }

    void Update() {
        if (moveTimer > 0.0f) {
            moveTimer -= Time.deltaTime;
        } else {
            MoveEnemies();
            moveTimer = timeToMove;
        }

        if (attackTimer > 0.0f) {
            attackTimer -= Time.deltaTime;
        } else {
            AttackEnemy();
            attackTimer = attackCooldown;
        }

        if (ufoList != null && ufoList.Count > 0) {
            for (int i = 0; i < ufoList.Count; i++) {
                ufoList[i].Attack();
            }
        }
    }

    private void MoveEnemies() {
        for (int i = 0; i < enemyList.Count; i++) {
            enemyList[i].enemy.MoveTo(enemyDirection * enemySpeed);
        }

        enemyDirection = nextEnemyDirection[0];
        nextEnemyDirection[0] = nextEnemyDirection[1];
    }

    private void AttackEnemy() {
        List<EnemyCell> enemies = GetEnemyOnFront();

        while (enemies.Count > 0) {
            int index = Random.Range(0, enemies.Count);

            if (enemies[index].enemy.CanAttack) {
                enemies[index].enemy.Attack();
                return;
            } else {
                enemies.RemoveAt(index);
            }
        }
    }

    public void ChangeDirection(Side side) {
        if (enemyDirection == Vector2.down) { return; }

        switch (side) {
            case Side.NONE:
                break;
            case Side.LEFT:
                nextEnemyDirection[1] = new Vector2(Tools.Negative(enemyDirection.x), enemyDirection.y);
                break;
            case Side.RIGHT:
                nextEnemyDirection[1] = new Vector2(Tools.Positive(enemyDirection.x), enemyDirection.y);
                break;
            default:
                break;
        }

        nextEnemyDirection[0] = Vector2.down;
    }

    public List<EnemyCell> GetEnemyOnFront() {
        List<EnemyCell> output = new List<EnemyCell>();

        for (int i = 0; i < enemyList.Count; i++) {
            if (InFrontLine(enemyList[i].enemy)) {
                output.Add(enemyList[i]);
            }
        }

        return output;
    }

    public bool InFrontLine(Enemy enemy) {
        EnemyCell enemyCell = GetEnemy(enemy);

        for (int i = 0; i < enemyList.Count; i++) {
            if (enemyList[i].cellPos.y == enemyCell.cellPos.y && enemyList[i].cellPos.x > enemyCell.cellPos.x) {
                return false;
            }
        }
        return true;
    }

    public EnemyCell GetEnemy(Enemy enemy) {
        EnemyCell enemyCell = enemyList.Find(x => x.enemy == enemy);
        return enemyCell;
    }
}
