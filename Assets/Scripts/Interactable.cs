using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public PlayerBehaviour Player;
    public float radius;
    //public LayerMask playerLayer;
    public bool CanBeInteractedWith;
    
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
        Player = FindObjectOfType<PlayerBehaviour>();
        
        if (PickupType == Type.ChestItem)
        {
            GetComponent<MeshRenderer>().material = closedMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var distanceFromPlayer = Vector3.Distance(Player.transform.position, gameObject.transform.position);

        if (distanceFromPlayer <= radius)
        {
            CanBeInteractedWith = true;
        }
        else
        {
            CanBeInteractedWith = false;
        }
        
        //RaycastHit hit;
        /*if (Physics.SphereCast(transform.position, radius, transform., out hit, playerLayer))
        {
            //Debug.Log("can interact");
            CanBeInteractedWith = true;
        }
        else
        {
            CanBeInteractedWith = false;
        }*/
        
        if (Input.GetButtonUp("X") && CanBeInteractedWith && !hasBeenOpened)
        {
            bool wasPickedUp = Inventory.instance.AddItem(item);
            switch (PickupType)
            {
                case Type.Item:
                    Debug.Log("Picked up " + item.name);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
