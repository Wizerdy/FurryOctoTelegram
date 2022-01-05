using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity {

    public void MoveTo(Vector2 direction) {
        direction.y = 0;
        rb.position += direction * speed * Time.deltaTime;
    }
}
