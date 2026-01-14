using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
public class Module3AudioPlayerScript : MonoBehaviour
{
    // DB
    public int totalIndeces = 44;
    private DatabaseReference dbReference;
    /////////////////////////////
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
    // others
    public bool isWarveteranActivityComplete = false;
    private int attempt = 1;

    void Awake()
    {
        currentClipIndex = PlayerPrefs.GetInt("MyIntKey", 0);
    }

    IEnumerator Start()
    {
        Debug.Log("Waiting for Firebase reference...");
        yield return new WaitUntil(() => global_canvas_script.DBRef != null);

        dbReference = global_canvas_script.DBRef;
        Debug.Log(dbReference);
        yield return LoadUserProgress("Module3");



        Maincamera.SetActive(true);
        ConclusionCamera.SetActive(false);
        fastForwardRestrictor = 0;
        if (audioClips.Length > 0)
        {
            StartCoroutine(InitialWait());
        }
    }
    private IEnumerator LoadUserProgress(string moduleName)
    {
        string userId = PlayerPrefs.GetString("userId");

        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User ID not found in PlayerPrefs. Cannot load progress.");
            yield break;
        }

        var moduleProgressRef = dbReference.Child("users")
                                           .Child(userId)
                                           .Child("modules")
                                           .Child(moduleName)
                                           .Child("progress");

        var progressTask = moduleProgressRef.GetValueAsync();
        Debug.Log("Progress task val:" + progressTask);
        yield return new WaitUntil(() => progressTask.IsCompleted);

        if (progressTask.IsFaulted || progressTask.IsCanceled)
        {
            Debug.LogError("Error loading module progress: " + progressTask.Exception);
            yield break;
        }

        if (progressTask.Result.Exists && int.TryParse(progressTask.Result.Value.ToString(), out int savedProgress))
        {
            // Convert percentage back to index
            int estimatedIndex = Mathf.FloorToInt((savedProgress / 100f) * totalIndeces);
            currentClipIndex = Mathf.Clamp(estimatedIndex, 0, audioClips.Length - 1);
            Debug.Log($"Loaded saved progress: {savedProgress}% → starting at clip index {currentClipIndex}");
        }
        else
        {
            currentClipIndex = 0;
            Debug.Log("No progress found, starting from the beginning.");
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
        // testing 
        if (currentClipIndex == 8 && isWarveteranActivityComplete && attempt == 1)
        {
            Debug.Log($"ClipIndex {currentClipIndex} isWarveteranActivityComplete {isWarveteranActivityComplete}");
            attempt++;
            PlayNextAudio();
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
            SceneManager.LoadScene(0);
        }
        UpdateUserProgressInFirebase("Module3");
        StartCoroutine(PlayAudio());
    }
    private void UpdateUserProgressInFirebase(string moduleName)
    {
        string userId = PlayerPrefs.GetString("userId");
        Debug.Log($"User ID: {userId}");

        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User ID not found in PlayerPrefs. Cannot update progress.");
            return;
        }

        float progress = ((float)currentClipIndex / totalIndeces) * 100f;
        int progressRounded = Mathf.Clamp(Mathf.RoundToInt(progress), 0, 100);

        Debug.Log($"Calculated progress for module {moduleName} for user {userId}: {progressRounded}%");

        // Reference to the module's progress
        var moduleProgressRef = dbReference.Child("users")
                                           .Child(userId)
                                           .Child("modules")
                                           .Child(moduleName)
                                           .Child("progress");

        // Get the current progress, then increment
        moduleProgressRef.GetValueAsync().ContinueWithOnMainThread(getTask =>
        {
            if (getTask.IsFaulted || getTask.IsCanceled)
            {
                Debug.LogError("Error fetching current module progress: " + getTask.Exception);
                return;
            }

            int currentProgress = 0;
            if (getTask.Result.Exists && int.TryParse(getTask.Result.Value.ToString(), out int parsedProgress))
            {
                currentProgress = parsedProgress;
            }

            int newProgress = Mathf.Clamp(progressRounded, currentProgress, 100);
            Debug.Log($"Updating module {moduleName} progress from {currentProgress}% to {newProgress}%");

            moduleProgressRef.SetValueAsync(newProgress).ContinueWithOnMainThread(setTask =>
            {
                if (setTask.IsCompleted)
                {
                    Debug.Log("Module progress updated successfully.");
                }
                else
                {
                    Debug.LogError("Error updating module progress: " + setTask.Exception);
                }
            });
        });
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
                //yield return new WaitForSeconds(1.0f);
                //Puzzle.SetActive(true);
            }
            else
            {
                //Puzzle.SetActive(false);
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
                //measuringInstrument.SetActive(true);
            }
            else
            {
                //measuringInstrument.SetActive(false);
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
        if (currentClipIndex != 8) // war veteren logic
        {
            yield return new WaitForSeconds(audioDuration + 2.0f);
            PlayNextAudio();
        }
        // if(currentClipIndex == 8  && isWarveteranActivityComplete)
        // {
        //     Debug.Log($"ClipIndex {currentClipIndex} isWarveteranActivityComplete {isWarveteranActivityComplete}");
        //     PlayNextAudio();
        // }
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
