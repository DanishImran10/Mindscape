using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;
using System.Collections.Generic;
public class CameraDistortionEffect : MonoBehaviour
{
    public PostProcessVolume volume;
    private LensDistortion lensDistortion;
    private Coroutine effectRoutine;

    void Start()
    {
        if (volume.profile.TryGetSettings(out lensDistortion))
        {
            lensDistortion.intensity.value = 0f;
        }
    }

    public void TriggerDistortionEffect()
    {
        if (effectRoutine != null) StopCoroutine(effectRoutine);
        effectRoutine = StartCoroutine(DistortionSequence());
    }

    IEnumerator DistortionSequence()
    {
        float duration = 0.5f;
        float elapsed = 0f;

        // Zoom into distortion
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            lensDistortion.intensity.value = Mathf.Lerp(0f, -70f, elapsed / duration);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        // Fade out distortion
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            lensDistortion.intensity.value = Mathf.Lerp(-70f, 0f, elapsed / duration);
            yield return null;
        }

        lensDistortion.intensity.value = 0f;

        // Optional: Do position change here
        transform.position = new Vector3(0, 2, 5); // Example
    }
}
