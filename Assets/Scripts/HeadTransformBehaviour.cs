using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTransformBehaviour : MonoBehaviour
{

    public PlayerBehaviour playerBehaviour;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerBehaviour = transform.parent.GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            playerBehaviour.isSwimming = true;
            playerBehaviour.gameObject.GetComponent<Rigidbody>().useGravity = false;
            playerBehaviour.speed = playerBehaviour.swimmingSpeed;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            playerBehaviour.isSwimming = false;
            playerBehaviour.gameObject.GetComponent<Rigidbody>().useGravity = true;
            playerBehaviour.speed = playerBehaviour.defaultSpeed;
        }
    }
}
