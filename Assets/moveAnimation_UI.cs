using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveAnimation_UI : MonoBehaviour
{
    public GameObject targetObject;

    void Start()
    {
        if (targetObject != null)
        {
            iTween.MoveTo(gameObject, iTween.Hash(
                "position", targetObject.transform.position, 
                "easeType", "easeInOutExpo", 
                "delay", 1.0));
        }
    }
}
