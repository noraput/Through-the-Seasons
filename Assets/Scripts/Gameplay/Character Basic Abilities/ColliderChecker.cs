using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColliderChecker : MonoBehaviour
{
    [SerializeField]
    Animator anim;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Obstacle")) {
            anim.SetTrigger("Hit");
        }

        if (col.CompareTag("Deadzone")) {
            SceneManager.LoadScene(0);  
        }
    }
}
