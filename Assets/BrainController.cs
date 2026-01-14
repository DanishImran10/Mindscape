using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class BrainController : MonoBehaviour
{
    public Camera mainCamera;         // Reference to the main camera
    public Transform brainObject;     // Reference to the entire brain parent object
    public Vector3 centeredScale = new Vector3(10f, 10f, 10f); // Desired scale when centering

    private bool movable = true; // restrict buton clicks while moving hemispheres or else
    // Start is called before the first frame update
    private GameObject[] brainparts;
    private GameObject[] prefrontalcortexparts;
    private GameObject[] amygdalas;
    private GameObject[] hippocampi;
    public Transform left_hemisphere;
    public Transform right_hemisphere;
    public Material[] BrainMaterials;
    private string highlightedPart = "";
    public GameObject[] prefrontal;
    public GameManager gameManager;
    public GameObject AmygdaloidPanel;
    public GameObject HippocampusPanel;
    public GameObject PrefrontalCortexPanel;

    private List<GameObject> effect1;
    private List<GameObject> effect2;
    private Animator h_animator1;
    private Animator h_animator2;
    private Animator p_animator1;
    private Animator p_animator2;
    private Dictionary<GameObject, Material[]> originalMaterials = new Dictionary<GameObject, Material[]>();

    void Start()
    {
        brainparts = GameObject.FindGameObjectsWithTag("BrainParts");
        prefrontalcortexparts = GameObject.FindGameObjectsWithTag("PrefrontalCortex");
        amygdalas = GameObject.FindGameObjectsWithTag("Amygdaloid");
        hippocampi = GameObject.FindGameObjectsWithTag("Hippocampus");

        StoreOriginalMaterials();

        effect1 = new List<GameObject>();
        amygdalas[0].GetChildGameObjects(effect1);
        effect2 = new List<GameObject>();
        amygdalas[1].GetChildGameObjects(effect2);

        h_animator1 = hippocampi[0].GetComponent<Animator>();
        h_animator2 = hippocampi[1].GetComponent<Animator>();

        p_animator1 = prefrontal[0].GetComponent<Animator>();
        p_animator2 = prefrontal[1].GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StoreOriginalMaterials()
    {
        originalMaterials.Clear();

        foreach (GameObject part in brainparts)
        {
            if (part.TryGetComponent<Renderer>(out Renderer renderer))
            {
                originalMaterials[part] = renderer.materials;
            }
        }
    }

    void ChangeMaterials()
    {
        for (int i = 0; i < brainparts.Length; i++)
        {
            brainparts[i].GetComponent<MeshRenderer>().materials = new Material[0];
            brainparts[i].GetComponent<MeshRenderer>().material = BrainMaterials[1];
        }
        for (int i = 0; i < prefrontalcortexparts.Length; i++)
        {
            prefrontalcortexparts[i].GetComponent<MeshRenderer>().materials = new Material[0];
            prefrontalcortexparts[i].GetComponent<MeshRenderer>().material = BrainMaterials[1];
        }
        for (int i = 0; i < amygdalas.Length; i++)
        {
            amygdalas[i].GetComponent<MeshRenderer>().materials = new Material[0];
            amygdalas[i].GetComponent<MeshRenderer>().material = BrainMaterials[1];
        }
        for (int i = 0; i < hippocampi.Length; i++)
        {
            hippocampi[i].GetComponent<MeshRenderer>().materials = new Material[0];
            hippocampi[i].GetComponent<MeshRenderer>().material = BrainMaterials[1];
        }
    }

    void RestoreMaterials()
    {
        foreach (var entry in originalMaterials)
        {
            if (entry.Key != null && entry.Key.TryGetComponent<Renderer>(out Renderer renderer))
            {
                renderer.materials = entry.Value;
            }
        }
        for (int i = 0; i < prefrontalcortexparts.Length; i++)
        {
            prefrontalcortexparts[i].GetComponent<MeshRenderer>().materials = new Material[0];
            prefrontalcortexparts[i].GetComponent<MeshRenderer>().material = BrainMaterials[0];
        }
        for (int i = 0; i < amygdalas.Length; i++)
        {
            amygdalas[i].GetComponent<MeshRenderer>().materials = new Material[0];
            amygdalas[i].GetComponent<MeshRenderer>().material = BrainMaterials[0];
        }
        for (int i = 0; i < hippocampi.Length; i++)
        {
            hippocampi[i].GetComponent<MeshRenderer>().materials = new Material[0];
            hippocampi[i].GetComponent<MeshRenderer>().material = BrainMaterials[0];
        }
    }

    void HighlightParts(GameObject[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            list[i].GetComponent<MeshRenderer>().materials = new Material[0];
            list[i].GetComponent<MeshRenderer>().material = BrainMaterials[2];
        }
    }

    //     IEnumerator MoveHemispheresTogether()
    // {
    //     movable = false;
    //     Vector3 targetPosL = left_hemisphere.position + left_hemisphere.right * 0.1f;
    //     Vector3 targetPosR = right_hemisphere.position - right_hemisphere.right * 0.1f;

    //     float time = 0f;
    //     float duration = 2f;
    //     while (time < duration)
    //     {
    //         float t = time / duration;
    //         left_hemisphere.position = Vector3.Lerp(left_hemisphere.position, targetPosL, t);
    //         right_hemisphere.position = Vector3.Lerp(right_hemisphere.position, targetPosR, t);
    //         time += Time.deltaTime;
    //         yield return null;
    //     }

    //     movable = true;
    //     yield break;
    // }

    // IEnumerator MoveHemispheresApart()
    // {
    //     movable = false;
    //     Vector3 targetPosL = left_hemisphere.position - left_hemisphere.right * 0.1f;
    //     Vector3 targetPosR = right_hemisphere.position + right_hemisphere.right * 0.1f;

    //     float time = 0f;
    //     float duration = 2f;
    //     while (time < duration)
    //     {
    //         float t = time / duration;
    //         left_hemisphere.position = Vector3.Lerp(left_hemisphere.position, targetPosL, t);
    //         right_hemisphere.position = Vector3.Lerp(right_hemisphere.position, targetPosR, t);
    //         time += Time.deltaTime;
    //         yield return null;
    //     }

    //     movable = true;
    //     yield break;
    // }


    IEnumerator MoveHemispheresTogether()
    {
        movable = false;

        Vector3 startPosL = left_hemisphere.localPosition;
        Vector3 targetPosL = startPosL + Vector3.right * 0.1f;

        Vector3 startPosR = right_hemisphere.localPosition;
        Vector3 targetPosR = startPosR - Vector3.right * 0.1f;

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            left_hemisphere.localPosition = Vector3.Lerp(startPosL, targetPosL, t);
            right_hemisphere.localPosition = Vector3.Lerp(startPosR, targetPosR, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        left_hemisphere.localPosition = targetPosL;
        right_hemisphere.localPosition = targetPosR;

        movable = true;
    }

    IEnumerator MoveHemispheresApart()
    {
        movable = false;

        Vector3 startPosL = left_hemisphere.localPosition;
        Vector3 targetPosL = startPosL - Vector3.right * 0.1f;

        Vector3 startPosR = right_hemisphere.localPosition;
        Vector3 targetPosR = startPosR + Vector3.right * 0.1f;

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            left_hemisphere.localPosition = Vector3.Lerp(startPosL, targetPosL, t);
            right_hemisphere.localPosition = Vector3.Lerp(startPosR, targetPosR, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        left_hemisphere.localPosition = targetPosL;
        right_hemisphere.localPosition = targetPosR;

        movable = true;
    }


    public void AmygdalaClick()
    {
        if (!movable) return;

        CenterBrain();
        h_animator1.SetBool("playAnimation", false);
        h_animator2.SetBool("playAnimation", false);
        p_animator1.SetBool("playAnimation", false);
        p_animator2.SetBool("playAnimation", false);

        if (highlightedPart == "")
        {
            StartCoroutine(MoveHemispheresApart());
            ChangeMaterials();
            HighlightParts(amygdalas);
            highlightedPart = "Amygdala";

            effect1[0].SetActive(true);
            effect2[0].SetActive(true);

            AmygdaloidPanel.SetActive(true);
            PrefrontalCortexPanel.SetActive(false);
            HippocampusPanel.SetActive(false);

            gameManager.PlayAudio(1);
        }
        else if (highlightedPart == "Amygdala")
        {
            StartCoroutine(MoveHemispheresTogether());
            RestoreMaterials();
            highlightedPart = "";

            effect1[0].SetActive(false);
            effect2[0].SetActive(false);

            AmygdaloidPanel.SetActive(false);
            PrefrontalCortexPanel.SetActive(false);
            HippocampusPanel.SetActive(false);

            // gameManager.PlayAudio(3);
            gameManager.StopAudio();
        }
        else
        {
            ChangeMaterials();
            HighlightParts(amygdalas);
            highlightedPart = "Amygdala";

            effect1[0].SetActive(true);
            effect2[0].SetActive(true);

            AmygdaloidPanel.SetActive(true);
            PrefrontalCortexPanel.SetActive(false);
            HippocampusPanel.SetActive(false);

            gameManager.PlayAudio(1);
        }
    }

    public void HippocampusClick()
    {
        if (!movable) return;

        CenterBrain();
        effect1[0].SetActive(false);
        effect2[0].SetActive(false);
        p_animator1.SetBool("playAnimation", false);
        p_animator2.SetBool("playAnimation", false);

        if (highlightedPart == "")
        {
            StartCoroutine(MoveHemispheresApart());
            ChangeMaterials();
            HighlightParts(hippocampi);
            highlightedPart = "Hippocampus";

            h_animator1.SetBool("playAnimation", true);
            h_animator2.SetBool("playAnimation", true);

            AmygdaloidPanel.SetActive(false);
            PrefrontalCortexPanel.SetActive(false);
            HippocampusPanel.SetActive(true);

            gameManager.PlayAudio(0);
        }
        else if (highlightedPart == "Hippocampus")
        {
            StartCoroutine(MoveHemispheresTogether());
            RestoreMaterials();
            highlightedPart = "";

            h_animator1.SetBool("playAnimation", false);
            h_animator2.SetBool("playAnimation", false);

            AmygdaloidPanel.SetActive(false);
            PrefrontalCortexPanel.SetActive(false);
            HippocampusPanel.SetActive(false);

            //gameManager.PlayAudio(3);
            gameManager.StopAudio();
        }
        else
        {
            ChangeMaterials();
            HighlightParts(hippocampi);
            highlightedPart = "Hippocampus";

            h_animator1.SetBool("playAnimation", true);
            h_animator2.SetBool("playAnimation", true);

            AmygdaloidPanel.SetActive(false);
            PrefrontalCortexPanel.SetActive(false);
            HippocampusPanel.SetActive(true);

            gameManager.PlayAudio(0);
        }
    }

    public void PrefrontalCortexClick()
    {
        if (!movable) return;

        CenterBrain();
        effect1[0].SetActive(false);
        effect2[0].SetActive(false);
        h_animator1.SetBool("playAnimation", false);
        h_animator2.SetBool("playAnimation", false);

        if (highlightedPart == "")
        {
            StartCoroutine(MoveHemispheresApart());
            ChangeMaterials();
            HighlightParts(prefrontalcortexparts);
            highlightedPart = "PrefrontalCortex";

            p_animator1.SetBool("playAnimation", true);
            p_animator2.SetBool("playAnimation", true);

            AmygdaloidPanel.SetActive(false);
            PrefrontalCortexPanel.SetActive(true);
            HippocampusPanel.SetActive(false);

            gameManager.PlayAudio(2);
        }
        else if (highlightedPart == "PrefrontalCortex")
        {
            StartCoroutine(MoveHemispheresTogether());
            RestoreMaterials();
            highlightedPart = "";

            p_animator1.SetBool("playAnimation", false);
            p_animator2.SetBool("playAnimation", false);

            AmygdaloidPanel.SetActive(false);
            PrefrontalCortexPanel.SetActive(false);
            HippocampusPanel.SetActive(false);

            //gameManager.PlayAudio(3);
            gameManager.StopAudio();
        }
        else
        {
            ChangeMaterials();
            HighlightParts(prefrontalcortexparts);
            highlightedPart = "PrefrontalCortex";

            p_animator1.SetBool("playAnimation", true);
            p_animator2.SetBool("playAnimation", true);

            AmygdaloidPanel.SetActive(false);
            PrefrontalCortexPanel.SetActive(true);
            HippocampusPanel.SetActive(false);

            gameManager.PlayAudio(2);
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
}






// using System.Collections;
// using System.Collections.Generic;
// using Unity.XR.CoreUtils;
// using UnityEngine;

// public class BrainController : MonoBehaviour
// {
//     public Camera mainCamera;         // Reference to the main camera
//     public Transform brainObject;     // Reference to the entire brain parent object
//     public Vector3 centeredScale = new Vector3(10f, 10f, 10f); // Desired scale when centering

//     private bool movable = true; // restrict buton clicks while moving hemispheres or else
//     // Start is called before the first frame update
//     private GameObject[] brainparts;
//     private GameObject[] prefrontalcortexparts;
//     private GameObject[] amygdalas;
//     private GameObject[] hippocampi;
//     public Transform left_hemisphere;
//     public Transform right_hemisphere;
//     public Material[] BrainMaterials;
//     private string highlightedPart = "";
//     public GameObject[] prefrontal;
//     public GameManager gameManager;
//     public GameObject AmygdaloidPanel;
//     public GameObject HippocampusPanel;
//     public GameObject PrefrontalCortexPanel;

//     private List<GameObject> effect1;
//     private List<GameObject> effect2;
//     private Animator h_animator1;
//     private Animator h_animator2;
//     private Animator p_animator1;
//     private Animator p_animator2;

//     void Start()
//     {
//         brainparts = GameObject.FindGameObjectsWithTag("BrainParts");
//         prefrontalcortexparts = GameObject.FindGameObjectsWithTag("PrefrontalCortex");
//         amygdalas = GameObject.FindGameObjectsWithTag("Amygdaloid");
//         hippocampi = GameObject.FindGameObjectsWithTag("Hippocampus");

//         effect1 = new List<GameObject>();
//         amygdalas[0].GetChildGameObjects(effect1);
//         effect2 = new List<GameObject>();
//         amygdalas[1].GetChildGameObjects(effect2);

//         h_animator1 = hippocampi[0].GetComponent<Animator>();
//         h_animator2 = hippocampi[1].GetComponent<Animator>();

//         p_animator1 = prefrontal[0].GetComponent<Animator>();
//         p_animator2 = prefrontal[1].GetComponent<Animator>();
//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }

//     void ChangeMaterials()
//     {
//         for (int i = 0; i < brainparts.Length; i++)
//         {
//             brainparts[i].GetComponent<MeshRenderer>().materials = new Material[0];
//             brainparts[i].GetComponent<MeshRenderer>().material = BrainMaterials[1];
//         }
//         for (int i = 0; i < prefrontalcortexparts.Length; i++)
//         {
//             prefrontalcortexparts[i].GetComponent<MeshRenderer>().materials = new Material[0];
//             prefrontalcortexparts[i].GetComponent<MeshRenderer>().material = BrainMaterials[1];
//         }
//         for (int i = 0; i < amygdalas.Length; i++)
//         {
//             amygdalas[i].GetComponent<MeshRenderer>().materials = new Material[0];
//             amygdalas[i].GetComponent<MeshRenderer>().material = BrainMaterials[1];
//         }
//         for (int i = 0; i < hippocampi.Length; i++)
//         {
//             hippocampi[i].GetComponent<MeshRenderer>().materials = new Material[0];
//             hippocampi[i].GetComponent<MeshRenderer>().material = BrainMaterials[1];
//         }
//     }

//     void RestoreMaterials()
//     {
//         for (int i = 0; i < brainparts.Length; i++)
//         {
//             brainparts[i].GetComponent<MeshRenderer>().materials = new Material[0];
//             brainparts[i].GetComponent<MeshRenderer>().material = BrainMaterials[0];
//         }
//         for (int i = 0; i < prefrontalcortexparts.Length; i++)
//         {
//             prefrontalcortexparts[i].GetComponent<MeshRenderer>().materials = new Material[0];
//             prefrontalcortexparts[i].GetComponent<MeshRenderer>().material = BrainMaterials[0];
//         }
//         for (int i = 0; i < amygdalas.Length; i++)
//         {
//             amygdalas[i].GetComponent<MeshRenderer>().materials = new Material[0];
//             amygdalas[i].GetComponent<MeshRenderer>().material = BrainMaterials[0];
//         }
//         for (int i = 0; i < hippocampi.Length; i++)
//         {
//             hippocampi[i].GetComponent<MeshRenderer>().materials = new Material[0];
//             hippocampi[i].GetComponent<MeshRenderer>().material = BrainMaterials[0];
//         }
//     }

//     void HighlightParts(GameObject[] list)
//     {
//         for (int i = 0; i < list.Length; i++)
//         {
//             list[i].GetComponent<MeshRenderer>().materials = new Material[0];
//             list[i].GetComponent<MeshRenderer>().material = BrainMaterials[2];
//         }
//     }

//     IEnumerator MoveHemispheresTogether()
// {
//     movable = false;
//     Vector3 targetPosL = left_hemisphere.position + left_hemisphere.right * 0.1f;
//     Vector3 targetPosR = right_hemisphere.position - right_hemisphere.right * 0.1f;

//     float time = 0f;
//     float duration = 2f;
//     while (time < duration)
//     {
//         float t = time / duration;
//         left_hemisphere.position = Vector3.Lerp(left_hemisphere.position, targetPosL, t);
//         right_hemisphere.position = Vector3.Lerp(right_hemisphere.position, targetPosR, t);
//         time += Time.deltaTime;
//         yield return null;
//     }

//     movable = true;
//     yield break;
// }

// IEnumerator MoveHemispheresApart()
// {
//     movable = false;
//     Vector3 targetPosL = left_hemisphere.position - left_hemisphere.right * 0.1f;
//     Vector3 targetPosR = right_hemisphere.position + right_hemisphere.right * 0.1f;

//     float time = 0f;
//     float duration = 2f;
//     while (time < duration)
//     {
//         float t = time / duration;
//         left_hemisphere.position = Vector3.Lerp(left_hemisphere.position, targetPosL, t);
//         right_hemisphere.position = Vector3.Lerp(right_hemisphere.position, targetPosR, t);
//         time += Time.deltaTime;
//         yield return null;
//     }

//     movable = true;
//     yield break;
// }


//     // IEnumerator MoveHemispheresTogether()
//     // {
//     //     Vector3 targetPosL = left_hemisphere.position;
//     //     targetPosL += left_hemisphere.right * 0.1f;

//     //     Vector3 targetPosR = right_hemisphere.position;
//     //     targetPosR += -right_hemisphere.right * 0.1f;

//     //     float time = 0f;
//     //     float duration = 2f;
//     //     while (time < duration)
//     //     {
//     //         float t = time / duration;
//     //         left_hemisphere.position = Vector3.Lerp(left_hemisphere.position, targetPosL, t);
//     //         right_hemisphere.position = Vector3.Lerp(right_hemisphere.position, targetPosR, t);
//     //         time += Time.deltaTime;
//     //         yield return null;
//     //     }
//     //     yield break;
//     // }

//     // IEnumerator MoveHemispheresApart()
//     // {
//     //     Vector3 targetPosL = left_hemisphere.position;
//     //     targetPosL += -left_hemisphere.right * 0.1f;

//     //     Vector3 targetPosR = right_hemisphere.position;
//     //     targetPosR += right_hemisphere.right * 0.1f;

//     //     float time = 0f;
//     //     float duration = 2f;
//     //     while (time < duration)
//     //     {
//     //         float t = time / duration;
//     //         left_hemisphere.position = Vector3.Lerp(left_hemisphere.position, targetPosL, t);
//     //         right_hemisphere.position = Vector3.Lerp(right_hemisphere.position, targetPosR, t);
//     //         time += Time.deltaTime;
//     //         yield return null;
//     //     }
//     //     yield break;
//     // }

//     public void AmygdalaClick()
//     {
//         if (!movable) return;

//         CenterBrain();
//         h_animator1.SetBool("playAnimation", false);
//         h_animator2.SetBool("playAnimation", false);
//         p_animator1.SetBool("playAnimation", false);
//         p_animator2.SetBool("playAnimation", false);

//         if (highlightedPart == "")
//         {
//             StartCoroutine(MoveHemispheresApart());
//             ChangeMaterials();
//             HighlightParts(amygdalas);
//             highlightedPart = "Amygdala";

//             effect1[0].SetActive(true);
//             effect2[0].SetActive(true);

//             AmygdaloidPanel.SetActive(true);
//             PrefrontalCortexPanel.SetActive(false);
//             HippocampusPanel.SetActive(false);

//             gameManager.PlayAudio(1);
//         }
//         else if (highlightedPart == "Amygdala")
//         {
//             StartCoroutine(MoveHemispheresTogether());
//             RestoreMaterials();
//             highlightedPart = "";

//             effect1[0].SetActive(false);
//             effect2[0].SetActive(false);

//             AmygdaloidPanel.SetActive(false);
//             PrefrontalCortexPanel.SetActive(false);
//             HippocampusPanel.SetActive(false);

//             gameManager.PlayAudio(3);
//         }
//         else
//         {
//             ChangeMaterials();
//             HighlightParts(amygdalas);
//             highlightedPart = "Amygdala";

//             effect1[0].SetActive(true);
//             effect2[0].SetActive(true);

//             AmygdaloidPanel.SetActive(true);
//             PrefrontalCortexPanel.SetActive(false);
//             HippocampusPanel.SetActive(false);

//             gameManager.PlayAudio(1);
//         }
//     }

//     public void HippocampusClick()
//     {
//         if (!movable) return;

//         CenterBrain();
//         effect1[0].SetActive(false);
//         effect2[0].SetActive(false);
//         p_animator1.SetBool("playAnimation", false);
//         p_animator2.SetBool("playAnimation", false);

//         if (highlightedPart == "")
//         {
//             StartCoroutine(MoveHemispheresApart());
//             ChangeMaterials();
//             HighlightParts(hippocampi);
//             highlightedPart = "Hippocampus";

//             h_animator1.SetBool("playAnimation", true);
//             h_animator2.SetBool("playAnimation", true);

//             AmygdaloidPanel.SetActive(false);
//             PrefrontalCortexPanel.SetActive(false);
//             HippocampusPanel.SetActive(true);

//             gameManager.PlayAudio(0);
//         }
//         else if (highlightedPart == "Hippocampus")
//         {
//             StartCoroutine(MoveHemispheresTogether());
//             RestoreMaterials();
//             highlightedPart = "";

//             h_animator1.SetBool("playAnimation", false);
//             h_animator2.SetBool("playAnimation", false);

//             AmygdaloidPanel.SetActive(false);
//             PrefrontalCortexPanel.SetActive(false);
//             HippocampusPanel.SetActive(false);

//             gameManager.PlayAudio(3);
//         }
//         else
//         {
//             ChangeMaterials();
//             HighlightParts(hippocampi);
//             highlightedPart = "Hippocampus";

//             h_animator1.SetBool("playAnimation", true);
//             h_animator2.SetBool("playAnimation", true);

//             AmygdaloidPanel.SetActive(false);
//             PrefrontalCortexPanel.SetActive(false);
//             HippocampusPanel.SetActive(true);

//             gameManager.PlayAudio(0);
//         }
//     }

//     public void PrefrontalCortexClick()
//     {
//         if (!movable) return;

//         CenterBrain();
//         effect1[0].SetActive(false);
//         effect2[0].SetActive(false);
//         h_animator1.SetBool("playAnimation", false);
//         h_animator2.SetBool("playAnimation", false);

//         if (highlightedPart == "")
//         {
//             StartCoroutine(MoveHemispheresApart());
//             ChangeMaterials();
//             HighlightParts(prefrontalcortexparts);
//             highlightedPart = "PrefrontalCortex";

//             p_animator1.SetBool("playAnimation", true);
//             p_animator2.SetBool("playAnimation", true);

//             AmygdaloidPanel.SetActive(false);
//             PrefrontalCortexPanel.SetActive(true);
//             HippocampusPanel.SetActive(false);

//             gameManager.PlayAudio(2);
//         }
//         else if (highlightedPart == "PrefrontalCortex")
//         {
//             StartCoroutine(MoveHemispheresTogether());
//             RestoreMaterials();
//             highlightedPart = "";

//             p_animator1.SetBool("playAnimation", false);
//             p_animator2.SetBool("playAnimation", false);

//             AmygdaloidPanel.SetActive(false);
//             PrefrontalCortexPanel.SetActive(false);
//             HippocampusPanel.SetActive(false);

//             gameManager.PlayAudio(3);
//         }
//         else
//         {
//             ChangeMaterials();
//             HighlightParts(prefrontalcortexparts);
//             highlightedPart = "PrefrontalCortex";

//             p_animator1.SetBool("playAnimation", true);
//             p_animator2.SetBool("playAnimation", true);

//             AmygdaloidPanel.SetActive(false);
//             PrefrontalCortexPanel.SetActive(true);
//             HippocampusPanel.SetActive(false);

//             gameManager.PlayAudio(2);
//         }
//     }
//     public void CenterBrain()
//     {
//         // Move brain to center of camera view
//         Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, mainCamera.WorldToScreenPoint(brainObject.position).z);
//         Vector3 worldCenterPos = mainCamera.ScreenToWorldPoint(screenCenter);

//         brainObject.position = worldCenterPos;

//         // Scale brain to a fixed size
//         brainObject.localScale = centeredScale;
//     }
// }
