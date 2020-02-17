using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton

    public static EquipmentManager instance;

    void Awake()
    {
        instance = this;
    }
    
    #endregion

    private Inventory Inventory;
    public Equipment[] CurrentEquipment;
    public GameObject targetMesh;
    public GameObject[] CurrentMeshes;
    
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);

    public OnEquipmentChanged onEquipmentChanged;

    void Start()
    {
        Inventory = Inventory.instance;
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        CurrentEquipment = new Equipment[numSlots];
        CurrentMeshes = new GameObject[numSlots];
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.EquipmentSlot;

        Equipment oldItem = null;
        
        if (CurrentEquipment[slotIndex] != null)
        {
            oldItem = CurrentEquipment[slotIndex];
            Inventory.AddItem(oldItem);
        }
        
        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
        
        CurrentEquipment[slotIndex] = newItem;
        //GameObject newMesh = Instantiate<GameObject>(newItem.ItemObject);
        //newMesh.transform.parent = targetMesh.transform;
        //GameObject newMesh = Instantiate(newItem.ItemObject, targetMesh.transform);
        
        //CurrentMeshes[slotIndex] = newMesh;
    }

    public void Unequip(int slotIndex)
    {
        if (CurrentEquipment[slotIndex])
        {
            if (CurrentMeshes[slotIndex])
            {
                Destroy(CurrentMeshes[slotIndex].gameObject);
            }
            Equipment oldItem = CurrentEquipment[slotIndex];
            Inventory.AddItem(oldItem);

            CurrentEquipment[slotIndex] = null;
            
            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
        }
    }

    public void UnequipAll()
    {
        for (int i = 0; i < CurrentEquipment.Length; i++)
        {
            Unequip(i);
        }
    }

    private void Update()
    {
        if (Inventory.InventoryEnabled && Input.GetButtonDown("Square"))
        {
            UnequipAll();
        }
    }
}
