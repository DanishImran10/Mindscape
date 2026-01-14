using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class BrainHemispheres : MonoBehaviour
{
    // Centered
    public Transform brainObject; // Assign the "Brain" object in Inspector
    public Camera mainCamera;     // Assign the main Camera
    public Vector3 centeredScale = new Vector3(10f, 10f, 10f); // Scale when centered

    // Animations
    public Animator leftCortexAnimator;
    public Animator rightCortexAnimator;
    public Animator leftHippoCampusAnimator;
    public Animator rightHippoCampusAnimator;
    ///////////////////////////////////
    public GameObject HippocampusPanel;
    public GameObject AmygdaloidPanel;
    public GameObject PrefrontalCortexPanel;
    public GameManager gameManager;
    public Transform otherOrgan;
    public float organMoveDistance = 0.5f;
    public float organScaleFactor = 2f;
    public float organMoveSpeed = 1f;

    private Vector3 organInitialLocalPos;
    private Vector3 organInitialScale;

    public Transform leftHemisphere;
    public Transform rightHemisphere;
    public float moveDistance = 0.05f;
    public float moveSpeed = 1.5f;

    private Vector3 leftInitialLocalPos, rightInitialLocalPos;

    public Material changingMaterial; // Material for highlighting

    private List<GameObject> brainParts = new List<GameObject>();
    private List<GameObject> prefrontalCortexParts = new List<GameObject>();
    private List<GameObject> amygdaloidParts = new List<GameObject>();
    private List<GameObject> hippocampusParts = new List<GameObject>();

    private Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>();

    void Start()
    {
        //gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        leftCortexAnimator.GetComponent<Animator>();
        rightCortexAnimator.GetComponent<Animator>();
        leftHippoCampusAnimator.GetComponent<Animator>();
        rightHippoCampusAnimator.GetComponent<Animator>();

        leftInitialLocalPos = leftHemisphere.localPosition;
        rightInitialLocalPos = rightHemisphere.localPosition;

        organInitialLocalPos = otherOrgan.localPosition;
        organInitialScale = otherOrgan.localScale;

        brainParts.AddRange(GameObject.FindGameObjectsWithTag("BrainParts"));
        prefrontalCortexParts.AddRange(GameObject.FindGameObjectsWithTag("PrefrontalCortex"));
        amygdaloidParts.AddRange(GameObject.FindGameObjectsWithTag("Amygdaloid"));
        hippocampusParts.AddRange(GameObject.FindGameObjectsWithTag("Hippocampus"));

        // Store original materials
        foreach (GameObject part in brainParts)
        {
            if (part.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            {
                originalMaterials[part] = renderer.material;
            }
        }
        foreach (GameObject part in prefrontalCortexParts)
        {
            if (part.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            {
                originalMaterials[part] = renderer.material;
            }
        }
        foreach (GameObject part in amygdaloidParts)
        {
            if (part.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            {
                originalMaterials[part] = renderer.material;
            }
        }
        foreach (GameObject part in hippocampusParts)
        {
            if (part.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            {
                originalMaterials[part] = renderer.material;
            }
        }
    }

    public void CenterBrain()
    {
        // Move brain to center of camera view
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, mainCamera.WorldToScreenPoint(brainObject.position).z);
        Vector3 worldCenterPos = mainCamera.ScreenToWorldPoint(screenCenter);

        brainObject.position = worldCenterPos;

        // Scale brain to a fixed size
        brainObject.localScale = centeredScale;
    }

    public void MoveHemispheres()
    {
        StartCoroutine(SmoothMove());
    }
    IEnumerator SmoothMove()
    {
        float elapsedTime = 0f;
        float duration = 1f / moveSpeed;

        Vector3 leftTarget = leftInitialLocalPos + new Vector3(-moveDistance, 0, 0);
        Vector3 rightTarget = rightInitialLocalPos + new Vector3(moveDistance, 0, 0);

        while (elapsedTime < duration)
        {
            leftHemisphere.localPosition = Vector3.Lerp(leftInitialLocalPos, leftTarget, elapsedTime / duration);
            rightHemisphere.localPosition = Vector3.Lerp(rightInitialLocalPos, rightTarget, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        leftHemisphere.localPosition = leftTarget;
        rightHemisphere.localPosition = rightTarget;
    }

    public void MoveOrgan()
    {
        MoveHemispheres();
        StartCoroutine(SmoothMoveOrgan());
    }

    IEnumerator SmoothMoveOrgan()
    {
        yield return new WaitForSeconds(1.5f);

        float elapsedTime = 0f;
        float duration = 1f / organMoveSpeed;

        Vector3 organTargetPos = organInitialLocalPos + new Vector3(0, organMoveDistance, 0);
        Vector3 organTargetScale = organInitialScale * organScaleFactor;

        while (elapsedTime < duration)
        {
            otherOrgan.localPosition = Vector3.Lerp(organInitialLocalPos, organTargetPos, elapsedTime / duration);
            otherOrgan.localScale = Vector3.Lerp(organInitialScale, organTargetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        otherOrgan.localPosition = organTargetPos;
        otherOrgan.localScale = organTargetScale;
    }

    public void HighlightRegion(string region)
    {
        MoveHemispheres();
        Debug.Log("HighlightRegion Called for: " + region);

        List<GameObject> selectedRegion = new List<GameObject>();

        switch (region)
        {
            case "PrefrontalCortex":
                selectedRegion = prefrontalCortexParts;
                break;
            case "Amygdaloid":
                selectedRegion = amygdaloidParts;
                break;
            case "Hippocampus":
                selectedRegion = hippocampusParts;
                break;
        }

        // Restore original materials first
        foreach (var kvp in originalMaterials)
        {
            kvp.Key.GetComponent<MeshRenderer>().material = kvp.Value;
        }

        // Apply highlighting
        foreach (GameObject part in brainParts)
        {
            if (!selectedRegion.Contains(part))
            {
                part.GetComponent<MeshRenderer>().materials = new Material[0];

                part.GetComponent<MeshRenderer>().material = changingMaterial;
            }
        }

        if (region != "PrefrontalCortex")
        {
            foreach (GameObject part in prefrontalCortexParts)
            {
                part.GetComponent<MeshRenderer>().materials = new Material[0];
                part.GetComponent<MeshRenderer>().material = changingMaterial;
            }
        }
        if (region != "Amygdaloid")
        {
            foreach (GameObject part in amygdaloidParts)
            {
                part.GetComponent<MeshRenderer>().materials = new Material[0];
                part.GetComponent<MeshRenderer>().material = changingMaterial;
            }
        }
        if (region != "Hippocampus")
        {
            foreach (GameObject part in hippocampusParts)
            {
                part.GetComponent<MeshRenderer>().materials = new Material[0];
                part.GetComponent<MeshRenderer>().material = changingMaterial;
            }
        }

        Debug.Log("Material Change Completed");
    }

    public void OnPrefrontalCortexClick()
    {
        if (gameManager.canClick)
        {
            CenterBrain();
            leftCortexAnimator.SetBool("playAnimation", true);
            rightCortexAnimator.SetBool("playAnimation", true);

            PrefrontalCortexPanel.SetActive(true);
            HippocampusPanel.SetActive(false);
            AmygdaloidPanel.SetActive(false);

            gameManager.canClick = true;
            Debug.Log("PrefrontalCortex Clicked");
            HighlightRegion("PrefrontalCortex");
            gameManager.PlayAudio(2);
        }
        // Debug.Log("PrefrontalCortex Clicked");
        // HighlightRegion("PrefrontalCortex");
        // gameManager.PlayAudio(2);
    }

    public void OnAmygdaloidClick()
    {
        if (gameManager.canClick)
        {
            CenterBrain();
            AmygdaloidPanel.SetActive(true);
            PrefrontalCortexPanel.SetActive(false);
            HippocampusPanel.SetActive(false);

            gameManager.canClick = true;
            Debug.Log("Amygdaloid Clicked");
            HighlightRegion("Amygdaloid");
            gameManager.PlayAudio(1);
        }
        // Debug.Log("Amygdaloid Clicked");
        // HighlightRegion("Amygdaloid");
        // gameManager.PlayAudio(1);
    }

    public void OnHippocampusClick()
    {
        if (gameManager.canClick)
        {
            CenterBrain();
            leftHippoCampusAnimator.SetBool("playAnimation", true);
            rightHippoCampusAnimator.SetBool("playAnimation", true);

            HippocampusPanel.SetActive(true);
            PrefrontalCortexPanel.SetActive(false);
            AmygdaloidPanel.SetActive(false);

            gameManager.canClick = true;
            Debug.Log("Hippocampus Clicked");
            HighlightRegion("Hippocampus");
            gameManager.PlayAudio(0);
        }
        // Debug.Log("Hippocampus Clicked");
        // HighlightRegion("Hippocampus");
        // gameManager.PlayAudio(0);
    }
}




// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// public class BrainHemispheres : MonoBehaviour
// {
//     // Organ animations
//     public Transform otherOrgan;
//     public float organMoveDistance = 0.5f; // Organ movement distance (Upwards)
//     public float organScaleFactor = 2f; // Organ enlargement factor
//     public float organMoveSpeed = 1f; // Organ movement speed
//     private Vector3 organInitialLocalPos;
//     private Vector3 organInitialScale;

//     ///////////////////////////////////////
//     public Transform leftHemisphere;  // Left Hemisphere Reference
//     public Transform rightHemisphere; // Right Hemisphere Reference
//     public float moveDistance = 0.2f; // Movement Distance
//     public float moveSpeed = 1.5f; // Movement Speed

//     private Vector3 leftInitialLocalPos, rightInitialLocalPos;

//     void Start()
//     {
//         // Store initial LOCAL positions
//         leftInitialLocalPos = leftHemisphere.localPosition;
//         rightInitialLocalPos = rightHemisphere.localPosition;

//         // Organ movement
//         organInitialLocalPos = otherOrgan.localPosition;
//         organInitialScale = otherOrgan.localScale;
//     }

//     public void MoveHemispheres()
//     {
//         StartCoroutine(SmoothMove());
//     }

//     IEnumerator SmoothMove()
//     {
//         float elapsedTime = 0f;
//         float duration = 1f / moveSpeed;

//         Vector3 leftTarget = leftInitialLocalPos + new Vector3(-moveDistance, 0, 0);
//         Vector3 rightTarget = rightInitialLocalPos + new Vector3(moveDistance, 0, 0);

//         while (elapsedTime < duration)
//         {
//             leftHemisphere.localPosition = Vector3.Lerp(leftInitialLocalPos, leftTarget, elapsedTime / duration);
//             rightHemisphere.localPosition = Vector3.Lerp(rightInitialLocalPos, rightTarget, elapsedTime / duration);
//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }

//         leftHemisphere.localPosition = leftTarget;
//         rightHemisphere.localPosition = rightTarget;
//     }

//     public void MoveOrgan()
//     {
//         MoveHemispheres();
//         StartCoroutine(SmoothMoveOrgan());
//     }

//     IEnumerator SmoothMoveOrgan()
//     {
//         yield return new WaitForSeconds(1.5f);

//         float elapsedTime = 0f;
//         float duration = 1f / organMoveSpeed;

//         Vector3 organTargetPos = organInitialLocalPos + new Vector3(0, organMoveDistance, 0);
//         Vector3 organTargetScale = organInitialScale * organScaleFactor;

//         while (elapsedTime < duration)
//         {
//             otherOrgan.localPosition = Vector3.Lerp(organInitialLocalPos, organTargetPos, elapsedTime / duration);
//             otherOrgan.localScale = Vector3.Lerp(organInitialScale, organTargetScale, elapsedTime / duration);
//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }

//         otherOrgan.localPosition = organTargetPos;
//         otherOrgan.localScale = organTargetScale;
//     }
// }
