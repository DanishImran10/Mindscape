using System.Collections;
using UnityEngine;

public class TeaKettleScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject steamEffect;
    public AudioSource audioPlayer;
    public AudioClip clip;
    void Start()
    {
        audioPlayer.clip = clip;
        StartCoroutine(Boil());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Boil()
    {
        yield return new WaitForSeconds(1.5f);
        Vector3 v = new Vector3(-24.04f, 66.56f, 59.11f);
        Quaternion r = new Quaternion(0, 0, 0, 0);
        GameObject.Instantiate(steamEffect, v, r);
        audioPlayer.Play();
    }
}
