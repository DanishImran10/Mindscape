using System.Collections;
using UnityEngine;

public class loadingCircleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0,0, -0.9f);
    }
}
