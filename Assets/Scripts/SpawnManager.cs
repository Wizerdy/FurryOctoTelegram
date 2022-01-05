using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public List<Enemy> enemyPrefab;
    public Vector2 cellNumbers;
    public Vector2 margin;

    void Start() {
        SpawnEnemies(transform.position);
    }

    public void SpawnEnemies(Vector2 position) {
        if (enemyPrefab == null || enemyPrefab.Count == 0) { return; }

        int enemyIndex = 0;
        for (int y = 0; y < cellNumbers.y; y++) {
            for (int x = 0; x < cellNumbers.x; x++) {
                Vector2 pos = new Vector2(x * margin.x, y * margin.y);
                pos += position;
                Enemy prefab = Instantiate(enemyPrefab[enemyIndex], pos, Quaternion.identity);
                prefab.transform.parent = transform;
                GameManager.instance.enemies.Add(prefab);
            }
            if (y % 2 == 0) {
                enemyIndex++;
                if (enemyIndex >= enemyPrefab.Count) { enemyIndex = enemyPrefab.Count - 1; }
            }
        }
    }
}
