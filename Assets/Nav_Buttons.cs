using System.Collections;
using UnityEngine;

public class Nav_Buttons : MonoBehaviour
{
    private bool prevIndexStartPt = false;
    private AudioPlayer gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<AudioPlayer>();
    }
    public void GoToNextIndex()
{
    gameManager.StopAllCoroutines();
    gameManager.currentClipIndex += 1;
    gameManager.PlayAudio();
}
    public void GoToPrevIndex()
{
    gameManager.StopAllCoroutines();
    if (prevIndexStartPt)
    {
        gameManager.currentClipIndex -= 1;
        gameManager.PlayAudio();
    }
    else
    {
        prevIndexStartPt = true;
        gameManager.PlayAudio();
        StartCoroutine(prevIndexStartPtFalse());
    }
}
    IEnumerator prevIndexStartPtFalse()
    {
        yield return new WaitForSeconds(2.0f);
        prevIndexStartPt = false;
        gameManager.PlayAudio();
    }
}
