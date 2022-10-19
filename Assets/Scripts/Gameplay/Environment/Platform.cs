using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
    [SerializeField]
    private float offset;
    private Collider2D col;
    private Transform playerTransform;

    void Start() {
        col = GetComponent<Collider2D>();
        playerTransform = GameManager.instance.PlayerTransform;
    }

    void Update() {
        col.enabled = playerTransform.position.y > transform.position.y + offset; 
    }
}
