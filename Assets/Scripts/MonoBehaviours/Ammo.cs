using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    //The amount of damage the ammunition will inflict on an enemy
    public int damageInflicted;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Called when another object enters the trigger collider attached to the ammo object
    void OnTriggerEnter2D(Collider2D other)
    {
        print("Damage enemy");
        //It's important to check if we hit the Boxcollider2D inside the enemy
        if(other is BoxCollider2D)
        {
            
            //Retrieve the enemy script component of the gamObject from the collision
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            //Start the coroutine to damage the enemy
            StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f));
            gameObject.SetActive(false);
        }    
    }
}
