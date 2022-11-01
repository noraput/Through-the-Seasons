using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThroughTheSeasons
{
    public class PlayerCore : PersistentObject<PlayerCore>
    {
        private Collider2D col;
        public Collider2D Col { get => col; }

        [SerializeField]
        Animator anim;

        protected void Awake() {
            base.Awake();
            col = GetComponent<Collider2D>();
        }

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
}
