using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour
{
    public Item item;
    public bool hasBeenOpened = false;
    public Material closedMaterial;
    public Material openedMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasBeenOpened)
        {
            GetComponent<MeshRenderer>().material = closedMaterial;
        }
    }

    public void PickUp()
    {
        bool wasPickedUp = Inventory.instance.AddItem(item);
        
        if (wasPickedUp && !hasBeenOpened)
        {
            hasBeenOpened = true;
            Debug.Log("Picked up " + item.name);
            Debug.Log(wasPickedUp);
            GetComponent<MeshRenderer>().material = openedMaterial;
            //Destroy(gameObject);
        }
        else
        {
            return;
        }
    }
}
