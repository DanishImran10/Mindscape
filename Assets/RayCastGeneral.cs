using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RayCastGeneral : MonoBehaviour
{
    public bool turnOnRaycast = true;
    public float gazeTime = 2f; // Time required to trigger click
    private float timer = 0f;
    
    private GameObject currentButton; // The currently focused button
    public LayerMask buttonLayer; // Assign the UI layer where buttons exist
    private LineRenderer lineRenderer;
    public GameObject GazeTimer;

    void Start()
    {
        
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward); // Cast ray forward from Camera
        if (turnOnRaycast)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, buttonLayer)) // Check if button is hit
            {
                //Debug.Log("Restarted!!");
                GameObject hitObject = hit.collider.gameObject;

                // If a new button is detected, reset the timer
                if (hitObject != currentButton)
                {
                    currentButton = hitObject;
                    timer = 0f;

                }

                timer += Time.unscaledDeltaTime;

                GazeTimer.SetActive(true);
                GazeTimer.GetComponent<Slider>().value = timer / gazeTime;
                Debug.Log(GazeTimer.GetComponent<Slider>().value);
                if (timer >= gazeTime)
                {
                    ClickButton(currentButton);
                    timer = 0f; // Reset timer after clicking
                }
            }
            else
            {
                ResetGaze();
            }
        }
        else
        {
            GazeTimer.SetActive(false);
        }
    }

    void ClickButton(GameObject buttonObject)
    {
        if (buttonObject)
        {
            Debug.Log("Gaze Clicked: " + buttonObject.name);

            // Get the Button component and invoke its existing listeners
            Button button = buttonObject.GetComponent<Button>();
            if (button)
            {
                button.onClick.Invoke(); // Simulate a real button click
            }
        }
    }

    void ResetGaze()
    {
        timer = 0f;
        currentButton = null;
        GazeTimer.GetComponent<Slider>().value = 0f;
        GazeTimer.SetActive(false);
        // if (gazeProgressUI)
        //     gazeProgressUI.fillAmount = 0f;
    }
}
