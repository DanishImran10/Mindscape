using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundScript : MonoBehaviour
{
    public Transform target; // The object to rotate around
    public float rotationSpeed = 10f; // Speed of rotation
    public float radius = 5f; // Distance from the target

    private Vector3 offset; // Stores the offset for circular rotation

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target is not set. Please assign a target object.");
            return;
        }

        // Calculate initial offset based on radius
        offset = transform.position - target.position;
        offset = offset.normalized * radius;
    }

    void Update()
    {
        if (target == null) return;

        // Rotate around the target
        transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);

        // Maintain radius distance
        Vector3 desiredPosition = (transform.position - target.position).normalized * radius + target.position;
        transform.position = desiredPosition;
    }
}
