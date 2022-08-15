using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public HitPoints hitPoints;
    public HealthBar healthBarPrefab;
    HealthBar healthBar;
    public Inventory inventoryPrefab;
    Inventory inventory;
    Coroutine damageCoroutine;


    
    // void Start()
    // {
    //     inventory = Instantiate(inventoryPrefab);
    //     healthBar = Instantiate(healthBarPrefab);
    //     healthBar.character = this;
    //     hitPoints.value = startingHitPoints;
    // }

    private void OnEnable()
    {
        ResetCharacter();
    }
    
    // be called whenever this object overlap with a trigger collider
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("CanBePickedUp"))
        {
            //Our goal is to retrieve a reference to the Item(Scriptable object) inside the Comsumable class and assign it to hitObject
            Item hitObj = other.gameObject.GetComponent<Consumable>().item;
            //other.gameObject.SetActive(false);
            if(hitObj!=null)
            {
                bool appear= true;
                print("it's: "+hitObj.objectName);
                switch (hitObj.itemType)
                {
                    case Item.ItemType.COIN:

                        appear = !inventory.AddItem(hitObj);
                        break;
                    case Item.ItemType.HEALTH:
                        appear = AdjustHitPoints(hitObj.quantity);
                        break;
                    default:
                        break;
                }
              
                other.gameObject.SetActive(appear);
                

            }
        }
    }
    public bool AdjustHitPoints(int amount)
    {
        if(hitPoints.value<maxHitPoints)
        {
            hitPoints.value += amount;
            print("Add: "+amount+ " New hit point: "+hitPoints.value);
            //print("hitpoints: "+hitPoints.value);
            return false;
        }
        return true;
        
    }
    //The same to enemy class
    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        print("Damage player");
        while(true)
        {
            print("hitPoints: "+this.hitPoints.value);
            this.hitPoints.value-=damage;
            
            if(hitPoints.value<=float.Epsilon)
            {
                //KillCharacter();
                break;
            }
            if(interval>float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else 
            {
                break;
            }
            
        }
    }
    public override void KillCharacter()
    {
        //use the base keyword to refer to the parent or base class that the current class inherit from
        base.KillCharacter();
        Destroy(healthBar.gameObject);
        Destroy(inventory.gameObject);
    }
    public override void ResetCharacter()
    {
        inventory = Instantiate(inventoryPrefab);
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;
        hitPoints.value = startingHitPoints;
    }
    void OnCollisionEnter2D(Collision2D other) {
        //We want to write game logic such that Enemies an only damage the Player. Compare the Tag on object that enemy collided with see if it's the Player object
        if(other.gameObject.CompareTag("Enemy"))
        {
            //At this point we are determined that the other object is the Player, so retrieve a reference to the Player component
            
            //Check to see if this Enemy is already running the DamageCharacter() Coroutine. If it is not, then start the Coroutine on the player object. Pass into DamageCharacter() the damageStrength and the interval, because the enemy will continue to damage the player for as long as they are in contact
            if(damageCoroutine==null )
            {
                print("Starting damage player");
                damageCoroutine = StartCoroutine(DamageCharacter(2, 1.0f));
            }
        }
    }
    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy"))
        {
            if(damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }
}
