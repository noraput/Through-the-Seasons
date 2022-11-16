using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField]
    GameObject cam;

    [SerializeField]
    float parallaxMultiplier;

    [SerializeField]
    float offset;

    [SerializeField]
    bool isVerticallyStatic;
    
    public bool needsFading = false;

    private SpriteRenderer spriteRenderer;

    private float width, startPosition, xPosition, distance;
    private float defaultYPosition;

    private void Start() {
        startPosition = transform.position.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
        width = spriteRenderer.bounds.size.x;
        defaultYPosition = transform.position.y;
    }
    
    private void Update() {
        distance = cam.transform.position.x * (1 - parallaxMultiplier);
        xPosition = cam.transform.position.x * parallaxMultiplier;
        
        transform.position = new Vector3(
            startPosition + xPosition,
            isVerticallyStatic ? transform.position.y : defaultYPosition,
            transform.position.z
        );

        if (distance > startPosition + width + offset)
            startPosition += width;

        else if (distance < startPosition - width + offset)
            startPosition -= width;
    }
}
