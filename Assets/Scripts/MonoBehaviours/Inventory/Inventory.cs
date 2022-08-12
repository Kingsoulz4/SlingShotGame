using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // Store a reference to the Slt prefab, which we will attach in the Unity Editor. Our Inventory script will instantiate multiple copies of this prefab to use as the Inventory Slots
    public GameObject slotPrefab;
    // The Inventory bar will contain 5 slot
    public const int numSlots = 5;
    //Instantiate an array called itemImages of size numSlots(5). This array will hold Image components. Each image component has a Sprite property
    Image[] itemImages = new Image[numSlots];
    // The items array will hold references to the actual Item, of type Scriptable Objects, that the player has picked up
    Item[] items = new Item[numSlots];
    // Each index in the slots array will reference a single Slot prefab
    GameObject[] slots = new GameObject[numSlots];
    void Start()
    {
        CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Instantiate the slot prefabs
    public void CreateSlots()
    {
        //Check to make sure that we have set the Slot prefab via the Unity Editor, befor we try to use it programmatically
        if(slotPrefab!=null)
        {

            for(int i=0; i<numSlots; i++)
            {
                //Instantiate a copy of the Slot prefab and assign it to newSlot
                GameObject newSlot = Instantiate(slotPrefab);
                //Change the name of the instantiated GameObject to "IntemSlot_" and append the index number 
                newSlot.name = "ItemSlot_"+i;
                //This script will be attached to InventoryObject. The InventoryObject prefab ha a single child object: Inventory. Set the Parent of the instantiated Slot to the child object at index 0 of InventoryObject
                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);
                slots[i] = newSlot;
                itemImages[i] = newSlot.transform.GetChild(1).GetComponent<Image>();
            }
        }
    }
    //The AddItem Method
    public bool AddItem(Item itemToAdd)
    {
        for(int i=0; i<items.Length; i++)
        {
            //These three conditions pertain to Stackable Items. Let's go through this if statement: items[i]!=null cto check if the current index is not null. The next to check if the itemType of the item is equal to the itemType of the item we want to add to the Inventory. The final to check if the item to add is stackable.The Combination of three statement will have the effect of checking to see if the current item in the index
            if(items[i]!=null&&items[i].itemType == itemToAdd.itemType&&itemToAdd.stackable == true)
            {
                //Because we are stacking this item, increment the quantity at the current index
                items[i].quantity +=1;
                //When we instantiate a Slot prefab, what we have realy doing is creating a GameObject with the Slot script. The Slot script contain a reference to the QtyText child Text obj
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                // Grab a reference to the Text object
                Text quantityText = slotScript.qtyText;
                // Because we are adding a stackable object to a slot already containing a stackable object, we now have multiple objects in a slot. Enable the Text obj that we will use to display the quantity
                quantityText.enabled = true;
                //Each Item obj has a quantity property. This statement convert the int to string
                quantityText.text = items[i].quantity.ToString();
                return true;
            }
            //Check of the current index of the items array contain an item. If it's null, then we are goign to add new item to this slot
            if(items[i]==null)
            {
                //Instantiate a copy of the itemToAdd and assign it to the items array
                items[i] = Instantiate(itemToAdd);
                //Set the quantity on the item object to 1
                items[i].quantity = 1;
                //Assign the Sprite from the itemToAddd, to the Image object in the iTemImages array
                itemImages[i].sprite = itemToAdd.sprite;
                // Enable the itemImage and return true
                itemImages[i].enabled = true;
                return true;
            }
        }
        return false;
    }
}
