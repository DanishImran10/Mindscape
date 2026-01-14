using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
public class Module2AudioPlayeScript : MonoBehaviour
{
    // DB
    public int totalIndeces = 44;
    private DatabaseReference dbReference;
    // nav
    private bool prevIndexStartPt = false;
    // rain anim
    public GameObject rain;
    public Animator dejectAnim;

    // audios
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public int currentClipIndex = 0;
    private float audioDuration;
    // char
    public Animator charAnim;

    //cameras
    public GameObject Maincamera;
    public GameObject ConclusionCamera;

    public GameObject directionalLight;
    // others
    private int fastForwardRestrictor;
    public GameObject ButtonPanel;
    public GameObject Symptoms;
    public GameObject FlashBack;
    public GameObject FlashBackWithCar;
    public GameObject FlashBackWithCar2;
    public GameObject uneasiness;
    public GameObject people;
    public GameObject socialSetting;
    public GameObject movingWords;
    public GameObject militaryEscape;
    public GameObject invisibleWall;
    public GameObject isolation;
    public GameObject mirrorChar;
    public GameObject confusion;
    public GameObject emotionalNumbness;
    public GameObject standingAtWindow;
    public GameObject bodyStuck;
    public GameObject readingBook;
    public GameObject EGC;

    IEnumerator Start()
    {
        Debug.Log("Waiting for Firebase reference...");
        yield return new WaitUntil(() => global_canvas_script.DBRef != null);

        dbReference = global_canvas_script.DBRef;
        Debug.Log(dbReference);
        yield return LoadUserProgress("Module2");


        Maincamera.SetActive(true);
        ConclusionCamera.SetActive(false);
        fastForwardRestrictor = 0;
        ButtonPanel.SetActive(true);
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
    }
    IEnumerator InitialWait()
    {
        yield return new WaitForSeconds(1.0f);
        PlayAudio();
    }
    public void PlayNextAudio()
    {
        ButtonPanel.SetActive(true);
        currentClipIndex++;
        if (currentClipIndex >= audioClips.Length)
        {
            SceneManager.LoadScene(5); // activity scene
                                       //    currentClipIndex = 0;
        }
        UpdateUserProgressInFirebase("Module2");
        PlayAudio();
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
    private void PlayAudio()
    {
        if (audioSource != null && audioClips[currentClipIndex] != null)
        {
            audioSource.clip = audioClips[currentClipIndex];
            //conditions apply here .....
            if (currentClipIndex == 4 || currentClipIndex == 5 || currentClipIndex == 6 || currentClipIndex == 7)
            {
                Symptoms.SetActive(true);
            }
            else
            {
                Symptoms.SetActive(false);
            }
            if (currentClipIndex == 13)
            {
                FlashBack.SetActive(true);
                StartCoroutine(rainDelay());
            }
            else
            {
                FlashBack.SetActive(false);
            }
            if (currentClipIndex == 16 /*|| currentClipIndex == 17*/)
            {
                FlashBackWithCar.SetActive(true);
            }
            else
            {
                FlashBackWithCar.SetActive(false);
            }
            if (currentClipIndex == 17 /*|| currentClipIndex == 17*/)
            {
                FlashBackWithCar2.SetActive(true);
            }
            else
            {
                FlashBackWithCar2.SetActive(false);
            }
            if (currentClipIndex == 19)
            {
                uneasiness.SetActive(true);
            }
            else
            {
                uneasiness.SetActive(false);
            }
            // if(currentClipIndex == 22)
            // {
            //     people.SetActive(true);
            // }
            // else
            // {
            //     people.SetActive(false);
            // }
            if (currentClipIndex == 22)
            {
                StartCoroutine(peopleClipDelay());
                //    people.SetActive(true);
            }
            else
            {
                people.SetActive(false);
            }
            if (currentClipIndex == 23)
            {
                socialSetting.SetActive(true);
            }
            else
            {
                socialSetting.SetActive(false);
            }
            if (currentClipIndex == 24)
            {
                movingWords.SetActive(true); // check nextButtonShow() for what you're looking for
            }
            else
            {
                movingWords.SetActive(false);
            }
            if (currentClipIndex == 25)
            {
                //militaryEscape.SetActive(true);
            }
            else
            {
                //militaryEscape.SetActive(false);
            }
            if (currentClipIndex == 26)
            {
                //StartCoroutine(wallDelay());
            }
            else
            {
                //invisibleWall.SetActive(false);
            }
            if (currentClipIndex == 27)
            {
                //StartCoroutine(isolationDelay());
            }
            else
            {
                //isolation.SetActive(false);
            }
            if (currentClipIndex == 31)
            {
                mirrorChar.SetActive(true);
            }
            else
            {
                mirrorChar.SetActive(false);
            }
            if (currentClipIndex == 34)
            {
                confusion.SetActive(true);
            }
            else
            {
                confusion.SetActive(false);
            }
            if (currentClipIndex == 36)
            {
                //emotionalNumbness.SetActive(true);
            }
            else
            {
                //emotionalNumbness.SetActive(false);
            }
            if (currentClipIndex == 37)
            {
                //StartCoroutine(windowDelay());
            }
            else
            {
                //standingAtWindow.SetActive(false);
            }
            if (currentClipIndex == 38)
            {
                StartCoroutine(readingDelay());
            }
            else
            {
                readingBook.SetActive(false);
            }
            if (currentClipIndex == 39)
            {
                StartCoroutine(bodyDelay());
            }
            else
            {
                bodyStuck.SetActive(false);
            }
            if (currentClipIndex == 40)
            {
                EGC.SetActive(true);
            }
            else
            {
                EGC.SetActive(false);
            }

            audioDuration = audioClips[currentClipIndex].length;
            StartCoroutine(nextButtonShow()); // shows next button after an audio end
            // charAnim.SetBool("talk", true);
            audioSource.Play();
        }
    }
    IEnumerator peopleClipDelay()
    {
        yield return new WaitForSeconds(4.5f);
        people.SetActive(true);
    }
    IEnumerator rainDelay()
    {
        yield return new WaitForSeconds(1.0f);
        rain.SetActive(true);
        dejectAnim.SetBool("deject", true);
    }
    IEnumerator wallDelay()
    {
        yield return new WaitForSeconds(3f);
        invisibleWall.SetActive(true);
    }
    IEnumerator isolationDelay()
    {
        yield return new WaitForSeconds(5f);
        isolation.SetActive(true);
    }
    IEnumerator windowDelay()
    {
        yield return new WaitForSeconds(7f);
        standingAtWindow.SetActive(true);
    }
    IEnumerator readingDelay()
    {
        yield return new WaitForSeconds(5f);
        readingBook.SetActive(true);
    }
    IEnumerator bodyDelay()
    {
        yield return new WaitForSeconds(2.75f);
        bodyStuck.SetActive(true);
    }
    IEnumerator nextButtonShow()
    {
        // if(currentClipIndex == 4)
        // {
        //     audioDuration += 3.0f;
        // }
        if (currentClipIndex == 4 || currentClipIndex == 5 || currentClipIndex == 6 || currentClipIndex == 7)
        {
            yield return new WaitForSeconds(audioDuration + 0.2f);
        }
        else if (currentClipIndex == 16)
        {
            yield return new WaitForSeconds(audioDuration/* + 1.2f*/);
        }
        else if (currentClipIndex == 20)
        {
            yield return new WaitForSeconds(audioDuration + 2f);
        }
        else if (currentClipIndex == 22)
        {
            yield return new WaitForSeconds(audioDuration + 0.5f);
        }
        else if (currentClipIndex == 23)
        {
            yield return new WaitForSeconds(audioDuration + 2.0f);
        }
        else if (currentClipIndex == 24)
        {
            yield return new WaitForSeconds(audioDuration + 5.5f);
        }
        else if (currentClipIndex == 37)
        {
            yield return new WaitForSeconds(audioDuration + 2f);
        }
        // else if(currentClipIndex == 32)
        // {
        //     yield return new WaitForSeconds(audioDuration + 2f);
        // }
        else
        {
            yield return new WaitForSeconds(audioDuration/* + 2.0f */);
        }
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
