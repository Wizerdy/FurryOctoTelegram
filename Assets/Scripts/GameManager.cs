using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    [HideInInspector] public int totalScore;
    public TextMeshProUGUI scoreText;

    public EnemyManager enemyManager;
    public SpawnManager spawnManager;
    public BackgroundManager backgroundManager;

    public Vector2 enemyDirection;
    public Transform leftSide;
    public Transform rightSide;

    public GameObject[] organs;

    public GameObject bloodExplosion;

    public ScoreManager scoreManager;
    public CameraController cameraManager;
    public ParticleSystem ps;
    private int nextEnemyDirection = 0;

    public Transform leftBound, rightBound, topBound, botBound;

    private void Awake() {
        instance = this;
    }

    private void Start()
    {
        SoundManager.i.Play("Music");
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
        scoreText.text = totalScore.ToString();
        scoreManager.Shake();
    }

    public void OrganExplosion(Transform spawnPoint, int number) {
        for (int i = 0; i < number; i++) {
            if (organs.Length <= 0) { return; }
            float randX = Random.Range(-1f, 1f);
            float randY = Random.Range(-1f, 1f);
            Vector3 randomVector3 = new Vector3(spawnPoint.position.x + randX, spawnPoint.position.y + randY, spawnPoint.position.z);
            GameObject Organ = Instantiate(organs[Random.Range(0, organs.Length - 1)], randomVector3, spawnPoint.rotation, transform);
            //Organ.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f)), ForceMode2D.Impulse);
            backgroundManager.AddToRoad(Organ);
        }
    }

    public void BloodExplosion(Transform spawnPoint)
    {
        GameObject pEffect = Instantiate(bloodExplosion, spawnPoint.position, spawnPoint.rotation, transform);
        backgroundManager.AddToRoad(pEffect);
        Destroy(pEffect, 10f);
    }

    public void AddDifficulty() {
        enemyManager.enemySpeed += enemyManager.enemySpeed * 0.1f;
    }
}