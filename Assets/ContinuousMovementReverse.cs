using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousMovementReverse : MonoBehaviour
{
    public float moveSpeed = 5f;    // Movement speed
    public float zThreshold = -345f;  // Z-axis position to trigger reset
    private Vector3 startPosition;  // Initial position storage

    void Start()
    {
        // Store initial position when game starts
        startPosition = transform.position;
    }

    void Update()
    {
        // Move object forward continuously
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // Check if Z position exceeds threshold
        if(transform.position.z <= zThreshold)
        {
            // Reset position to start position
            transform.position = startPosition;
        }
    }
}
