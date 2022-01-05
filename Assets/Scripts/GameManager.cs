using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public List<Entity> enemies;

    public Vector2 enemyDirection;
    public Transform leftSide;
    public Transform rightSide;

    private int nextEnemyDirection = 0;

    private void Awake() {
        instance = this;
    }

    private void LateUpdate() {
        if (nextEnemyDirection > 0) {
            enemyDirection.x = Tools.Positive(enemyDirection.x);
        } else if (nextEnemyDirection < 0) {
            enemyDirection.x = Tools.Negative(enemyDirection.x);
        }
    }

    public void ChangeEnemyDirection(int direction) {
        nextEnemyDirection = direction;
    }
}