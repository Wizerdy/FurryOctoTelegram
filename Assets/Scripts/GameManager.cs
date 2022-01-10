using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    [HideInInspector] public int totalScore;
    public Text scoreText;

    public EnemyManager enemyManager;
    public SpawnManager spawnManager;

    public Vector2 enemyDirection;
    public Transform leftSide;
    public Transform rightSide;

    public GameObject[] organs;

    public ScoreManager scoreManager;
    public CameraController cameraManager;
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

    public void AddScore(int amount) {
        totalScore += amount;
        scoreText.text = "Score : " + totalScore.ToString();
        scoreManager.Shake();
    }

    public void OrganExplosion(Transform spawnPoint, int number) {
        for (int i = 0; i < number; i++) {
            if (organs.Length <= 0) { return; }
            GameObject Organ = Instantiate(organs[Random.Range(0, organs.Length - 1)], spawnPoint.position, spawnPoint.rotation, transform);
            Organ.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-25, 25), Random.Range(-10, 25)), ForceMode2D.Impulse);
            Destroy(Organ, 3f);
        }
    }
}