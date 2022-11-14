using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class SpawnedChunk : MonoBehaviour
    {
        private Vector3 startPosition;
        private Vector3 endPosition;

        private float distanceFromEndPosition;
        private float disableDistance = 200f;
        
        private bool isPlayerInChunk;
        private bool hasReachedTheEnd;

        private bool isEnterEventFired;
        private bool isExitEventFired;

        public bool isStartingChunk;
        
        private Season season;
        
        private float treeMinDistance = 15f;
        private float treeMaxDistance = 20f;
        private float treeMaxYOffset = 0.075f;
        private float treeMargin = 5f;
        private float treeYOrigin = -4.25f;
        private float treeRaycastDistance = 10f;
        private float treeColliderCheckMultiplier = 1f;
        private float treeShiftStep = 5f;
        private float currentTreeCheckXPosition;
        
        public static event Action<SpawnedChunk> OnChunkEnter;
        public static event Action<SpawnedChunk> OnChunkExit;

        private void Start() {
            startPosition = GetStartPosition();
            endPosition = GetEndPosition();

            if (isStartingChunk)
                return;

            SpawnedRandomTrees();
        }

        public void InitializeItems(Season season) {
            this.season = season;

            CollectibleItem[] collectibleItems = GetComponentsInChildren<CollectibleItem>();
            if (!collectibleItems.Any())
                return;
            
            foreach (CollectibleItem item in collectibleItems) {
                item.Initialize(season);
            }
        }

        private void SpawnedRandomTrees() {
            List<GameObject> trees = TileAssets.instance.GetTreePoolFromSeason(season);
            
            if (!trees.Any())
                return;
            
            float xDistance = 0f;
            float yDistance = treeYOrigin - UnityEngine.Random.Range(0f, treeMaxYOffset);

            currentTreeCheckXPosition = startPosition.x + treeMargin;    
            Vector2 treePosition = new Vector2(currentTreeCheckXPosition, yDistance);

            Transform fristTree = Instantiate(trees.PickRandom(), treePosition, Quaternion.identity).transform;
            fristTree.SetParent(transform);

            GameObject lastTree = fristTree.gameObject;
            Vector2 frontTreeRaycastOrigin, backTreeRaycastOrigin = Vector2.zero;
            bool isShifting = false;

            while (currentTreeCheckXPosition <= endPosition.x - treeMargin) {
                GameObject tree = trees.PickRandom();
                while (tree.name == lastTree.name) {
                    tree = trees.PickRandom();
                }

                lastTree = tree;
                Renderer treeRenderer = tree.transform.Find("Tree").GetComponent<Renderer>();

                xDistance = isShifting
                    ? treeShiftStep
                    : UnityEngine.Random.Range(treeMinDistance, treeMaxDistance);
                    
                
                yDistance = treeYOrigin - UnityEngine.Random.Range(0f, treeMaxYOffset);
                currentTreeCheckXPosition += xDistance;

                if (!tree || !treeRenderer) {
                    Debug.LogError("Tree Prefab and/or Tree's Sprite Renderer Not Found!");
                    continue;
                }

                treePosition = new Vector2(currentTreeCheckXPosition, yDistance);
                
                frontTreeRaycastOrigin = new Vector2(
                    treePosition.x + treeRenderer.bounds.extents.x,
                    treePosition.y + treeRenderer.bounds.extents.y
                );

                backTreeRaycastOrigin = new Vector2(
                    treePosition.x - treeRenderer.bounds.extents.x,
                    treePosition.y + treeRenderer.bounds.extents.y
                );

                Debug.Log(treePosition.x + treeRenderer.bounds.extents.x);

                RaycastHit2D[] frontHits = Physics2D.RaycastAll(frontTreeRaycastOrigin, Vector2.down, treeRaycastDistance);
                RaycastHit2D[] backHits = Physics2D.RaycastAll(backTreeRaycastOrigin, Vector2.down, treeRaycastDistance);
                
                Collider2D[] obstacles = Physics2D.OverlapBoxAll(
                    treePosition,
                    treeRenderer.bounds.size * 5f,
                    0f
                );

                // string debug = "Layers:";
                // foreach (RaycastHit2D hit in frontHits) {
                //     debug += " " + LayerMask.LayerToName(hit.collider.gameObject.layer);
                // }
                
                // Debug.Log(debug);

                if (!frontHits.Any() || !backHits.Any()) {
                    Debug.Log("End of Chunk/Falling Pit");
                    isShifting = true;
                    continue;
                }

                if (obstacles.Any(col => col.gameObject.CompareLayer("Obstacle"))) {
                    Debug.Log("Obstacle");
                    isShifting = true;
                    continue;
                }

                if (obstacles.Any(col => col.gameObject.CompareLayer("Collectible"))) {
                    Debug.Log("Collectible");
                    isShifting = true;
                    continue;
                }

                if (obstacles.Any(col => col.CompareTag("Platform"))) {
                    Debug.Log("Platform");
                    isShifting = true;
                    continue;
                }

                Transform spawnedTree = Instantiate(tree, treePosition, Quaternion.identity).transform;
                spawnedTree.SetParent(transform);
                isShifting = false;

                //Debug.Log("Tree Spawned: " + spawnedTree.name + " | Pos: " + spawnedTree.transform.position);
            }
        }

        public Vector3 GetStartPosition() {
            return transform.Find("StartPosition").position;
        }

        public Vector3 GetEndPosition() {
            return transform.Find("EndPosition").position;
        }
        
        public void Update() {
            CheckIfPlayerIsInChunk();  
            CheckEndPosition();
        }

        private void CheckEndPosition() {
            hasReachedTheEnd = GameManager.instance.PlayerTransform.position.x > endPosition.x;

            if (!hasReachedTheEnd)
                return;

            if (!isExitEventFired) {
                OnChunkExit?.Invoke(this);
                isExitEventFired = true;
            }

            distanceFromEndPosition = GameManager.instance.GetDistanceFromPlayer(endPosition.x);

            if (distanceFromEndPosition >= disableDistance)
                Destroy(gameObject);
        }

        private void CheckIfPlayerIsInChunk() {
            isPlayerInChunk = (GameManager.instance.PlayerTransform.position.x)
                .IsBetween(startPosition.x, endPosition.x);

            if (!isPlayerInChunk)
                return;

            if (!isEnterEventFired) {
                OnChunkEnter?.Invoke(this);
                isEnterEventFired = true;
            }
        }
    }   
}
