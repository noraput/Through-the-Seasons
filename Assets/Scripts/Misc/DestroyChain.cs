using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChain : MonoBehaviour
{
    [SerializeField]
    GameObject[] objectToDestroyAfter;

    void OnDestroy() {
        foreach (GameObject go in objectToDestroyAfter)
        {
            if (go) {
                Destroy(go);
            }
        }
    }
}
