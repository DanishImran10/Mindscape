using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene1Again : MonoBehaviour
{
    private BusSoundManager busSoundManager;
    // Start is called before the first frame update
    void Start()
    {
        busSoundManager = FindObjectOfType<BusSoundManager>();
        StartCoroutine(busSoundManager.LoadScene1Again());
    }

}
