using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woman_animPlay_timer : MonoBehaviour
{
    public Animator womanAnim;
    public GameObject blood;
    void Start()
    {
        blood.SetActive(false);
        StartCoroutine(timerForDeathAnim());
    }

    IEnumerator timerForDeathAnim()
    {
        yield return new WaitForSeconds(1.0f);
        blood.SetActive(true);
        womanAnim.SetBool("Die", true);
    }
}
