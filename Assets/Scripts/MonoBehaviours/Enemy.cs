using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    float hitPoints;
    //Set in Unity Editor, this var will determine how much damage the enemy will do when it runs intro the Player
    public int damageStrength;
    //References to running Coroutines can be saved to a variable and stopped at a later time. We will use damageCoroutine to store a reference to the DameCharater() Coroutine so we can stop it later on
    Coroutine damageCoroutine;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Override abtract class from the parrent class
    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        //This while loop will continue inflict damage until the character dies or if the interval =0, it will break and return
        while(true)
        {
            //Subtract the amount of damage inflicted from the current hitPoints and set the result to hitPoints
            hitPoints -=damage;
            //After adjusting the hitPoints, we would like to check if the hitPoint are less than 0. 
            //However, hitPoints is of type: float and floating point arithmetic is prone to rounding errors due to the way float are implemented under the hood. For this reason we compare with float.Epsilon
            if(hitPoints<=float.Epsilon)
            {
                //if hitPoints is less than float.Epsilon then the enemy has been vanquished. Call the KillCharacter() and break out the while loop
                KillCharacter();
                break;

            }
            //If the interval is greater than float.Epsilon, then we ean to yield execution, wait for interval seconds, then resume executing 
            if(interval>float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else 
            {
                //If the interval is not greater than float.Epsilon then this break statement will be hit, and the loop will be broken. The parameter interval will be zero in situations where damage is not continuous
                break;
            }
        }
    }
    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
    }
    private void OnEnable() {
        ResetCharacter();    
    }
    // void OnCollisionEnter2D(Collision2D other) {
    //     //We want to write game logic such that Enemies an only damage the Player. Compare the Tag on object that enemy collided with see if it's the Player object
    //     if(other.gameObject.CompareTag("Player"))
    //     {
    //         //At this point we are determined that the other object is the Player, so retrieve a reference to the Player component
    //         Player player = other.gameObject.GetComponent<Player>();
    //         //Check to see if this Enemy is already running the DamageCharacter() Coroutine. If it is not, then start the Coroutine on the player object. Pass into DamageCharacter() the damageStrength and the interval, because the enemy will continue to damage the player for as long as they are in contact
    //         if(damageCoroutine==null )
    //         {
    //             print("Starting damage player");
    //             damageCoroutine = StartCoroutine(player.DamageCharacter(damageStrength, 1.0f));
    //         }
    //     }
    // }
    // private void OnCollisionExit2D(Collision2D other) {
    //     if(other.gameObject.CompareTag("Player"))
    //     {
    //         if(damageCoroutine != null)
    //         {
    //             StopCoroutine(damageCoroutine);
    //             damageCoroutine = null;
    //         }
    //     }
    // }
}
