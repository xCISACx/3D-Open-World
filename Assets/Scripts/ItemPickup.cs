using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    public enum Type
    {
        Item, ChestItem
    }

    public Type PickupType;
    
    public bool hasBeenOpened = false;
    public Material closedMaterial;
    public Material openedMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PickupType == Type.ChestItem)
        {
            GetComponent<MeshRenderer>().material = closedMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp()
    {
        bool wasPickedUp = Inventory.instance.AddItem(item);
        
        if (!hasBeenOpened && wasPickedUp)
        {
            switch (PickupType)
            {
                case Type.Item:
                    Debug.Log("Picked up " + item.name);
                    Debug.Log(wasPickedUp);
                    Destroy(gameObject);
                    break;
                
                case Type.ChestItem:
                        hasBeenOpened = true;
                        Debug.Log("Picked up " + item.name);
                        Debug.Log(wasPickedUp);
                        GetComponent<MeshRenderer>().material = openedMaterial;          
                    break;
            }
        }
    }
}
