using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            Collect();
        }
    }

    public virtual void Collect() {
        Destroy(gameObject);
    }
}
