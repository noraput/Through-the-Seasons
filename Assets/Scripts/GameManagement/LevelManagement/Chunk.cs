using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chunk-0X", menuName = "Chunk")]
public class Chunk : ScriptableObject
{
    public Transform chunkObject;
    public Vector3 offset; 

    public float GetXPadding()
    {
        Vector3 startPosition = chunkObject.Find("StartPosition").position;
        Vector3 endPosition = chunkObject.Find("EndPosition").position;

        return Mathf.Abs((startPosition.x - endPosition.x) / 2);
    }
}