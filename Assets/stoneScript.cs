using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stoneScript : MonoBehaviour
{ 
    public Vector3 force = new Vector3(0, 300, 500);
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(force);
        }
        else
        {
            Debug.LogError("No Rigidbody found on the stone object.");
        }
    }
}
