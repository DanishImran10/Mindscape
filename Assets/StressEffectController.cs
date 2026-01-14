using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class StressEffectController : MonoBehaviour
{
    public PostProcessVolume volume;
    private Vignette vignette;
    private ChromaticAberration chromatic;
    private ColorGrading colorGrading;

    public bool isStressed = false;

    void Start()
    {
        volume.profile.TryGetSettings(out vignette);
        volume.profile.TryGetSettings(out chromatic);
        volume.profile.TryGetSettings(out colorGrading);
    }

    void Update()
    {
        if (isStressed)
        {
            float pulse = Mathf.Abs(Mathf.Sin(Time.time * 6f));

            if (vignette != null)
                vignette.intensity.value = Mathf.Lerp(0.2f, 0.6f, pulse);

            if (colorGrading != null)
                colorGrading.postExposure.value = Mathf.Lerp(-1f, 0f, pulse); // screen darkens & brightens with pulse
        }
        else
        {
            if (vignette != null)
                vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0.1f, Time.deltaTime * 2f);

            if (colorGrading != null)
                colorGrading.postExposure.value = Mathf.Lerp(colorGrading.postExposure.value, 0f, Time.deltaTime * 2f);
        }
    }

}
