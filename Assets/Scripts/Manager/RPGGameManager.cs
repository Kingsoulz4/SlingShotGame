using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGGameManager : MonoBehaviour
{
    public RPGCameraManager cameraManager;
    // A static var: sharedInstance is used to access the Singleton object. The singleton should only be accessed through this propertyy
    // 
    public static RPGGameManager sharedInstance = null;
    // The playerSpawnPoint property will hold a reference to the SpawnPoint specifically designated for the player. We are keeping reference to this specific Spawn Point because we will want the ability to respawn the player when they meet an untimely demise
    public SpawnPoint playerSpawnPoint;
    void Awake()
    {
        // Check if SharedInstance is intialized and not equal to the current instance, then destroy it
        if(sharedInstance != null&&sharedInstance!=this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }
    }
    void Start()
    {
        //Consolidate all the logic to setup a scene inside a single method
        SetupScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnPlayer()
    {
        // Check if the playerSpawnPoint property is not null before we try to use it
        if(playerSpawnPoint!=null)
        {
            // Call the SpawnObject() method on the pleyerSpawnPoint. SpawnObject to Spawn the player. Store a local reference to the instantitated player, which we will be using shortly

            GameObject player = playerSpawnPoint.SpawnObject();
            //This will instruct the Cinemachine Virtual Camera to follow the player once again as she walks around the map
            cameraManager.virtualCamera.Follow = player.transform;
        }
    }
    public void SetupScene()
    {
        // This will invoke the SpawnPlayer() method above
        SpawnPlayer();
    }
}
