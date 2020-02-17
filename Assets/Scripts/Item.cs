using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite ItemSprite = null;
    public GameObject ItemObject;
    public Mesh ItemMesh = null;
    public bool isDefaultItem = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Use()
    {
        Debug.Log("Used " + name);
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.RemoveItem(this);
    }
}
