using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseScript : MonoBehaviour
{
    public GameObject pauseButton;
    public GameObject resumeButton;
    private bool isPaused = false;

    void Start()
    {
        resumeButton.SetActive(false);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pauseButton.SetActive(!isPaused);
        resumeButton.SetActive(isPaused);
        if (isPaused)
        {
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.pitch = 0f;
            }
        }
        else
        {
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.pitch = 1f;
            }
        }
    }
    public void OnHomeClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); // currectly 0 is the home scene index
    }
}
