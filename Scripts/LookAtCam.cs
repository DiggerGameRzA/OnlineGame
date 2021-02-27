using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform);    //Make this 3D text look at the main camera
        transform.Rotate(0, 180, 0);                //Flip this 3D text backward
    }
}
