using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tornadoScript : MonoBehaviour
{
    public GameObject brokenHouse;
    public GameObject house;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1.5f);
        brokenHouse.SetActive(true);
        house.SetActive(false);
    }
}
