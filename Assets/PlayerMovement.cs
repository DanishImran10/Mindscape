using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down
        print(horizontal);
        print(vertical);

        // Get camera's forward and right directions (ignore vertical tilt)
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        // Flatten the vectors to the horizontal plane
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Calculate movement direction
        Vector3 moveDir = camForward * vertical + camRight * horizontal;

        // Apply movement
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("mod1"))
        {
            SceneManager.LoadScene(1);
        }
        if (other.gameObject.CompareTag("mod2"))
        {
            SceneManager.LoadScene(2);
        }
        if (other.gameObject.CompareTag("mod3"))
        {
            PlayerPrefs.SetInt("MyIntKey", 0);
            PlayerPrefs.Save();
            SceneManager.LoadScene(3);
        }
        if (other.gameObject.CompareTag("vuforia"))
        {
            SceneManager.LoadScene(4);
        }
        if (other.gameObject.CompareTag("mod2act"))
        {
            SceneManager.LoadScene(5);
        }
        if (other.gameObject.CompareTag("mod3act"))
        {
            PlayerPrefs.SetInt("MyIntKey", 8);
            PlayerPrefs.Save();
            SceneManager.LoadScene(3);
        }
        if (other.gameObject.CompareTag("mod3quiz"))
        {
            SceneManager.LoadScene(6);
        }
    }



    // public float moveSpeed = 5f;
    // public float rotationSpeed = 200f;

    // private Rigidbody rb;

    // void Start()
    // {
    //     rb = GetComponent<Rigidbody>();
    //     rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Optional: Prevent tipping over
    // }

    // void FixedUpdate()
    // {
    //     float moveInput = Input.GetAxis("Vertical");
    //     float rotateInput = Input.GetAxis("Horizontal");

    //     Vector3 move = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime;
    //     rb.MovePosition(rb.position + move);

    //     float rotation = rotateInput * rotationSpeed * Time.fixedDeltaTime;
    //     Quaternion turn = Quaternion.Euler(0f, rotation, 0f);
    //     rb.MoveRotation(rb.rotation * turn);
    // }
    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("mod1"))
    //     {
    //         SceneManager.LoadScene(1);
    //     }
    //     if (other.gameObject.CompareTag("mod2"))
    //     {
    //         SceneManager.LoadScene(2);
    //     }
    //     if (other.gameObject.CompareTag("mod3"))
    //     {
    //         PlayerPrefs.SetInt("MyIntKey", 0);
    //         PlayerPrefs.Save();
    //         SceneManager.LoadScene(3);
    //     }
    //     if (other.gameObject.CompareTag("vuforia"))
    //     {
    //         SceneManager.LoadScene(4);
    //     }
    //     if (other.gameObject.CompareTag("mod2act"))
    //     {
    //         SceneManager.LoadScene(5);
    //     }
    //     if (other.gameObject.CompareTag("mod3act"))
    //     {
    //         PlayerPrefs.SetInt("MyIntKey", 8);
    //         PlayerPrefs.Save();
    //         SceneManager.LoadScene(3);
    //     }
    //     if(other.gameObject.CompareTag("mod3quiz"))
    //     {
    //         SceneManager.LoadScene(6);
    //     }
    // }
}