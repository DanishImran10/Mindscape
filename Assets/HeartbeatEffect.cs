using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartbeatEffect : MonoBehaviour
{
    private Camera cam;
    private float originalFOV;
    public bool isPulsing = false;
    public float pulseSpeed = 1.5f;
    public float pulseAmount = 2f; // FOV change amount

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam != null)
            originalFOV = cam.fieldOfView;
    }

    void Update()
    {
        if (isPulsing && cam != null)
        {
            cam.fieldOfView = originalFOV + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        }
        else if (cam != null)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, originalFOV, Time.deltaTime * 3f);
        }
    }
}
