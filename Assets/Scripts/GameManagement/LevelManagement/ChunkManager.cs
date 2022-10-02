using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField]
    private int chunksSpawnAtTheStart;

    [SerializeField]
    private Chunk[] chunks;

    [SerializeField]
    private float distanceToSpawnChunk;

    private Vector3 lastEndPosition;
    private float distanceFromLastEnd;

    private Transform startingChunk;

    private void Awake() {
        startingChunk = GameObject.FindGameObjectWithTag("StartingChunk").transform;
        lastEndPosition = startingChunk.Find("EndPosition").position;
        
        for (int i = 0; i <= chunksSpawnAtTheStart; ++i) {
            SpawnChunk();
        }
    }

    private void Update() {
        distanceFromLastEnd = Mathf.Abs(GameManager.instance.playerTransform.position.x - lastEndPosition.x);

        if (distanceFromLastEnd <= distanceToSpawnChunk) {
            SpawnChunk();
        }

        // Debug.Log(xDistance + "  | " + distanceFromLastEnd);
    }

    private void SpawnChunk() {
        Chunk chunk = chunks[Random.Range(0, chunks.Length)];
        Transform spawnedChunk = SpawnChunk(chunk, lastEndPosition);

        lastEndPosition = spawnedChunk.Find("EndPosition").position;
    }

    private Transform SpawnChunk(Chunk chunk, Vector3 lastEnd) {
        Vector3 spawnPosition = new Vector3(
            lastEnd.x + chunk.GetXPadding() + chunk.offset.x,
            lastEnd.y + chunk.offset.y
        );

        return Instantiate(chunk.chunkObject, spawnPosition, Quaternion.identity);
    }   
}
