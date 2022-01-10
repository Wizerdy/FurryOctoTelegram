using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [Header("Grid")]
    public List<Enemy> enemyPrefab;
    public Vector2 cellNumbers;
    public Vector2 margin;

    [Header("UFO")]
    public List<Enemy> ufoPrefab;
    public float ufoSpawnTime = 5.0f;
    public float ufoSpawnTimeDelta = 0.5f;
    private float ufoSpawnTimer;

    void Start() {
        SpawnEnemies(transform.position);
    }

    private void Update() {
        if (ufoSpawnTimer > 0.0f) {
            ufoSpawnTimer -= Time.deltaTime;
        } else {

            if (ufoPrefab.Count <= 0) { return; }
            ufoSpawnTimer = Random.Range(ufoSpawnTime - ufoSpawnTimeDelta, ufoSpawnTime + ufoSpawnTimeDelta);
            SpawnUFO(transform.position);
        }
    }

    public void SpawnEnemies(Vector2 position) {
        if (enemyPrefab == null || enemyPrefab.Count == 0) { return; }

        int enemyIndex = 0;
        for (int y = 0; y < cellNumbers.y; y++) {
            for (int x = 0; x < cellNumbers.x; x++) {
                Vector2 pos = new Vector2(x * margin.x, y * margin.y);
                pos += position;
                Enemy lastEnemy = Instantiate(enemyPrefab[enemyIndex], pos, Quaternion.identity);
                lastEnemy.transform.parent = transform;
                EnemyManager.instance.enemyList.Add(new EnemyManager.EnemyCell(new Vector2(y, x), lastEnemy));
            }
            if (y % 2 == 0) {
                enemyIndex++;
                if (enemyIndex >= enemyPrefab.Count) { enemyIndex = enemyPrefab.Count - 1; }
            }
        }
    }

    public void SpawnUFO(Vector2 position) {
        if (ufoPrefab == null) { return; }

        int index = Random.Range(0, ufoPrefab.Count - 1);
        Enemy lastEnemy = Instantiate(ufoPrefab[index], position, Quaternion.identity);
        EnemyManager.instance.ufoList.Add(lastEnemy);
    }
}
