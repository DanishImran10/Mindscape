using UnityEngine;

public class HeartbeatSlomoEffect : MonoBehaviour
{
    public Camera mainCamera;
    public float slomoDuration = 10f; // Total duration of slomo
    public float heartbeatInterval = 0.6f; // Time between pulses
    public float slomoTimeScale = 0.3f; // Slow time scale
    public float fovPulseAmount = 5f; // Camera FOV bump
    private float originalTimeScale;
    private float originalFOV;

    void Start()
    {
        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera not assigned!");
            return;
        }

        originalTimeScale = Time.timeScale;
        originalFOV = mainCamera.fieldOfView;
        StartCoroutine(HeartbeatEffect());
    }

    System.Collections.IEnumerator HeartbeatEffect()
    {
        Time.timeScale = slomoTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        float elapsed = 0f;
        while (elapsed < slomoDuration)
        {
            // Heartbeat pulse (FOV bump)
            mainCamera.fieldOfView = originalFOV + fovPulseAmount;
            yield return new WaitForSecondsRealtime(0.1f); // short pulse
            mainCamera.fieldOfView = originalFOV;

            yield return new WaitForSecondsRealtime(heartbeatInterval);
            elapsed += heartbeatInterval;
        }

        // Reset
        Time.timeScale = originalTimeScale;
        Time.fixedDeltaTime = 0.02f;
        mainCamera.fieldOfView = originalFOV;
    }
}
