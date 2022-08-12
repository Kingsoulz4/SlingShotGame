using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RPGCameraManager : MonoBehaviour
{
    public static RPGCameraManager sharedInstance = null;
    //Store a reference to the Cinemachine Virtual Camera. Make it public so that other classes can access it
    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;
    //Implement the Singleton pattern
    void Awake()
    {
        if(sharedInstance != null&&sharedInstance!=this)
        {
            Destroy(gameObject);
        }
        else 
        {
            sharedInstance = this;
        }
        // Find the VirtualCamera in the current Scene
        GameObject vCamGameObject = GameObject.FindWithTag("VirtualCamera");
        //Save a reference to the Virtual Camera component, so we can control these Virtual Camera properties programatically
        virtualCamera = vCamGameObject.GetComponent<CinemachineVirtualCamera>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
