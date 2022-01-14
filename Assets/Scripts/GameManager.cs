using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
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
    public ParticleSystem gameOverParticle;
    [HideInInspector] public bool gameOver;

    public Transform leftBound, rightBound, topBound, botBound;


    [Header("Effects")]
    public static bool[] effects = new bool[10];
    [Header("Score")]
    public GameObject bloodScore;
    public Animator heartScore;
    [Header("Bullets")]
    public List<GameObject> bullets;
    [Header("Cape")]
    public GameObject capeCloth;
    [Header("MapAmbiance")]
    public List<GameObject> mapAmbianceParticles;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        SoundManager.i.Play("Music");

        if (effects == null) {
            effects = new bool[10];
        }

        for (int i = 0; i < effects.Length; i++) {
            UpdateEffect(i);
        }
    }

    private void LateUpdate() {
        if (nextEnemyDirection > 0) {
            enemyDirection.x = Tools.Positive(enemyDirection.x);
        } else if (nextEnemyDirection < 0) {
            enemyDirection.x = Tools.Negative(enemyDirection.x);
        }

        for (int i = 0; i < effects.Length; i++) {
            if (Input.GetKeyDown(i.ToString())) {
                effects[i] = !effects[i];
                UpdateEffect(i);
            }
        }

        if (gameOver)
        {
            ResetGame();
        }
    }

    public void UpdateEffect(int index) {
        switch (index) {
            case 1:
                if (bloodScore != null) {
                    bloodScore.SetActive(effects[1]);
                }
                if (heartScore != null) {
                    heartScore.enabled = effects[1];
                }
                break;
            case 2:
                if (bullets.Count > 0) {
                    for (int i = 0; i < bullets.Count; i++) {
                        Animator bulletAnimator = bullets[i].transform.GetChild(0).GetComponent<Animator>();
                        bulletAnimator.enabled = effects[2];
                        bulletAnimator.transform.GetChild(0).gameObject.SetActive(effects[2]);
                    }
                }
                break;
            case 4:
                if (capeCloth != null) {
                    for (int i = 0; i < capeCloth.transform.childCount; i++) {
                        capeCloth.transform.GetChild(i).GetComponent<Rigidbody2D>().simulated = effects[4];
                    }
                }
                break;
            case 5:
                Camera.main.GetComponent<Volume>().enabled = effects[5];
                break;
            case 6:
                for (int i = 0; i < mapAmbianceParticles.Count; i++) {
                    mapAmbianceParticles[i].SetActive(effects[6]);
                }
                for (int i = 0; i < backgroundManager.maps.Length; i++) {
                    Transform firstChild = backgroundManager.maps[i].GetChild(0);
                    for (int j = 0; j < firstChild.childCount; j++) {
                        firstChild.GetChild(j).gameObject.SetActive(effects[6]);
                    }
                }
                break;
            case 8:
                for (int i = 0; i < enemyManager.enemyList.Count; i++) {
                    enemyManager.enemyList[i].enemy.ToggleAnimator(effects[8]);
                }
                break;
        }
    }

    public void ChangeEnemyDirection(int direction) {
        nextEnemyDirection = direction;
    }

    public void AddScore(int amount) {
        totalScore += amount;
        scoreText.text = totalScore.ToString();
        if (effects[1]) {
            scoreManager.Shake();
        }
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

    public void BloodExplosion(Transform spawnPoint) {
        GameObject pEffect = Instantiate(bloodExplosion, spawnPoint.position, spawnPoint.rotation, transform);
        backgroundManager.AddToRoad(pEffect);
        Destroy(pEffect, 10f);
    }

    public void AddDifficulty() {
        enemyManager.AddDifficulty();
    }

    public void GameOver() {
        gameOverParticle.Play();
        gameOver = true;
        Time.timeScale = 0;
    }

    public void ResetGame()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            gameOver = false;
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}