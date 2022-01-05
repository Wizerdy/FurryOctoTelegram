using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float bulletSpeed;
    public int side;

    private void Start() {
        StartCoroutine(IDestroyAfter(5f));
    }

    private void Update() {
        transform.position += new Vector3(0, 1, 0) * bulletSpeed * Time.deltaTime;
    }

    private IEnumerator IDestroyAfter(float time) {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Entity entity = collision.GetComponent<Entity>();
        if (entity != null) {
            if (entity.Side == side) { return; }

            Destroy(gameObject);
        }
    }
}
