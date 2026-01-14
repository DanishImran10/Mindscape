using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car_collide : MonoBehaviour
{
    public GameObject exp1;
    public GameObject exp2;
    public Vector3 rotationAmount = new Vector3(0, 50, 0);
    public Vector3 torqueToAdd = new Vector3(0, 100, 0); // Add 10 units of torque on the Y-axis

    private ConstantForce constantForce;
    // Start is called before the first frame update
    void Start()
    {
        constantForce = GetComponent<ConstantForce>();
        StartCoroutine(wait());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("truck"))
        {
            exp1.SetActive(true);
            exp2.SetActive(true);
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(6.0f);
        constantForce.torque += torqueToAdd;// * Time.deltaTime;
    //    transform.Rotate(rotationAmount * Time.deltaTime);
    }
}
/*using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // Rotation values (change these to adjust how much the GameObject rotates)
    public Vector3 rotationAmount = new Vector3(0, 10, 0); // Rotate 10 degrees on the Y-axis

    // Update is called once per frame
    void Update()
    {
        // Rotate the GameObject slightly each frame
        transform.Rotate(rotationAmount * Time.deltaTime);
    }
}
*/
