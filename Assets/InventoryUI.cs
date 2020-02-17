using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Inventory _inventory;
    
    public Transform SlotHolder;

    public int AvailableSlotsNumber;
    public InventorySlot[] MaxSlots;
    
    // Start is called before the first frame update
    void Start()
    {
        _inventory = Inventory.instance;
        _inventory.OnItemChangedCallback += UpdateUI;
    }

    void Awake()
    {
        _inventory = Inventory.instance;
    }

    // Update is called once per frame
    void Update()
    {
        AvailableSlotsNumber = Inventory.instance.AvailableSlots;
        MaxSlots = SlotHolder.GetComponentsInChildren<InventorySlot>(includeInactive: true);
      
        for (int i = 0; i < MaxSlots.Length; i++)
        {
            if (i < AvailableSlotsNumber)
            {
                MaxSlots[i].gameObject.SetActive(true);
            }
            else
            {
                MaxSlots[i].gameObject.SetActive(false);
            }
        }
    }

    void UpdateUI()
    {
        //Debug.Log("Update UI");

        for (int i = 0; i < MaxSlots.Length; i++)
        {
            if (i < _inventory.ItemList.Count)
            {
                MaxSlots[i].AddItem(_inventory.ItemList[i]);
            }
            else
            {
                MaxSlots[i].ClearSlot();
            }
        }
    }
}
