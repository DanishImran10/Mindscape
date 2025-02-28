using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioPlayer;
    public AudioClip breaking_glass;
    void Start()
    {
        audioPlayer.clip = breaking_glass;
        StartCoroutine(Fall());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 angle = new Vector3(-10f, 0, 0);
        transform.Rotate(angle, Space.Self);
        audioPlayer.Play();
    }
}
