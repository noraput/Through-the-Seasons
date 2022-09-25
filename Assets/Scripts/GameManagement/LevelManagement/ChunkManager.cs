using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField]
    private Transform startingChunk;

    [SerializeField]
    private Transform chunk;

    [SerializeField]
    private float offset;

    [SerializeField]
    private float distanceToSpawnChunk;

    private Vector3 nextStartPosition;
    private Vector3 nextEndPosition;

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
        nextStartPosition = chunk.Find("StartPosition").position;
        nextEndPosition = chunk.Find("EndPosition").position;

        Transform spawnedChunk = SpawnChunk(lastEndPosition, nextStartPosition, nextEndPosition);

        lastEndPosition = spawnedChunk.Find("EndPosition").position;
    }

    private Transform SpawnChunk(Vector3 lastEnd, Vector3 nextStart, Vector3 nextEnd) {
        Vector3 spawnPosition = new Vector3(
            lastEnd.x + Mathf.Abs((nextStart.x - nextEnd.x) / 2),
            lastEndPosition.y
        );

        return Instantiate(chunk, spawnPosition, Quaternion.identity);
    }   
}
