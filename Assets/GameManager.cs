using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public int currentClipIndex = 0;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    private float audioDuration;
    public bool canClick = true;
    void Start()
    {

    }

    void Update()
    {

    }
    public void PlayAudio(int currentClipIndex)
    {
        if (audioSource != null && audioClips[currentClipIndex] != null)
        {
            audioSource.clip = audioClips[currentClipIndex];

            audioDuration = audioClips[currentClipIndex].length;
            StartCoroutine(nextButtonShow());
            audioSource.Play();
        }
    }
    public void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
    IEnumerator nextButtonShow()  // ye function use ni krna iss mein, lkn issko use kr k access deni next button click krne ki 
    {
        yield return new WaitForSeconds(audioDuration /* + 2.0f */ );
        canClick = true;
        //PlayNextAudio();
    }
    // public void PlayNextAudio()
    // {
    //     currentClipIndex++;
    //     if (currentClipIndex >= audioClips.Length)
    //     {
    //         SceneManager.LoadScene(0);
    //     }

    //     PlayAudio();
    // }
}
