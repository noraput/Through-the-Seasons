using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    public static event Action<SpawnedChunk> OnChunkEnter;
    public static event Action<SpawnedChunk> OnChunkExit;

    private void Start() {
        startPosition = GetStartPosition();
        endPosition = GetEndPosition();
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
