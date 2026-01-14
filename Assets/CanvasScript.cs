using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    public GameObject presentationText;
    void Start()
    {
        presentationText.SetActive(true);
    }

    void Update()
    {
        
    }
    public void OnToggleTextClick()
    {
        presentationText.SetActive(!presentationText.activeSelf);
    }
    public void OnHomeClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); // currectly 0 is the home scene index
    }
}
