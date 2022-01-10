using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity {
    protected override void UpdateMove() { }

    public override void MoveTo(Vector2 direction) {
        if (rb.position.x > GameManager.instance.rightSide.position.x && direction.x > 0) { return; }
        if (rb.position.x < GameManager.instance.leftSide.position.x && direction.x < 0) { return; }
        rb.position += direction * speed * Time.deltaTime;
    }
}
