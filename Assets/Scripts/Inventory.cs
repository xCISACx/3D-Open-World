using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Singleton
     
     public static Inventory instance;
 
     void Awake()
     {
         if (instance)
         {
             Debug.LogWarning("More than 1 instance of Inventory found.");
             return;
         }
 
         instance = this;
     }
     
     #endregion

    public PlayerBehaviour Player;
    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedCallback;
    
    public bool InventoryEnabled;
    public GameObject InventoryUI;
    public GameObject SlotHolder;
    public int AvailableSlots;
    public List<Item> ItemList = new List<Item>();   
     
    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<PlayerBehaviour>();
        SlotHolder.transform.GetChild(0).GetChild(0).GetComponent<Button>().Select();
        //Debug.Log(SlotHolder.transform.GetChild(0).GetChild(0).name);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyUp(KeyCode.I) || Input.GetButtonDown("Select"))
        {
            InventoryEnabled = !InventoryEnabled;
            Player.canMove = !Player.canMove;
        }

        if (InventoryEnabled)
        {
            InventoryUI.SetActive(true);
        }
        else
        {
            InventoryUI.SetActive(false);
        }
    }

    public bool AddItem(Item item)
    {
        if (!item.isDefaultItem)
        {
            if (ItemList.Count >= AvailableSlots)
            {
                Debug.Log("Not enough room!");
                return false;
            }
            ItemList.Add(item);

            if (OnItemChangedCallback != null)
            {
                OnItemChangedCallback.Invoke();
            }
        }
        return true;
    }

    public void RemoveItem(Item item)
    {
        ItemList.Remove(item);
        
        if (OnItemChangedCallback != null)
        {
            OnItemChangedCallback.Invoke();
        }
    }
}
