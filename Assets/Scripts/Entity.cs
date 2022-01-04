using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float bulletSpeed = 1f;
    [SerializeField] protected float attackSpeed = 1f;
    protected float attackCooldown = 0f;

    void Start() {

    }

    void Update() {

    }

    IEnumerator IAttackCooldown() {
        yield return null;
    }
}
