using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{

    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;
    private Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;
    public LayerMask layerMask;
    public Vector3 offset;
    public float percent;
    public Vector3 desiredCameraPos;
    //public Light directionalLight;
    //public Color waterColour;
    //public Color defaultColour;
    
    // Start is called before the first frame update
    void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        dollyDirAdjusted = dollyDir + offset;
        distance = transform.localPosition.magnitude;
        //directionalLight = FindObjectOfType<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        dollyDirAdjusted = dollyDir + offset;
        desiredCameraPos = transform.parent.TransformPoint(dollyDirAdjusted * maxDistance);
        RaycastHit hit;
        //Debug.DrawLine(transform.parent.position, desiredCameraPos);

        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit, layerMask))
        {
            distance = Mathf.Clamp(hit.distance * percent, minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDirAdjusted * distance, Time.deltaTime * smooth);
    }
    
    /*private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            Debug.Log("camera under water");
            directionalLight.color = waterColour; //004BCA
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            directionalLight.color = defaultColour; //FFF4D6
        }
    }*/
}
