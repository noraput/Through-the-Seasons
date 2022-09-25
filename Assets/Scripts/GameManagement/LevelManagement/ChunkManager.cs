using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField]
    private Transform startingChunk;

    [SerializeField]
    private Chunk[] chunks;

    [SerializeField]
    private float distanceToSpawnChunk;

    private Vector3 lastEndPosition;

    private Transform playerTransform;
    private Vector3 playerStartingPosition;
    
    private float xDistance;
    private float distanceFromLastEnd;

    private void Awake() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerStartingPosition = playerTransform.position;

        lastEndPosition = startingChunk.Find("EndPosition").position;
        
        // Just for Testing, Forget it
        SpawnChunk();
        SpawnChunk();
        SpawnChunk();
        SpawnChunk();
        SpawnChunk();
    }

    private void Update() {
        xDistance = Mathf.Abs(playerTransform.position.x - playerStartingPosition.x);
        distanceFromLastEnd = Mathf.Abs(playerTransform.position.x - lastEndPosition.x);

        Debug.Log(xDistance + " " + distanceFromLastEnd);

        if (distanceFromLastEnd <= distanceToSpawnChunk) {
            SpawnChunk();
        }
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
