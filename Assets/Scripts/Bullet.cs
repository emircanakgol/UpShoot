using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }

    void Start() {
        _rb.AddForce(Vector3.forward * Shooting.instance.bulletForce);
        StartCoroutine(DestroyAfterSeconds());
    }

    private IEnumerator DestroyAfterSeconds() {
        yield return new WaitForSeconds(Shooting.instance.bulletLife);
        Destroy(gameObject);
    }
}