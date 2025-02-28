using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
// audioDurations = audioClips[i].length;
public class AudioPlayer : MonoBehaviour
{
    public int currentClipIndex = 0;
    public Animator charAnim;
    //cameras
    public GameObject Maincamera;
    public GameObject ConclusionCamera;
    // public Animator shadowPlayerAnim;
    public GameObject directionalLight;
    private int fastForwardRestrictor;

    public GameObject physicalHarm;
    public GameObject Murder;
    public GameObject emotionalDistress;
    public GameObject NaturalDisaster;
    public GameObject Survival_Instincts;
    public GameObject fff;
    public GameObject brainCalmState;
    public GameObject Nervous;
    public GameObject Shadow;
    public GameObject chronicPain;
    public GameObject emotionalNumbness;
    public GameObject skeleton;
    public GameObject earth;
    public GameObject population;
    public GameObject seventy;
    public GameObject fivePtSix;
    public GameObject pushingRock;
    public GameObject hug;
    public GameObject SpiralingDNA;

//    public TextMeshProUGUI[] fffTexts;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    private float audioDuration;
    private bool prevIndexStartPt = false;

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
        if(audioSource.isPlaying)
        {
            charAnim.SetBool("talk", true);
        }
        else{
            charAnim.SetBool("talk", false);
        }
    }
    IEnumerator InitialWait()
    {
        yield return new WaitForSeconds(1.0f);
        PlayAudio();  
    }
    public void PlayNextAudio()
    {
        currentClipIndex++;
        if (currentClipIndex >= audioClips.Length)
        {
            SceneManager.LoadScene("Start Scene");
        }

        PlayAudio();
    }
    public void PlayAudio()
    {
        if (audioSource != null && audioClips[currentClipIndex] != null)
        {
            audioSource.clip = audioClips[currentClipIndex];
            if(currentClipIndex == 2)
            {
                physicalHarm.SetActive(true);
            }
            else{
                physicalHarm.SetActive(false);
            }
            // if(currentClipIndex == 3) // combined with 11 and 12
            // {
            //     emotionalDistress.SetActive(true);
            // }
            // else{
            //     emotionalDistress.SetActive(false);
            // }
            if(currentClipIndex == 4)
            {
                NaturalDisaster.SetActive(true);
            }
            else{
                NaturalDisaster.SetActive(false);
            }
            if(currentClipIndex == 5)
            {
                Murder.SetActive(true);
            }
            else{
                Murder.SetActive(false);
            }
            if(currentClipIndex == 7)
            {
                StartCoroutine(SurvialAnimDelay());
            //    Survival_Instincts.SetActive(true);
            }
            else{
                Survival_Instincts.SetActive(false);
            }
            if(currentClipIndex == 8)
            {
                fff.SetActive(true);
                // for (int i = 0; i < 3; i++)
                //     fffTexts[i].gameObject.SetActive(true);
            }
            else{
                fff.SetActive(false);
                // for (int i = 0; i < 3; i++)
                //     fffTexts[i].gameObject.SetActive(false);
            }
            if(currentClipIndex == 10)
            {
               brainCalmState.SetActive(true);
            }
            else{
                brainCalmState.SetActive(false);
            }
            if(currentClipIndex == 11 /*|| currentClipIndex == 12*/ || currentClipIndex == 3)
            {
               emotionalDistress.SetActive(true);
            }
            else{
                emotionalDistress.SetActive(false);
            }
            if(currentClipIndex == 13 || currentClipIndex == 12 || currentClipIndex == 14)
            {
               Nervous.SetActive(true);
            }
            else{
                Nervous.SetActive(false);
            }
            if(currentClipIndex == 14)
            {
                StartCoroutine(shadowWait());
               //Shadow.SetActive(true);
            }
            else{
                if(currentClipIndex != 17)
                {
                directionalLight.SetActive(true);
                }
                Shadow.SetActive(false);
            }
            if(currentClipIndex == 16)
            {
                StartCoroutine(chronicPainWait());
            }
            else
            {
                chronicPain.SetActive(false);
            }
            if(currentClipIndex == 17)
            {
                emotionalNumbness.SetActive(true);
                directionalLight.SetActive(false);
            }
            else
            {
                emotionalNumbness.SetActive(false);
            }
            if(currentClipIndex == 18)
            {
                skeleton.SetActive(true);
            }
            else
            {
                skeleton.SetActive(false);
            }
            if(currentClipIndex == 19)
            {
                StartCoroutine(earthPop());
            } // no else bcz these are dissabled in coroutine..!!
            if(currentClipIndex == 20)
            {
                StartCoroutine(numbersPop());
            } // no else bcz these are dissabled in coroutine..!!
            if(currentClipIndex == 21)
            {
                StartCoroutine(pushAnimWaitTime());
            }
            else
            {
                pushingRock.SetActive(false);
            }
            if(currentClipIndex == 22)
            {
                hug.SetActive(true);
            }
            else
            {
                hug.SetActive(false);
            }
            if(currentClipIndex == 23)
            {
                SpiralingDNA.SetActive(true);
            }
            else
            {
                SpiralingDNA.SetActive(false);
            }
            if(currentClipIndex == 25)
            {
                Maincamera.SetActive(false);
                ConclusionCamera.SetActive(true);
            }
            audioDuration = audioClips[currentClipIndex].length;
            StartCoroutine(nextButtonShow()); // shows next button after an audio end
            // charAnim.SetBool("talk", true);
            audioSource.Play();
        }
    }
    IEnumerator pushAnimWaitTime()
    {
        yield return new WaitForSeconds(5.0f);
        pushingRock.SetActive(true);
    }
    IEnumerator numbersPop()
    {
        yield return new WaitForSeconds(5.0f);
        seventy.SetActive(true);
        yield return new WaitForSeconds(7.0f);
        seventy.SetActive(false);
        fivePtSix.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        fivePtSix.SetActive(false);
    } 
    IEnumerator earthPop()
    {
        yield return new WaitForSeconds(5.0f);
        earth.SetActive(true);
        yield return new WaitForSeconds(6.0f);
        earth.SetActive(false);
        population.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        population.SetActive(false);
    } 
    IEnumerator chronicPainWait()
    {
        yield return new WaitForSeconds(5.3f);
        chronicPain.SetActive(true);
    }
    IEnumerator shadowWait()
    {
        yield return new WaitForSeconds(6.5f);
        Shadow.SetActive(true);
        // shadowPlayerAnim.SetBool("crawl", true);
        directionalLight.SetActive(false);
    }
    IEnumerator nextButtonShow()
    {
        if(currentClipIndex == 4)
        {
            audioDuration += 3.0f;
        }
        //charAnim.SetBool("talk", false);
        yield return new WaitForSeconds(audioDuration + 2.0f);
        // charAnim.SetBool("talk", false);
        // yield return new WaitForSeconds(2.0f);
        //charAnim.SetBool("talk", false);
        PlayNextAudio();
        // ButtonPanel.SetActive(true);
        // Debug.Log("nextButtonShow() called");
    }
    IEnumerator SurvialAnimDelay()
    {
        yield return new WaitForSeconds(1.5f);
        Survival_Instincts.SetActive(true);
    }
    public void FastForwardScene(float speedMultiplier)
    {
        if(fastForwardRestrictor <= 3)
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

