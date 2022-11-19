using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ThroughTheSeasons {
    public class TiledPlatform : Platform
    {
        TilemapCollider2D tilemapCollider;

        protected override void Start() {
            tilemapCollider = GetComponent<TilemapCollider2D>();
            col = GetComponentInChildren<BoxCollider2D>();

            playerCol = PlayerCore.instance.Col;
            bounds = (col.bounds.min.x, col.bounds.max.x, col.bounds.max.y);
            tilemapCollider.enabled = false;

            col.enabled = false;
        }

        protected override void Update() {
            if (!HasPlayerReachedPlatform())
                return;

            if (hasDisabled)
                return;

            if (HasPlayerExitPlatform()) {
                tilemapCollider.enabled = false;
                hasDisabled = true;
            }

            if (hasEnabled)
                return;

            if (ShouldEnableCollider()) {
                tilemapCollider.enabled = true;
                hasEnabled = true;
            }
        }
    }
}

