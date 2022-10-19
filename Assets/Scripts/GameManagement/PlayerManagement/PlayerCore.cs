using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCore : PersistentObject<PlayerCore>
{
    [SerializeField]
    Animator anim;

    private void Update() {
        if (GameManager.instance.IsPlayerFallFromCurrentChunk()) {
            SceneManager.LoadScene(0);
            transform.position = GameManager.instance.PlayerStartingPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Obstacle")) {
            anim.SetTrigger("Hit");
        }
        // else if (col.CompareTag("Deadzone")) {
        //     SceneManager.LoadScene(0);
        //     transform.position = GameManager.instance.playerStartingPosition;
        // }
    }
}
