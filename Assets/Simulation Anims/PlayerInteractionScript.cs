using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float detect_range = 3f;
    public LayerMask interactable_layer;
    private GameObject interactableObject;
    private float timer = 0f;
    public GameObject GazeTimer;
    public float gazeTime = 2f;
    public GameObject coffeeCup;
    public Transform coffeeCupPicked;
    public GameObject mobile;
    public Transform mobilePicked;
    public GameObject Player;
    public GameObject op1;
    public GameObject op2;
    public GameObject img1;
    public GameObject img2;
    public GameObject MobileScreen;
    private Vector3 mobileOGPos;
    private Quaternion mobileOGRot;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, detect_range, interactable_layer))
        {
            GameObject hitObject = hit.collider.gameObject;

                // If a new button is detected, reset the timer
                if (hitObject != interactableObject)
                {
                    interactableObject = hitObject;
                    timer = 0f;
                }

                timer += Time.deltaTime;

                GazeTimer.SetActive(true);
                GazeTimer.GetComponent<Slider>().value = timer / gazeTime;

                if (timer >= gazeTime)
                {
                    ObjectLookedAt(interactableObject);
                    timer = 0f; // Reset timer after clicking
                }
        }
        else
        {
            ResetGaze();
        }
    }

    void ObjectLookedAt(GameObject interactableObject)
    {
        print("df");
        if (interactableObject.name.StartsWith("Coffee"))
        {
            print("c");
            StartCoroutine(DrinkCoffee());
        }
        else if (interactableObject.name.StartsWith("Mobile"))
        {
            StartCoroutine(TextSomeone());
        }
        else if (interactableObject.name.StartsWith("Opt 1"))
        {
            DisplayText1();
        }
        else if (interactableObject.name.StartsWith("Opt 2"))
        {
            DisplayText2();
        }
    }

    public IEnumerator DrinkCoffee()
    {
        float time = 0f;
        float duration = 1f;

        Vector3 originalPos = coffeeCup.transform.position;
        Quaternion originalRot = coffeeCup.transform.rotation;
        mobileOGPos = originalPos;

        while (time < duration)
        {
            float t = time / duration;
            coffeeCup.transform.position = Vector3.Lerp(coffeeCup.transform.position, coffeeCupPicked.position, t);
            coffeeCup.transform.rotation = Quaternion.Lerp(coffeeCup.transform.rotation, coffeeCupPicked.rotation, t);
            time += Time.deltaTime;
            yield return null;
        }
        coffeeCup.transform.SetParent(Player.transform);
        coffeeCup.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(3f);

        time = 0f;
        duration = 1f;

        while (time < duration)
        {
            float t = time / duration;
            coffeeCup.transform.position = Vector3.Lerp(coffeeCup.transform.position, originalPos, t);
            coffeeCup.transform.rotation = Quaternion.Lerp(coffeeCup.transform.rotation, originalRot, t);
            time += Time.deltaTime;
            yield return null;
        }
        coffeeCup.transform.SetParent(null);
        yield break;
    }

    IEnumerator TextSomeone()
    {
        float time = 0f;
        float duration = 1f;

        Vector3 originalPos = mobile.transform.position;
        Quaternion originalRot = mobile.transform.rotation;
        mobileOGPos = originalPos;
        mobileOGRot = originalRot;

        while (time < duration)
        {
            float t = time / duration;
            mobile.transform.position = Vector3.Lerp(mobile.transform.position, mobilePicked.position, t);
            mobile.transform.rotation = Quaternion.Lerp(mobile.transform.rotation, mobilePicked.rotation, t);
            time += Time.deltaTime;
            yield return null;
        }
        mobilePicked.gameObject.SetActive(true);
        //mobilePicked.gameObject.GetComponent<Animator>().SetBool("startPop", true);

        mobile.SetActive(false);

        op1.SetActive(true);
        op2.SetActive(true);

        yield return new WaitForSeconds(3f);
        yield break;
    }

    void DisplayText1()
    {
        img1.SetActive(true);
        StartCoroutine(WithdrawMobile());
    }

    void DisplayText2()
    {
        img2.SetActive(true);
        StartCoroutine(WithdrawMobile());
    }

    IEnumerator WithdrawMobile()
    {
        op1.SetActive(false);
        op2.SetActive(false);

        yield return new WaitForSeconds(2);
        mobilePicked.gameObject.SetActive(false);
        mobile.SetActive(true);

        Vector3 originalPos = mobileOGPos;
        Quaternion originalRot = mobileOGRot;

        float time = 0f;
        float duration = 1f;

        while (time < duration)
        {
            float t = time / duration;
            mobile.transform.position = Vector3.Lerp(mobile.transform.position, originalPos, t);
            mobile.transform.rotation = Quaternion.Lerp(mobile.transform.rotation, originalRot, t);
            time += Time.deltaTime;
            yield return null;
        }
        mobile.GetComponent<Collider>().enabled = false;
    }

    void ResetGaze()
    {
        timer = 0f;
        interactableObject = null;
        GazeTimer.GetComponent<Slider>().value = 0f;
        GazeTimer.SetActive(false);
    }    
}
