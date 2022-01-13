using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : Enemy {

    protected override void OnStart() {
        base.OnStart();
        if (attackSpeed <= 0.0f) { return; }
        attackCooldown = Random.Range(0, 1f / attackSpeed);
    }

    protected override void OnSetMove(Vector2 direction) { }

    protected override void OnUpdate() {
        MoveTo(Vector2.right);

        if (GameManager.instance.rightBound == null) { return; }
        if (rb.position.x > GameManager.instance.rightBound.position.x) {
            Destroy(gameObject);
        }
    }
}
