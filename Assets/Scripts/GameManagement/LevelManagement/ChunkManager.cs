using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class ChunkManager : MonoBehaviour
    {
        [SerializeField]
        private int chunksSpawnAtTheStart;

        [SerializeField]
        private Chunk[] chunks;

        [SerializeField]
        private Chunk[] flyingChunks;

        [SerializeField]
        private Chunk[] testingChunks;

        [SerializeField]
        private bool isTesting;

        [SerializeField]
        private float distanceToSpawnChunk;

        [SerializeField]
        private float flyingOffset;

        [SerializeField]
        private float floorOffset = -4.25f;

        private Vector3 lastEndPosition;
        private float distanceFromLastEnd;

        private Transform startingChunk;
        public Transform StartingChunk { get => startingChunk; }

        private bool isFlying;
        private bool isFirstFlyingChunk;
        private bool isFirstNormalChunk;

        private void Awake() {
            startingChunk = GameObject.FindGameObjectWithTag("StartingChunk").transform;
            lastEndPosition = startingChunk.Find("EndPosition").position;
            
            // for (int i = 0; i <= chunksSpawnAtTheStart; ++i) {
            //     SpawnChunk();
            // }
        }

        private void Update() {
            distanceFromLastEnd = GameManager.instance.GetDistanceFromPlayer(lastEndPosition.x);

            if (distanceFromLastEnd <= distanceToSpawnChunk) {
                SpawnChunk();
            }

            // Debug.Log(xDistance + "  | " + distanceFromLastEnd);
        }

        private void SpawnChunk() {
            Chunk[] chunksPool = PickChunkPool();
            Chunk chunk = PickChunk(chunksPool); 
            SpawnedChunk spawnedChunk = SpawnChunk(chunk, lastEndPosition).GetComponent<SpawnedChunk>();
            
            lastEndPosition = spawnedChunk.GetEndPosition();
            spawnedChunk.InitializeItems(chunk.season);
            
            // Debug.Log(lastEndPosition + " " + spawnedChunk.GetComponent<SpawnedChunk>().GetEndPosition);
        }

        private void SpawnChunkForced(Vector3 position) {
            Chunk[] chunksPool = PickChunkPool();
            Chunk chunk = PickChunk(chunksPool); 
            SpawnedChunk spawnedChunk = SpawnChunk(chunk, position).GetComponent<SpawnedChunk>();
            
            lastEndPosition = spawnedChunk.GetEndPosition();
            spawnedChunk.InitializeItems(chunk.season);
            
            // Debug.Log(lastEndPosition + " " + spawnedChunk.GetComponent<SpawnedChunk>().GetEndPosition);
        }

        private Chunk[] PickChunkPool() {
            return isFlying
                ? flyingChunks
                : isTesting
                ? testingChunks
                : chunks;
        }

        private Chunk PickChunk(Chunk[] pool) {
            return (isFlying && isFirstFlyingChunk)
                ? pool.First(chunk => chunk.isStartingChunk)
                : pool[Random.Range(0, pool.Length)];
        }

        private Transform SpawnChunk(Chunk chunk, Vector3 lastEnd) {
            //Debug.Log("Last End: " + lastEnd.x + " | Padding: " + chunk.GetXPadding());

            Vector3 spawnPosition = new Vector3(
                lastEnd.x + chunk.GetXPadding() + chunk.offset.x,
                lastEnd.y + chunk.offset.y
            );
            
            if (isFlying && isFirstFlyingChunk) {
                spawnPosition += Vector3.up * flyingOffset;
                isFirstFlyingChunk = false;
            }
            else if (!isFlying && isFirstNormalChunk) {
                spawnPosition += Vector3.down * flyingOffset;
                isFirstNormalChunk = false;
            }

            return Instantiate(chunk.chunkObject, spawnPosition, Quaternion.identity);
        }

        public void StartFlying() {
            isFlying = true;
            isFirstFlyingChunk = true;

            SpawnChunkForced(
                new Vector3(
                    GameManager.instance.PlayerTransform.position.x,
                    floorOffset
                )
            );
        }

        public void StopFlying() {
            isFlying = false;
            isFirstNormalChunk = true;
        }
    }
}
