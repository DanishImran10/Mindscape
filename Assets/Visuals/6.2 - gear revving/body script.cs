// using System.Collections;
// using UnityEngine;

// public class bodyscript : MonoBehaviour
// {
//     // Start is called before the first frame update
//     private Rigidbody rb;
//     private ConstantForce cf;
//     public GameObject burner;
//     private Animator gearAnimator;
//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         cf = GetComponent<ConstantForce>();
//         gearAnimator = GetComponent<Animator>();
//         StartCoroutine(ToggleRotation());
//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }

//     IEnumerator ToggleRotation()
//     {
//         for(int i = 0; i < 3; i++)
//         {
//             yield return new WaitForSeconds(2f);
//             rb.constraints = RigidbodyConstraints.FreezeRotationZ;
//             yield return new WaitForSeconds(0.25f);
//             rb.constraints = RigidbodyConstraints.None;
//         }
        
//         cf.relativeTorque = new Vector3(0, 0, 0);
//         gearAnimator.enabled = true;
//         gearAnimator.SetBool("TriggerCrash", true);
//         burner.SetActive(true);
//     }

// }



using System.Collections;
using UnityEngine;

public class bodyscript : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    private ConstantForce cf;
    public GameObject burner;
    private Animator gearAnimator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cf = GetComponent<ConstantForce>();
        gearAnimator = GetComponent<Animator>();
        StartCoroutine(ToggleRotation());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ToggleRotation()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(2f);
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
            yield return new WaitForSeconds(0.25f);
            rb.constraints = RigidbodyConstraints.None;
        }

        cf.relativeTorque = new Vector3(0, 0, 0);
        gearAnimator.enabled = true;
        gearAnimator.SetBool("TriggerCrash", true);
        burner.SetActive(true);
    }
}

