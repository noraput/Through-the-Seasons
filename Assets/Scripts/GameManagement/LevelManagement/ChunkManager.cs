using System.Collections;
using System.Collections.Generic;
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
        private Chunk[] testingChunks;

        [SerializeField]
        private bool isTesting;

        [SerializeField]
        private float distanceToSpawnChunk;

        private Vector3 lastEndPosition;
        private float distanceFromLastEnd;

        private Transform startingChunk;
        public Transform StartingChunk { get => startingChunk; }

        private void Awake() {
            startingChunk = GameObject.FindGameObjectWithTag("StartingChunk").transform;
            lastEndPosition = startingChunk.Find("EndPosition").position;
            
            for (int i = 0; i <= chunksSpawnAtTheStart; ++i) {
                SpawnChunk();
            }
        }

        private void Update() {
            distanceFromLastEnd = GameManager.instance.GetDistanceFromPlayer(lastEndPosition.x);

            if (distanceFromLastEnd <= distanceToSpawnChunk) {
                SpawnChunk();
            }

            // Debug.Log(xDistance + "  | " + distanceFromLastEnd);
        }

        private void SpawnChunk() {
            Chunk[] chunksPool = isTesting ? testingChunks : chunks;
            Chunk chunk = chunksPool[Random.Range(0, chunksPool.Length)];
            Transform spawnedChunk = SpawnChunk(chunk, lastEndPosition);

            lastEndPosition = spawnedChunk.GetComponent<SpawnedChunk>().GetEndPosition();
            // Debug.Log(lastEndPosition + " " + spawnedChunk.GetComponent<SpawnedChunk>().GetEndPosition);
        }

        private Transform SpawnChunk(Chunk chunk, Vector3 lastEnd) {
            //Debug.Log("Last End: " + lastEnd.x + " | Padding: " + chunk.GetXPadding());
            
            Vector3 spawnPosition = new Vector3(
                lastEnd.x + chunk.GetXPadding() + chunk.offset.x,
                lastEnd.y + chunk.offset.y
            );

            return Instantiate(chunk.chunkObject, spawnPosition, Quaternion.identity);
        }   
    }
}
