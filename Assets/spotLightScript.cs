using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spotLightScript : MonoBehaviour
{
    public Animator shadowPlayerAnim;
    public GameObject spotLight;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wt());
    }

    IEnumerator wt()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.5f);
            spotLight.SetActive(false);
            shadowPlayerAnim.SetBool("crawl", false);
            yield return new WaitForSeconds(1.5f);
            spotLight.SetActive(true);
            shadowPlayerAnim.SetBool("crawl", true);
        }
    }
}
