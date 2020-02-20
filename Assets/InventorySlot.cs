using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    
    public Item item;

    public Button RemoveButton;

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.ItemSprite;
        icon.enabled = true;
        RemoveButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;
        
        icon.sprite = null;
        icon.enabled = false;
        RemoveButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.RemoveItem(item);
        ClearSlot();
    }

    public void UseItem()
    {
        if (item)
        {
            Debug.Log("item name: " + item.name);
            item.Use();
        }
    }

    public void Update()
    {
        //Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        //Debug.Log(gameObject);
        if (Inventory.instance.InventoryEnabled && EventSystem.current.currentSelectedGameObject != null && gameObject == EventSystem.current.currentSelectedGameObject)
        {
            if (Input.GetButtonUp("X"))
            {
                UseItem();
            }
        }
    }
}
