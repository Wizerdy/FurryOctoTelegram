using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {
    public GameObject mapPrefab;
    public Transform[] maps = new Transform[3];
    public float speed = -1f;

    private Vector2 mapSize;

    void Start() {
        Sprite mapSprite = mapPrefab.GetComponent<SpriteRenderer>().sprite;
        Rect rect = mapSprite.rect;
        mapSize = Vector2.Scale(mapPrefab.transform.localScale, new Vector2(rect.width, rect.height));
        mapSize /= mapSprite.pixelsPerUnit;
        for (int i = 0; i < maps.Length; i++) {
            if (maps[i] == null) {
                maps[i] = Instantiate(mapPrefab, Vector3.zero + Vector3.up * mapSize.y * i, Quaternion.identity).transform;
                maps[i].transform.parent = transform.parent;
            }
        }
    }

    void Update() {
        for (int i = 0; i < maps.Length; i++) {
            maps[i].position += Vector3.up * speed * Time.deltaTime;
        }
        if (maps[0].position.y <= -mapSize.y) {
            maps[0].position = maps[2].position + Vector3.up * mapSize.y;
            for (int i = 0; i < maps[0].childCount; i++) {
                Destroy(maps[0].GetChild(i).gameObject);
            }

            Transform tempMap = maps[0];
            maps[0] = maps[1];
            maps[1] = maps[2];
            maps[2] = tempMap;
        }
    }

    public void AddToRoad(GameObject obj) {
        obj.transform.parent = maps[1];
    }
}
