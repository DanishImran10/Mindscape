using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Module4AudioPlayerScript : MonoBehaviour
{
    public int currentClipIndex = 0;
    public Animator charAnim;
    //cameras
    public GameObject Maincamera;
    // public Animator shadowPlayerAnim;
    public GameObject directionalLight;
    private int fastForwardRestrictor;
    //    public TextMeshProUGUI[] fffTexts;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    private float audioDuration;
    private bool prevIndexStartPt = false;

    // GameObjects
    public GameObject diagnosticAndStatisticalManual;
    public GameObject sittingAndHoldingHead;
    public GameObject walkingLeft_right;
    public GameObject hospitalBed;
    public GameObject framedPhoto;
    public GameObject Burglary;
    public GameObject naturalDisaster;
    public GameObject multipleShadows;
    public GameObject Anger;
    public GameObject legsFoldedToTheirChest;
    public GameObject splitScreen;
    

    void Start()
    {
        Maincamera.SetActive(true);
        fastForwardRestrictor = 0;
        if (audioClips.Length > 0)
        {
            StartCoroutine(InitialWait());
        }
    }
    void Update()
    {
        // if(Input.GetKey(KeyCode.S))
        // {
        //     Debug.Log("S pressed");
        // }
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
            if (currentClipIndex == 2)
            {
                diagnosticAndStatisticalManual.SetActive(true);
            }
            else
            {
                diagnosticAndStatisticalManual.SetActive(false);
            }
            if(currentClipIndex == 4)
            {
                sittingAndHoldingHead.SetActive(true);
            }
            else
            {
                sittingAndHoldingHead.SetActive(false);
            }
            if(currentClipIndex == 8)
            {
                walkingLeft_right.SetActive(true);
            }
            else
            {
                walkingLeft_right.SetActive(false);
            }
            if(currentClipIndex == 9)
            {
                hospitalBed.SetActive(true);
            }
            else
            {
                hospitalBed.SetActive(false);
            }
            if(currentClipIndex == 10)
            {
                framedPhoto.SetActive(true);
            }
            else
            {
                framedPhoto.SetActive(false);
            }
            if(currentClipIndex == 13)
            {
                Burglary.SetActive(true);
            }
            else
            {
                Burglary.SetActive(false);
            }
            if(currentClipIndex == 16)
            {
                naturalDisaster.SetActive(true);
            }
            else
            {
                naturalDisaster.SetActive(false);
            }
            if(currentClipIndex == 19)
            {
                multipleShadows.SetActive(true);
            }
            else
            {
                multipleShadows.SetActive(false);
            }
            if(currentClipIndex == 20)
            {
                Anger.SetActive(true);
            }
            else
            {
                Anger.SetActive(false);
            }
            if(currentClipIndex == 21)
            {
                legsFoldedToTheirChest.SetActive(true);
            }
            else
            {
                legsFoldedToTheirChest.SetActive(false);
            }
            if(currentClipIndex == 23)
            {
                splitScreen.SetActive(true);
            }
            else
            {
                splitScreen.SetActive(false);
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
        // if (currentClipIndex == 4) // for extra delays
        // {
        //     audioDuration += 3.0f;
        // }
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
