using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentObject<GameManager>
{
    public Transform playerTransform;
    public Vector3 playerStartingPosition;
    
    public float xDistance;

    void Awake() {
        base.Awake();
        
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerStartingPosition = playerTransform.position;
    }

    void Update() {
        xDistance = Mathf.Abs(playerTransform.position.x - playerStartingPosition.x);
    }
}
