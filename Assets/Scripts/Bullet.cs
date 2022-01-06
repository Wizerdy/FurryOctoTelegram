using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public GameObject body;
    public float bulletSpeed;
    public int side;

    private void Start() {
        StartCoroutine(IDestroyAfter(5f));
    }

    private void Update() {
        transform.position += transform.up * bulletSpeed * Time.deltaTime;
    }

    private IEnumerator IDestroyAfter(float time) {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Entity entity = collision.GetComponent<Entity>();
        if (entity != null) {
            if (entity.Side == side) { return; }

            if (collision.gameObject.CompareTag("Player")) {
                // YOU LOSE
                Debug.Break();
            }

            Destroy(gameObject);
        }
    }
}
