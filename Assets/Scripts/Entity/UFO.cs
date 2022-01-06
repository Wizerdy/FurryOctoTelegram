using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : Enemy {
    protected override void OnMove(Vector2 direction) { }

    protected override void OnUpdate() {
        MoveTo(Vector2.right);
    }
}
