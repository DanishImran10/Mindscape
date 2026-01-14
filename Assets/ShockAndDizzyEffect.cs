using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockAndDizzyEffect : MonoBehaviour
{
    public float shockDuration = 0.5f;
    public float shockIntensity = 0.2f;

    public float dizzyDuration = 2.5f;
    public float dizzyIntensity = 10f;

    private float shockTimer = 0f;
    private float dizzyTimer = 0f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool isShocking = false;
    private bool isDizzy = false;

    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        if (isShocking)
        {
            shockTimer -= Time.deltaTime;
            if (shockTimer > 0)
            {
                transform.localPosition = originalPosition + Random.insideUnitSphere * shockIntensity;
            }
            else
            {
                isShocking = false;
                isDizzy = true;
                dizzyTimer = dizzyDuration;
                transform.localPosition = originalPosition;
            }
        }
        else if (isDizzy)
        {
            dizzyTimer -= Time.deltaTime;
            if (dizzyTimer > 0)
            {
                float angle = Mathf.Sin(Time.time * 5f) * dizzyIntensity;
                transform.localRotation = originalRotation * Quaternion.Euler(0, 0, angle);
            }
            else
            {
                isDizzy = false;
                transform.localRotation = originalRotation;
            }
        }
    }

    // Call this to trigger the full effect
    public void TriggerShockAndDizzy()
    {
        shockTimer = shockDuration;
        isShocking = true;
        isDizzy = false;
    }
}
