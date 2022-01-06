using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    [HideInInspector]public int totalScore;
    public Text scoreText;

    public EnemyManager enemyManager;

    public Vector2 enemyDirection;
    public Transform leftSide;
    public Transform rightSide;

    public ScoreManager scoreManager;
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

    public void AddScore(int amount)
    {
        totalScore += amount;
        scoreText.text = "Score : " + totalScore.ToString();
        scoreManager.Shake();
    }
}