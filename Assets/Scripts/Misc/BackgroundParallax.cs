using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField]
    GameObject cam;

    [SerializeField]
    float parallaxMultiplier;

    [SerializeField]
    float offset;

    private float width, startPosition, xPosition, distance;

    void Start() {
        startPosition = transform.position.x;
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update() {
        distance = cam.transform.position.x * (1 - parallaxMultiplier);
        xPosition = cam.transform.position.x * parallaxMultiplier;
        
        transform.position = new Vector3(
            startPosition + xPosition,
            transform.position.y,
            transform.position.z
        );

        if (distance > startPosition + width + offset)
            startPosition += width;

        else if (distance < startPosition - width + offset)
            startPosition -= width;
    }
}
