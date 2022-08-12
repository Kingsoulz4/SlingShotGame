using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // This could be any prefab that we want to spawn once or at a consistent interval. We'll set this to be the player or enemy prefab in the Unity Editor
    public GameObject prefabToSpawn;
    // If we want to spawn the prefab at a regular interval, we will set this property in the Unity Editor
    public float repeatInterval;

    void Start()
    {
        // If the repeatInterval is > 0 then we are indicating that the object should be spawned repeatedly at some preset interval
        if (repeatInterval>0)
        {
            // Because the repeatInterval is > 0, we use InvokeRepeating() to spawn the object at regular, repeated intervals. 
            // The method signature for InvokeReating() takes three para: the method to call, the time to wait before invoking the first time, and the time interval to wait between invovations.
            InvokeRepeating("SpawnObject", 0.0f, repeatInterval);
        }
    }
    void Update()
    {
        
    }
    // SpawnObject() is responsible for actually instantiating the prefab and spawning the object. The method signature indicates that it will return a result of type: GameObject, which will be an instance of the spawned object. 
    public GameObject SpawnObject()
    {
        // Check to make sure we have set the prefab in the Unity Editor before we instantiate a copy to avoid errors
        if(prefabToSpawn!=null)
        {
            // Instantiate the prefab at the locattion of the current SpawnPoint object. There are a few different type of Instantiate method used to instantiate prefabs. The specific method we are using takes a prefab, a Vector3 indicating the position, and a special type of date structure called a Quaternion
            // Quaternions are used to represent rotations, and Quaternion.identity represent "no rotation". So we instantiate the prefab at the position of the SpawnPont and without a rotation
            return Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }
        // if the prefabToSpawn is null, then this Spawn Point was probably not configured properly in the editor. Return null
        return null;
    }

}
