using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using Firebase.Auth;
using System.Collections.Generic;
using UnityEngine.UI;

public class global_canvas_script : MonoBehaviour
{
    [Header("Progress Bars")]
    public RectTransform module1Fill;
    public RectTransform module2Fill;
    public RectTransform module3Fill;
    public GameObject feedbackButton; // multiple feedback restriction
    public GameObject resumeOrRestartPanel;
    private string selectedModuleName = "";
    private int selectedSceneIndex = -1;
    public GameObject loadingScreen;
    public GameObject moduleList;
    public GameObject Instructions;
    public GameObject StartPanel;
    private DatabaseReference dbReference;
    public static DatabaseReference DBRef { get; private set; }
    void Start()  // using previously signed user
    {
        loadingScreen.SetActive(true);
        Invoke("removeLoadingScreen", 2f);
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                FirebaseAuth auth = FirebaseAuth.DefaultInstance;

                // Check if already signed in
                if (auth.CurrentUser != null)
                {
                    string userId = auth.CurrentUser.UserId;
                    PlayerPrefs.SetString("userId", userId);

                    InitializeDatabase(userId);
                }
                else
                {
                    auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(authTask =>
                    {
                        if (!authTask.IsFaulted && !authTask.IsCanceled)
                        {
                            FirebaseUser user = authTask.Result.User;
                            string userId = user.UserId;
                            PlayerPrefs.SetString("userId", userId);

                            InitializeDatabase(userId);
                        }
                        else
                        {
                            Debug.LogError("Firebase anonymous sign-in failed.");
                        }
                    });
                }
            }
            else
            {
                Debug.LogError("Could not resolve Firebase dependencies.");
            }
        });
    }
    void removeLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }
    private void InitializeDatabase(string userId)
    {
        var instance = FirebaseDatabase.GetInstance(
            FirebaseApp.DefaultInstance,
            "https://mindscape0820-default-rtdb.firebaseio.com/"
        );

        DBRef = instance.RootReference;
        dbReference = DBRef;

        DatabaseReference userModulesRef = DBRef.Child("users").Child(userId).Child("modules");

        userModulesRef.GetValueAsync().ContinueWithOnMainThread(getTask =>
        {
            if (getTask.IsCompleted)
            {
                DataSnapshot snapshot = getTask.Result;

                if (!snapshot.Exists)
                {
                    Dictionary<string, object> modulesData = new Dictionary<string, object>
                    {
                    { "Module1", new Dictionary<string, object> { { "progress", 0 } } },
                    { "Module2", new Dictionary<string, object> { { "progress", 0 } } },
                    { "Module3", new Dictionary<string, object> { { "progress", 0 } } }
                    };

                    userModulesRef.SetValueAsync(modulesData);
                }
            }
            else
            {
                Debug.LogError("Error checking/initializing module data: " + getTask.Exception);
            }
        });

        // Check if feedback exists for the current user
        DBRef.Child("users").Child(userId).Child("feedback").GetValueAsync().ContinueWithOnMainThread(feedbackTask =>
        {
            if (feedbackTask.IsCompleted)
            {
                DataSnapshot feedbackSnapshot = feedbackTask.Result;
                if (feedbackSnapshot.Exists)
                {
                    // Feedback already given, hide the button
                    feedbackButton.SetActive(false);
                }
                else
                {
                    // No feedback yet, show the button
                    feedbackButton.SetActive(true);
                }
            }
            else
            {
                Debug.LogError("Failed to check feedback status: " + feedbackTask.Exception);
                feedbackButton.SetActive(true); // Show by default if check fails
            }
        });
    }
    public void OnStartButtonClick()
    {
        moduleList.SetActive(true);
        loadingScreen.SetActive(false);
        Instructions.SetActive(false);
        StartPanel.SetActive(false);

        UpdateProgressBars();
    }
    public void OnInstructionButtonClick()
    {
        Instructions.SetActive(true);
        moduleList.SetActive(false);
        loadingScreen.SetActive(false);
        StartPanel.SetActive(false);
    }
    public void OnCloseClick()
    {
        CloseAllpanels();
    }
    private void CloseAllpanels()
    {
        moduleList.SetActive(false);
        Instructions.SetActive(false);
        StartPanel.SetActive(true);
        loadingScreen.SetActive(false);
        resumeOrRestartPanel.SetActive(false);
    }
    public void OnModule1Click()
    {
        HandleModuleClick("Module1", 1);
        // StartPanel.SetActive(false);
        // moduleList.SetActive(false);
        // Instructions.SetActive(false);
        // selectedModuleName = "Module1";
        // selectedSceneIndex = 1;
        // resumeOrRestartPanel.SetActive(true);
        //StartCoroutine(IncrementModuleProgressAndLoadScene("Module1", 1));
    }

    private IEnumerator IncrementModuleProgressAndLoadScene(string moduleName, int sceneIndex)
    {
        if (string.IsNullOrEmpty(moduleName))
        {
            Debug.LogError("Module name is null or empty!");
            yield break;
        }

        string basePath = $"modules/{moduleName}/startCount";
        Debug.Log($"Reading startCount from path: {basePath}");

        var getTask = dbReference.Child("modules").Child(moduleName).Child("startCount").GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted);

        if (getTask.Exception != null)
        {
            Debug.LogError($"Error reading startCount for {moduleName}: {getTask.Exception}");
            yield break;
        }

        int currentCount = 0;
        if (getTask.Result.Exists && int.TryParse(getTask.Result.Value.ToString(), out int val))
        {
            currentCount = val;
        }

        int newCount = currentCount + 1;
        Debug.Log($"Updating startCount at path {basePath} to: {newCount}");

        var setTask = dbReference.Child("modules").Child(moduleName).Child("startCount").SetValueAsync(newCount);
        yield return new WaitUntil(() => setTask.IsCompleted);

        if (setTask.Exception != null)
        {
            Debug.LogError($"Error updating startCount for {moduleName}: {setTask.Exception}");
            yield break;
        }

        Debug.Log($"Successfully updated startCount for {moduleName} to {newCount}");

        // Proceed to load the scene
        StartCoroutine(loadScene(sceneIndex));
    }
    public void OnModule2Click()
    {
        HandleModuleClick("Module2", 2);
        // StartPanel.SetActive(false);
        // moduleList.SetActive(false);
        // Instructions.SetActive(false);

        // selectedModuleName = "Module2";
        // selectedSceneIndex = 2;
        // resumeOrRestartPanel.SetActive(true);
        //StartCoroutine(IncrementModuleProgressAndLoadScene("Module2", 2));
    }
    public void OnModule3Click()
    {
        HandleModuleClick("Module3", 3);
        // StartPanel.SetActive(false);
        // moduleList.SetActive(false);
        // Instructions.SetActive(false);

        // selectedModuleName = "Module3";
        // selectedSceneIndex = 3;
        // resumeOrRestartPanel.SetActive(true);
        //StartCoroutine(IncrementModuleProgressAndLoadScene("Module3", 3));
    }
    private void HandleModuleClick(string moduleName, int sceneIndex)
    {
        StartPanel.SetActive(false);
        moduleList.SetActive(false);
        Instructions.SetActive(false);

        selectedModuleName = moduleName;
        selectedSceneIndex = sceneIndex;

        string userId = PlayerPrefs.GetString("userId");
        DBRef.Child("users").Child(userId).Child("modules").Child(moduleName).Child("progress")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    int progress = 0;
                    int.TryParse(task.Result.Value.ToString(), out progress);

                    if (progress == 0)
                    {
                        // Directly start scene if progress is 0
                        StartCoroutine(IncrementModuleProgressAndLoadScene(moduleName, sceneIndex));
                    }
                    else
                    {
                        // Show resume/restart options
                        resumeOrRestartPanel.SetActive(true);
                    }
                }
                else
                {
                    Debug.LogWarning("Could not fetch module progress. Defaulting to show resume panel.");
                    resumeOrRestartPanel.SetActive(true);
                }
            });
    }

    public void OnModule4Click()
    {
        StartPanel.SetActive(false);
        moduleList.SetActive(false);
        loadingScreen.SetActive(true);
        Instructions.SetActive(false);
        StartCoroutine(IncrementModuleProgressAndLoadScene("Vuforia", 4));
    }
    public void OnSimulationClick()
    {
        StartPanel.SetActive(false);
        moduleList.SetActive(false);
        loadingScreen.SetActive(true);
        Instructions.SetActive(false);
        StartCoroutine(IncrementModuleProgressAndLoadScene("Simulation", 7));
    }
    IEnumerator loadScene(int n)
    {
        moduleList.SetActive(false);
        loadingScreen.SetActive(false);
        Instructions.SetActive(false);
        StartPanel.SetActive(false);

        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(n);
    }
    public void OnPlayButtonClick()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
    public void OnWarVeteranClick()
    {
        StartPanel.SetActive(false);
        moduleList.SetActive(false);
        loadingScreen.SetActive(true);
        Instructions.SetActive(false);
        SceneManager.LoadScene(9); // separate scene bna diya hai
    }

    public void OnMod2ActivityClick()
    {
        StartPanel.SetActive(false);
        moduleList.SetActive(false);
        loadingScreen.SetActive(true);
        Instructions.SetActive(false);
        SceneManager.LoadScene(5);
    }
    public void OnMod3ActivityClick()
    {
        StartPanel.SetActive(false);
        moduleList.SetActive(false);
        loadingScreen.SetActive(true);
        Instructions.SetActive(false);
        SceneManager.LoadScene(6);
    }
    public void OnFeedbacClick()
    {
        StartPanel.SetActive(false);
        moduleList.SetActive(false);
        loadingScreen.SetActive(true);
        Instructions.SetActive(false);
        SceneManager.LoadScene(8);
    }

    public void OnResumeClick()
    {
        StartPanel.SetActive(false);
        moduleList.SetActive(false);
        loadingScreen.SetActive(true);
        Instructions.SetActive(false);
        if (!string.IsNullOrEmpty(selectedModuleName) && selectedSceneIndex >= 0)
        {
            StartCoroutine(IncrementModuleProgressAndLoadScene(selectedModuleName, selectedSceneIndex));
        }
        resumeOrRestartPanel.SetActive(false);
    }

    public void OnRestartClick()
    {
        StartPanel.SetActive(false);
        moduleList.SetActive(false);
        loadingScreen.SetActive(true);
        Instructions.SetActive(false);
        if (!string.IsNullOrEmpty(selectedModuleName) && selectedSceneIndex >= 0)
        {
            string userId = PlayerPrefs.GetString("userId");

            DBRef.Child("users").Child(userId).Child("modules").Child(selectedModuleName).Child("progress")
                .SetValueAsync(0).ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        Debug.Log($"{selectedModuleName} progress reset to 0.");
                        StartCoroutine(loadScene(selectedSceneIndex));
                    }
                    else
                    {
                        Debug.LogError("Failed to reset progress: " + task.Exception);
                    }
                });
        }
        resumeOrRestartPanel.SetActive(false);
    }



    // for progress loading bars
    void UpdateProgressBars()
    {
        string userId = PlayerPrefs.GetString("userId");

        DatabaseReference userModulesRef = DBRef.Child("users").Child(userId).Child("modules");

        userModulesRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    UpdateBar(snapshot, "Module1", module1Fill);
                    UpdateBar(snapshot, "Module2", module2Fill);
                    UpdateBar(snapshot, "Module3", module3Fill);
                }
            }
        });
    }

    void UpdateBar(DataSnapshot snapshot, string moduleName, RectTransform fillBar)
    {
        float progress = 0f;

        if (snapshot.HasChild(moduleName) &&
            snapshot.Child(moduleName).HasChild("progress") &&
            float.TryParse(snapshot.Child(moduleName).Child("progress").Value.ToString(), out float value))
        {
            progress = Mathf.Clamp01(value / 100f); // store as 0-100 but normalize to 0-1
        }

        float fullWidth = 100f; // ✅ Keep width max 100 pixels
        StartCoroutine(AnimateBar(fillBar, progress * fullWidth));
    }

    IEnumerator AnimateBar(RectTransform bar, float targetWidth)
    {
        float duration = 0.5f;
        float startWidth = bar.sizeDelta.x;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newWidth = Mathf.Lerp(startWidth, targetWidth, elapsed / duration);
            bar.sizeDelta = new Vector2(newWidth, bar.sizeDelta.y);
            yield return null;
        }

        bar.sizeDelta = new Vector2(targetWidth, bar.sizeDelta.y);
    }

}
