using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dodge_anim_script : MonoBehaviour
{
    public Animator animatior;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(delay());
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.5f);
        animatior.SetBool("dodge", true);
        yield return new WaitForSeconds(1.0f);
        animatior.SetBool("dodge", false);
    }
}
