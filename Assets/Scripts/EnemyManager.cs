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

    public bool updateEnemy = false;

    public float enemySpeed;
    public float timeToMove = 1f;
    public float maxEnemySpeed;
    private float minEnemySpeed = 1f;
    public AnimationCurve speedCurve = AnimationCurve.Linear(0, 0, 1, 1);
    private float speedFactor;
    private int enemyMax = 0;

    [Header("Attacks")]
    public float attackCooldown;
    private float attackTimer;

    private Vector2 enemyDirection = Vector2.right;
    private Vector2[] nextEnemyDirection;
    private float moveTimer;

    #region Properties

    public bool HasWaveEnd {
        get { return enemyList.Count <= 0; }
    }

    #endregion

    #region Unity Callbacks

    private void Awake() {
        instance = this;

        nextEnemyDirection = new Vector2[3];
        for (int i = 0; i < nextEnemyDirection.Length; i++) {
            nextEnemyDirection[i] = enemyDirection;
        }

        enemyList = new List<EnemyCell>();
    }

    void Start() {
        moveTimer = timeToMove;
        attackTimer = attackCooldown;
        enemyMax = Mathf.FloorToInt(GameManager.instance.spawnManager.cellNumbers.x * GameManager.instance.spawnManager.cellNumbers.y);
        speedFactor = 1;
    }

    void Update() {
        if (!updateEnemy) {
            for (int i = 0; i < enemyList.Count; i++) {
                if (!enemyList[i].enemy.inPlace) {
                    return;
                }
            }
            updateEnemy = true;
        }

        // Movements
        if (moveTimer > 0.0f) {
            moveTimer -= Time.deltaTime;
        } else {
            MoveEnemies();
            moveTimer = timeToMove / speedFactor;
        }

        // Attack
        if (attackTimer > 0.0f) {
            attackTimer -= Time.deltaTime;
        } else {
            AttackEnemy();
            attackTimer = attackCooldown;
        }

        // Ufos
        if (ufoList != null && ufoList.Count > 0) {
            for (int i = 0; i < ufoList.Count; i++) {
                ufoList[i].Attack();
            }
        }
    }

    #endregion

    private void MoveEnemies() {
        for (int i = 0; i < enemyList.Count; i++) {
            enemyList[i].enemy.MoveTo(nextEnemyDirection[0] * enemySpeed, timeToMove / speedFactor / 2f);
        }

        nextEnemyDirection[0] = nextEnemyDirection[1];
        nextEnemyDirection[1] = nextEnemyDirection[2];
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
        Vector2 directionToGo = Vector2.zero;

        switch (side) {
            case Side.LEFT:
                directionToGo = new Vector2(Tools.Negative(enemyDirection.x), enemyDirection.y);
                break;
            case Side.RIGHT:
                directionToGo = new Vector2(Tools.Positive(enemyDirection.x), enemyDirection.y);
                break;
        }

        if (nextEnemyDirection[1] == directionToGo || nextEnemyDirection[2] == directionToGo) {
            return;
        }

        nextEnemyDirection[2] = directionToGo;
        nextEnemyDirection[1] = Vector2.down;
    }

    public void OnEnemyDeath(Enemy enemy) {
        enemyList.Remove(GetEnemy(enemy));
        float perc = Mathf.InverseLerp(0, enemyMax, enemyList.Count);
        speedFactor = minEnemySpeed + ((maxEnemySpeed - minEnemySpeed) - (maxEnemySpeed - minEnemySpeed) * speedCurve.Evaluate(perc));
        for (int i = 0; i < enemyList.Count; i++) {
            enemyList[i].enemy.speedFactor = speedFactor;
        }
        Debug.Log(enemyList.Count);
        if (HasWaveEnd) {
            WaveMe();
        }
    }

    public void WaveMe() {
        GameManager.instance.AddDifficulty();
        updateEnemy = false;
        GameManager.instance.spawnManager.SpawnEnemies(GameManager.instance.spawnManager.transform.position);
    }

    public void AddDifficulty() {
        maxEnemySpeed *= 1.1f;
        minEnemySpeed *= 1.1f;
        if (minEnemySpeed > maxEnemySpeed) {
            minEnemySpeed = maxEnemySpeed;
        }
        enemySpeed *= 1.1f;

    }

    #region Getters

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

    #endregion
}
