using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusSoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource busRideAudio;           // Continuous at start
    public AudioSource crowdBackgroundAudio;   // Continuous at start
    public AudioSource notificationAudio;      // Loops at interval
    public AudioSource gigglingAudio;          // Loops at interval
    public AudioSource insaneFunnyScreamAudio; // Manually triggered
    public AudioClip fastBreathingAudio;         // Assign your audio clip in the Inspector
    public AudioSource FastBreathingAudioSource;


    [Header("Play Intervals (in seconds)")]
    public float notificationInterval = 8f;    // e.g., 7–10 seconds
    public float gigglingInterval = 12f;       // e.g., 12–15 seconds

    private float notificationTimer = 0f;
    private float gigglingTimer = 0f;

    //private ShockAndDizzyEffect shockAndDizzyEffect;
    private bool playSounds = true;
    public GameObject playerOfScene1;
    public GameObject Scene2;

    // Boss and its replacement
    public GameObject Boss;
    public GameObject BossReplacement;
    public Camera mainCamera;  // Assign this in Inspector
    [Header("Anxiety Control")]
    public bool isAnxious = false;
    private Coroutine anxietyRoutine;
    public GameObject Breathingball;
    public AudioSource heartbeatAudio; // assign in Inspector
    public AudioClip heartbeatClip;
    public StressEffectController stressEffect;


    void Start()
    {
        Scene2.SetActive(false);
        //var stressEffect = FindObjectOfType<StressEffectController>();

        //shockAndDizzyEffect = FindObjectOfType<ShockAndDizzyEffect>();
        // Start continuous audio
        if (busRideAudio != null)
            busRideAudio.Play();

        if (crowdBackgroundAudio != null)
            crowdBackgroundAudio.Play();

        // Initialize timers
        notificationTimer = notificationInterval;
        gigglingTimer = gigglingInterval;

        Invoke("PlayInsaneFunnyScream", 50f); // Play scream after a short delay
    }

    void Update()
    {
        if (playSounds)
        {
            // Handle repeating notification sound
            if (notificationAudio != null)
            {
                notificationTimer -= Time.deltaTime;
                if (notificationTimer <= 0f)
                {
                    notificationAudio.Play();
                    notificationTimer = notificationInterval;
                }
            }

            // Handle repeating giggling sound
            if (gigglingAudio != null)
            {
                gigglingTimer -= Time.deltaTime;
                if (gigglingTimer <= 0f)
                {
                    gigglingAudio.Play();
                    gigglingTimer = gigglingInterval;
                }
            }
        }
    }

    // Call this function from another script or event to play the scream
    public void PlayInsaneFunnyScream()
    {
        if (insaneFunnyScreamAudio != null)
        {
            insaneFunnyScreamAudio.Play();
        }
        StartCoroutine(DelayedShockEffect());
        playSounds = false;
    }
    private IEnumerator DelayedShockEffect()
    {
        yield return new WaitForSeconds(2f);
        FindObjectOfType<CameraDistortionEffect>().TriggerDistortionEffect();
        // if (shockAndDizzyEffect != null)
        // {
        //     shockAndDizzyEffect.TriggerShockAndDizzy();
        // }
        Invoke("LoadScene2", 3f);
    }
    private void LoadScene2()
    {
        playerOfScene1.SetActive(false);
        Scene2.SetActive(true);
        if (busRideAudio != null)
        {
            busRideAudio.Stop();
        }
        if (crowdBackgroundAudio != null)
        {
            crowdBackgroundAudio.Stop();
        }
    }
    public IEnumerator LoadScene1Again()
    {
        yield return new WaitForSeconds(3f);

        playerOfScene1.SetActive(true);
        Scene2.SetActive(false);
        heartbeatAudio.clip = heartbeatClip;
        heartbeatAudio.loop = true;
        heartbeatAudio.Play();
        mainCamera.GetComponent<HeartbeatEffect>().isPulsing = true;

        if (stressEffect != null)
            stressEffect.isStressed = true;

        if (busRideAudio != null)
            busRideAudio.Play();

        if (crowdBackgroundAudio != null)
            crowdBackgroundAudio.Play();

        playSounds = true;

        // Start anxiety mode
        isAnxious = true;

        FastBreathingAudioSource.clip = fastBreathingAudio;
        FastBreathingAudioSource.loop = true;
        FastBreathingAudioSource.Play();

        // Start anxiety loop
        if (anxietyRoutine != null)
            StopCoroutine(anxietyRoutine);

        //anxietyRoutine = StartCoroutine(AnxietyEffectsLoop());

        Boss.SetActive(false);
        BossReplacement.SetActive(true);
        Breathingball.SetActive(true);
    }


    private IEnumerator AnxietyEffectsLoop()
    {
        var shake = mainCamera.GetComponent<CameraShake>();
        var distortion = FindObjectOfType<CameraDistortionEffect>();

        while (isAnxious)
        {
            // Repeated camera shake
            if (shake != null)
                yield return StartCoroutine(shake.Shake(0.5f, 0.15f)); // short shakes

            // // Optional: Trigger distortion
            // if (distortion != null)
            //     distortion.TriggerDistortionEffect();

            yield return new WaitForSeconds(0.5f); // short delay before next shake
        }

        // Reset any lingering effects if needed
        FastBreathingAudioSource.Stop();
        FastBreathingAudioSource.loop = false;
    }


    public void StopAnxietyEffects() // call kro jb b effect stop krane hon, after the shphere activity
    {
        isAnxious = false;

        if (anxietyRoutine != null)
            StopCoroutine(anxietyRoutine);


        if (stressEffect != null)
            stressEffect.isStressed = false;

        FastBreathingAudioSource.Stop();
        FastBreathingAudioSource.loop = false;
        heartbeatAudio.Stop();
        mainCamera.GetComponent<HeartbeatEffect>().isPulsing = false;
    }

}
