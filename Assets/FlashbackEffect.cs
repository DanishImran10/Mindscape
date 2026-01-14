using System.Collections;
using UnityEngine;

public class FlashbackEffect : MonoBehaviour
{
    public GameObject flashbackVolume;   // Assign FlashbackVolume object
    public float flashbackDuration = 5f; // Duration in seconds
    
    void Start()
    {
        flashbackVolume.SetActive(true); // ye use krraha hn kyu k jb tk ye script rahey gi tb tk ye dissable ni krna filhal
    }
    public void TriggerFlashback()
    {
        StartCoroutine(FlashbackRoutine());
    }

    private IEnumerator FlashbackRoutine()
    {
        flashbackVolume.SetActive(true); // Enable black & white
        yield return new WaitForSeconds(flashbackDuration);
        flashbackVolume.SetActive(false); // Restore normal color
    }
}
