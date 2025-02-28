using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Module3AudioPlayerScript : MonoBehaviour
{
    public int currentClipIndex = 0;
    public Animator charAnim;
    //cameras
    public GameObject Maincamera;
    public GameObject ConclusionCamera;
    // public Animator shadowPlayerAnim;
    public GameObject directionalLight;
    private int fastForwardRestrictor;
    //    public TextMeshProUGUI[] fffTexts;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    private float audioDuration;
    private bool prevIndexStartPt = false;

    // GameObjects
    public GameObject PTSD_triggers;
    public GameObject fastHeartbeat;
    public GameObject brokenItems;
    public GameObject Puzzle;
    public GameObject Alarm;
    public GameObject Silhouettes;
    public GameObject yellowSmoke;
    public GameObject blackSetOfLines;
    public GameObject _imNotSafe_text;
    public GameObject warVeteran;
    public GameObject measuringInstrument;

    void Start()
    {
        Maincamera.SetActive(true);
        ConclusionCamera.SetActive(false);
        fastForwardRestrictor = 0;
        if (audioClips.Length > 0)
        {
            StartCoroutine(InitialWait());
        }
    }
    void Update()
    {
        if (audioSource.isPlaying)
        {
            charAnim.SetBool("talk", true);
        }
        else
        {
            charAnim.SetBool("talk", false);
        }
    }
    IEnumerator InitialWait()
    {
        Debug.Log("in Initial wait");
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(PlayAudio());
    }
    public void PlayNextAudio()
    {
        currentClipIndex++;
        if (currentClipIndex >= audioClips.Length)
        {
            SceneManager.LoadScene("Start Scene");
        }

        StartCoroutine(PlayAudio());
    }
    IEnumerator PlayAudio()
    {
        Debug.Log($"Playing audio clip at index: {currentClipIndex}");
        if (audioSource != null && audioClips[currentClipIndex] != null)
        {
            audioSource.clip = audioClips[currentClipIndex];
            if (currentClipIndex == 0)
            {
                yield return new WaitForSeconds(1.0f);
                PTSD_triggers.SetActive(true);
            }
            else
            {
                PTSD_triggers.SetActive(false);
            }
            if (currentClipIndex == 1)
            {
                fastHeartbeat.SetActive(true);
            }
            else
            {
                fastHeartbeat.SetActive(false);
            }
            if (currentClipIndex == 2)
            {
                brokenItems.SetActive(true);
            }
            else
            {
                brokenItems.SetActive(false);
            }
            if (currentClipIndex == 3)
            {
                yield return new WaitForSeconds(1.0f);
                Puzzle.SetActive(true);
            }
            else
            {
                Puzzle.SetActive(false);
            }
            if (currentClipIndex == 4)
            {
                Alarm.SetActive(true);
            }
            else
            {
                Alarm.SetActive(false);
            }
            if (currentClipIndex == 5)
            {
                yield return new WaitForSeconds(6.0f);
                Silhouettes.SetActive(true);
            }
            else
            {
                Silhouettes.SetActive(false);
            }
            if (currentClipIndex == 6)
            {
                yellowSmoke.SetActive(true);
            }
            else
            {
                yellowSmoke.SetActive(false);
            }
            if (currentClipIndex == 7)
            {
                _imNotSafe_text.SetActive(true);
            }
            else
            {
                _imNotSafe_text.SetActive(false);
            }
            if (currentClipIndex == 8)
            {
                Maincamera.SetActive(false);
                warVeteran.SetActive(true);
            }
            else
            {
                Maincamera.SetActive(true);
                warVeteran.SetActive(false);
            }
            if (currentClipIndex == 9)
            {
                measuringInstrument.SetActive(true);
            }
            else
            {
                measuringInstrument.SetActive(false);
            }

            audioDuration = audioClips[currentClipIndex].length;
            StartCoroutine(nextButtonShow());
            audioSource.Play();
            yield return null;
        }
        else
        {
            Debug.Log("null returned");
            yield break;
        }
    }


    IEnumerator nextButtonShow()
    {
        if (currentClipIndex == 4)
        {
            audioDuration += 3.0f;
        }
        yield return new WaitForSeconds(audioDuration + 2.0f);
        PlayNextAudio();
    }
    public void FastForwardScene(float speedMultiplier)
    {
        if (fastForwardRestrictor <= 3)
        {
            fastForwardRestrictor += 1;
            Time.timeScale += speedMultiplier;
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.pitch += speedMultiplier;
            }
        }
    }

    public void ResetTimeScale()
    {
        fastForwardRestrictor = 0;
        Time.timeScale = 1f;
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.pitch = 1f;
        }
    }

    public void pause()
    {
        Time.timeScale = 0f;
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.pitch = 0f;
        }
    }
    // public void ReverseScene(float speedMultiplier)
    // {
    //     if(fastForwardRestrictor <= 1)
    //     {
    //     fastForwardRestrictor += 1;
    //     Time.timeScale -= speedMultiplier;
    //     AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
    //     foreach (AudioSource audioSource in audioSources)
    //     {
    //     audioSource.pitch -= speedMultiplier;
    //     }
    //     }
    // }

    public void GoToNextIndex()
    {
        StopAllCoroutines();
        currentClipIndex += 1;
        PlayAudio();
    }
    public void GoToPrevIndex()
    {
        StopAllCoroutines();
        if (prevIndexStartPt)
        {
            currentClipIndex -= 1;
            PlayAudio();
        }
        else
        {
            prevIndexStartPt = true;
            PlayAudio();
            StartCoroutine(prevIndexStartPtFalse());
        }
    }
    IEnumerator prevIndexStartPtFalse()
    {
        yield return new WaitForSeconds(2.0f);
        prevIndexStartPt = false;
        PlayAudio();
    }
}
