using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Create AssetMenu creates an entry in the Create submenu, thí allows us to easily create instances of the Item Scriptable Object

[CreateAssetMenu(menuName = "Item")]

public class Item : ScriptableObject {
    //The field: objectName, can serve many different purposes: Debugging, Display
    public string objectName;

    public Sprite sprite;
    public int quantity;
    //Stackable is a term used to describe how multiple coppies of identical items can be stored in the same place and can be interacted with by the player
    public bool stackable;
    //Define an enum used to indicate the type of an item
    //public int type;
    public enum ItemType
    {
        COIN,
        HEALTH
    }
    public ItemType itemType;
}
