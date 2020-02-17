using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using RenderSettings = UnityEngine.RenderSettings;

public class WaterBehaviour : MonoBehaviour
{
    public Light directionalLight;
    public Color waterColour;
    public Color defaultColour;
    //public Camera mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        //mainCamera = FindObjectOfType<Camera>();
        if (RenderSettings.sun)
        {
            directionalLight = RenderSettings.sun.gameObject.GetComponent<Light>();
            defaultColour = directionalLight.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            directionalLight.color = waterColour; //004BCA
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            directionalLight.color = defaultColour; //FFF4D6
        }
    }
}
