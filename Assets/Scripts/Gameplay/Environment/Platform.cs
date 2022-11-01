using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons {
    public class Platform : MonoBehaviour {
        [SerializeField]
        private float offset;
        private float dodgeOffset = 0.25f;
        private BoxCollider2D col;

        private Transform playerTransform;
        private CharacterBase charBase;
        private BoxCollider2D playerCol;

        private (float xMin, float xMax, float yMax) bounds;
        private bool hasEnabled, hasDisabled;

        private void Start() {
            col = GetComponent<BoxCollider2D>();
            //playerTransform = GameManager.instance.PlayerTransform;
            playerCol = (BoxCollider2D) PlayerCore.instance.Col;

            bounds = (col.bounds.min.x, col.bounds.max.x, col.bounds.max.y);
            col.enabled = false;
        }

        private void Update() {
            if (!HasPlayerReachedPlatform())
                return;

            if (hasDisabled)
                return;

            if (HasPlayerExitPlatform()) {
                col.enabled = false;
                hasDisabled = true;
            }

            if (hasEnabled)
                return;

            if (ShouldEnableCollider()) {
                col.enabled = true;
                hasEnabled = true;

                Debug.Log("Enabled");
            }
        }

        private bool ShouldEnableCollider() {
            return playerCol.bounds.min.y > bounds.yMax;
        }

        private bool HasPlayerReachedPlatform() {
            return playerCol.bounds.max.x >= bounds.xMin;
        }

        private bool HasPlayerExitPlatform() {
            return playerCol.bounds.min.x >= bounds.xMax;
        }
    }
}
