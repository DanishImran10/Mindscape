using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RaycastWarVet_Activity : MonoBehaviour
{
    // for changing shhader material
    public Material newMaterial;
    // activity bar b q
    public int objectCount;
    public Text objCount;
    ////////////////////
    public bool turnOnRaycast = false;
    public float gazeTime = 2f; // Time required to trigger click
    private float timer = 0f;

    private GameObject currentButton; // The currently focused button
    // public Image gazeProgressUI; // Optional: Assign a UI Image for progress
    public LayerMask buttonLayer; // Assign the UI layer where buttons exist
    //private LineRenderer lineRenderer;
    public GameObject GazeTimer;

    void Start()
    {
        //lineRenderer = GetComponent<LineRenderer>();
        objectCount = 0;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward); // Cast ray forward from Camera
        if (turnOnRaycast)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, buttonLayer)) // Check if button is hit
            {
                //lineRenderer.SetPosition(0, transform.localPosition);
                //lineRenderer.SetPosition(1, transform.InverseTransformDirection(hit.point));

                GameObject hitObject = hit.collider.gameObject;

                // If a new button is detected, reset the timer
                if (hitObject != currentButton)
                {
                    currentButton = hitObject;
                    timer = 0f;
                }

                timer += Time.deltaTime;

                GazeTimer.SetActive(true);
                GazeTimer.GetComponent<Slider>().value = timer / gazeTime;

                // Update progress bar UI (optional)
                // if (gazeProgressUI)
                //     gazeProgressUI.fillAmount = timer / gazeTime;

                if (timer >= gazeTime)
                {
                    ClickButton(currentButton);
                    timer = 0f; // Reset timer after clicking
                }
            }
            else
            {
                //lineRenderer.SetPosition(0, transform.localPosition);
                //lineRenderer.SetPosition(1, transform.localPosition - transform.forward * 100f);
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
            objectCount++;
            objCount.text = objectCount + " ";
            // box collider dissabled, ta k dubara wohi object detect na ho
            BoxCollider boxCollider = buttonObject.GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                boxCollider.enabled = false; // Disable the BoxCollider
            }
            // changing shader material
            MeshRenderer meshRenderer = buttonObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null && meshRenderer.materials.Length > 1)
            {
                Material[] materials = meshRenderer.materials;
                materials[1] = newMaterial;
                meshRenderer.materials = materials;
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
