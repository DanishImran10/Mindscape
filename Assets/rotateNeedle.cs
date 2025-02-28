using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateNeedle : MonoBehaviour
{
    public float rotationSpeed = 50f;
    private float rotationDuration = 5f;
    private float elapsedTime = 0f;

    void FixedUpdate()
    {
        if (elapsedTime < rotationDuration)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, 0f, rotationStep);
            elapsedTime += Time.deltaTime;
        }
    }
}
