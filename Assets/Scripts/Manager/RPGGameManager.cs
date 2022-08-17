using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Android;

public class RPGGameManager : MonoBehaviour
{
    public RPGCameraManager cameraManager;
    // A static var: sharedInstance is used to access the Singleton object. The singleton should only be accessed through this propertyy
    // 
    public static RPGGameManager sharedInstance = null;
    // The playerSpawnPoint property will hold a reference to the SpawnPoint specifically designated for the player. We are keeping reference to this specific Spawn Point because we will want the ability to respawn the player when they meet an untimely demise
    public SpawnPoint playerSpawnPoint;

    Player _player = null;
    AudioController audioCon;

    public AudioController AudioCon
    {
        set { audioCon = value; }
        get { return audioCon; }
    }

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
        audioCon = GameObject.Find("AudioController").GetComponent<AudioController>();
        RequestNotifitcationPermission();
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

            _player = player.GetComponent<Player>() ;
            //This will instruct the Cinemachine Virtual Camera to follow the player once again as she walks around the map
            cameraManager.virtualCamera.Follow = player.transform;
        }
    }
    public void SetupScene()
    {
        // This will invoke the SpawnPlayer() method above
        SpawnPlayer();
    }

    /**
     * Switch to game scene
     */
    public void SwitchToGameScene()
    {
        audioCon.PlayClickedEffect();
        StartCoroutine(SwitchToScene("SampleScene", audioCon.ClickedEffect.length));
    }
    /**
     * Switch to main menu scene
     */
    public void SwitchToMainMenuScene()
    {
        audioCon.PlayClickedEffect();
        StartCoroutine(SwitchToScene("MainMenuScene", audioCon.ClickedEffect.length));
    }
    /**
     * Switch to game over scene
     */
    public void SwitchToGameOVerScene()
    {
        StartCoroutine(SwitchToScene("GameOverScene", audioCon.ClickedEffect.length));
    }
    /**
     * Exit game
     */
    public void ExitGame()
    {
        Application.Quit();
    }
    /**Switch to scene coroutine
     */
    public IEnumerator SwitchToScene(string sceneName, float interval)
    {
        yield return new WaitForSeconds(interval);
        SceneManager.LoadScene(sceneName);
    }





    /**
     * Request permission 
     */
    public void RequestNotifitcationPermission()
    {
#if PLATFORM_ANDROID
        if (Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            // The user authorized use of the microphone.
        }
        else
        {
            bool useCallbacks = false;
            if (!useCallbacks)
            {
                // We do not have permission to use the microphone.
                // Ask for permission or proceed without the functionality enabled.
                Permission.RequestUserPermission(Permission.Microphone);
            }
            else
            {
                var callbacks = new PermissionCallbacks();
                callbacks.PermissionDenied += PermissionCallbacks_PermissionDenied;
                callbacks.PermissionGranted += PermissionCallbacks_PermissionGranted;
                callbacks.PermissionDeniedAndDontAskAgain += PermissionCallbacks_PermissionDeniedAndDontAskAgain;
                Permission.RequestUserPermission(Permission.Microphone, callbacks);
            }
        }
#endif
    }

    internal void PermissionCallbacks_PermissionDeniedAndDontAskAgain(string permissionName)
    {
        Debug.Log($"{permissionName} PermissionDeniedAndDontAskAgain");
    }

    internal void PermissionCallbacks_PermissionGranted(string permissionName)
    {
        Debug.Log($"{permissionName} PermissionCallbacks_PermissionGranted");
    }

    internal void PermissionCallbacks_PermissionDenied(string permissionName)
    {
        Debug.Log($"{permissionName} PermissionCallbacks_PermissionDenied");
    }


}
