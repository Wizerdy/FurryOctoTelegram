using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    private Character character;

    void Start() {
        character = GetComponent<Character>();
    }

    void Update() {
        Movement();
    }

    private void Movement() {
        Vector2 direction = new Vector2(Mathf.CeilToInt(Input.GetAxisRaw("Horizontal")), 0.0f);
        if (direction != Vector2.zero) {
            character.MoveTo(direction);
        }

        if (Input.GetButton("Fire1")) {
            character.Attack();
        }
    }
}
