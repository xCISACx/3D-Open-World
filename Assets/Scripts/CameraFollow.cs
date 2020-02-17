using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    public float RHor;

    public Transform cameraPivot;
    public float cameraRotationSpeed;
    public Transform cameraTransform;
    public float heading;
    public float xheading;
    public float xSensitivity;
    public float ySensitivity;
    public Vector3 cameraInput;
    public Transform headTransform;
    public Vector3 headTransformOffset;

    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        cameraInput = new Vector3(Input.GetAxis("RVertical"), Input.GetAxis("RHorizontal"), 0);
        
        //RHor = Input.GetAxis("RHorizontal");
        //var rotation = transform.localRotation;

        //rotation.y = RHor++;

        RotateCamera();
        
        cameraPivot.transform.position = headTransform.position + headTransformOffset;
        cameraPivot.rotation = Quaternion.Euler(xheading, heading, 0);
    }

    void RotateCamera()
    {
        if (Input.GetAxis("RHorizontal") > 0.1f)
        {
            //Debug.Log("tilting right stick right");
            heading += xSensitivity;
        }

        if (Input.GetAxis("RHorizontal") < -0.1f)
        {
            //Debug.Log("tilting right stick left");
            heading -= xSensitivity;
        }
        
        if (Input.GetAxisRaw("RVertical") > 0.3f)
        {
            //Debug.Log("tilting right stick up");
            xheading += ySensitivity;
        }

        if (Input.GetAxisRaw("RVertical") < -0.3f)
        {
            //Debug.Log("tilting right stick down");
            xheading -= ySensitivity;
        }
    }
}
