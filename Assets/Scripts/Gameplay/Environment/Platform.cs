using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons {
    public class Platform : MonoBehaviour {
        [SerializeField]
        protected float offset;
        protected float dodgeOffset = 0.25f;
        protected BoxCollider2D col;

        protected Transform playerTransform;
        protected CharacterBase charBase;
        protected BoxCollider2D playerCol;

        protected (float xMin, float xMax, float yMax) bounds;
        protected bool hasEnabled, hasDisabled;

        protected virtual void Start() {
            col = GetComponent<BoxCollider2D>();
            playerCol = PlayerCore.instance.Col;

            bounds = (col.bounds.min.x, col.bounds.max.x, col.bounds.max.y);
            col.enabled = false;
        }

        protected virtual void Update() {
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
            }
        }

        protected bool ShouldEnableCollider() {
            return playerCol.bounds.min.y > bounds.yMax;
        }

        protected bool HasPlayerReachedPlatform() {
            return playerCol.bounds.max.x >= bounds.xMin;
        }

        protected bool HasPlayerExitPlatform() {
            return playerCol.bounds.min.x >= bounds.xMax;
        }
    }
}
