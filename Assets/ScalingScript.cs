using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingScript : MonoBehaviour
{
    private float scaleSpeed = 0.001f;
    public Vector3 scaleDirection = new Vector3(1f, 1f, 1f);

    void FixedUpdate()
    {
        transform.localScale += scaleDirection * scaleSpeed * Time.deltaTime;
    }
}
