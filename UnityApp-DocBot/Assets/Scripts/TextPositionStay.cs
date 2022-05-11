using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPositionStay : MonoBehaviour
{

    private Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform; // find the main camera
    
    }

    // Update is called once per frame
    void LateUpdate()
    {

   
        transform.LookAt(transform.position + cam.forward); // repositions the UI in terms of the camera persceptive.

        transform.rotation =Quaternion.Euler(new Vector3(90,-180,-270));
    }
}