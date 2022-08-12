using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add requires an animator component, so make sure there's always one available 
[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    //A bool to describe if the Player currently firing the slingshot
    bool isFiring;
    //Use the [HideInInspector] attribute along with the public accessor so the animator can be accessed from outside this class but won't show up in the Inspector
    [HideInInspector]
    public Animator animator;
    // Use localCamera store a reference to the Camera so we don't have to retrieve it each time we need it
    Camera localCamera;
    //Store the slope of the two lines used in the quadrant calculation
    float positiveSlope;
    float negativeSlope;
    //An enum used to describe the direction the Player is firing in
    enum Quadrant
    {
        East,
        South,
        West,
        North
    }
    public float weaponVelocity;
    //The property ammoPrefab will be set via the Unity Editor and used to instantiate copies of the AmmoObject. These copies will be added to a pool of objects in the Awake() method
    public GameObject ammoPrefab;
    //The list represent the object pool
    static List<GameObject> ammoPool;
    //The poolSize property allows us to set to number of objects to be pre-instantiated in the object pool
    public int poolSize;

    //The code to create the object pool and pre-initialize the AmmoObjects will be containted in the Awake() method. It is called one time in the lifetime of a script: when the script is being loaded
    void Awake()
    {
        //Check to see if the ammoPool object pool has been initialized already. If not, create a new one
        if (ammoPool==null)
        {
            ammoPool = new List<GameObject>();
        }
        //Create a loop using poolSize as the upper limit. On each iteration of the loop, instantiate a new copy of ammoPrefab, set it to be inactive and add it to the ammoPool
        for (int i=0; i<poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoPrefab);
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }
    }
    void Start()
    {
        //Optimize by grabbing a reference to the Animator component so we don't have to retrieve it every time we need it
        animator = GetComponent<Animator>();
        //Set the isFiring variable to false to start with
        isFiring = false;
        //Grab and save a reference to the local Camera so we don't have to retrieve it each time it's needed
        localCamera = Camera.main;
        //Create four vectors to represent the four cornets of the sceen 
        Vector2 lowerLeft = localCamera.ScreenToWorldPoint(new Vector2(0,0));
        Vector2 upperRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 upperLeft = localCamera.ScreenToWorldPoint(new Vector2(0,Screen.height));
        Vector2 lowerRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0));
        // Use the GetSlope() method to get the slope of each line. One line goes from the upper-left to the lower-right, and the other goes from the upper left to the lower right
        positiveSlope = GetSlope(lowerLeft, upperRight);
        negativeSlope = GetSlope(upperLeft, lowerRight);
        
    }

    // Inside Update() method, check every fame to see if the user has clicked the mouse to fire the slingshot
    void Update()
    {
        //The GetMouseButtonDown() method is part of the Input class and takes a single para. The method parameter, 0, indicates that we are interested in the left mouse button. If we interested in the right mouse button, we would pass the value: 1 instead
        if (Input.GetMouseButtonDown(0))
        {
            //When the left mouse button has been pressed and lifted, set to isFiring var to true. This var will be checked inside the UpdateState() 
            isFiring = true;
            //Call fireAmmo
            FireAmmo();
        }
        //This method will update the animation state every frame, regardless of whether the user has pressed the mouse button or not
        UpdateState();
    }
    //The SpawnAmmo() method will be responsible for retrieving and returning a AmmoObject from the object pool. The method takes a single para: location, indicating where to actually place the retrieved AmmoObject. SpawnAmmo() return GameObject-the activated AmmoObject retrieved from the ammoPool Object Pool
    public GameObject SpawnAmmo(Vector3 location)
    {
        //Loop through the pool of pre-instantiated objects 
        foreach (GameObject ammo in ammoPool)
        {
            //Check if current object is inactivate
            if(ammo.activeSelf==false)
            {
                //We have found an inactivate object, so set it to be active
                ammo.SetActive(true);
                // Set the transform.position on the object to the parameter: location when we call SpawnAmmo(), we will pass a location to make it appear as though the AmmoObject was fired from the sling shot
                ammo.transform.position = location;
                //return active object
                return ammo;
            }
        }
        //No inactivate object was found 
        return null;
    }
    // FireAmmo() will be responsible for moving the AmmoObject from the starting location where it was spawned in SpawnAmmo(), to the end position where the mouse button was clicked
    void FireAmmo()
    {
        //Because the mouse uses Screen space, we convert the mouse position from screen space to world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Retrieve an activated AmmoObject from the ammon object pool via the spawnAmmo() method. Pas the current weapon's transform.position as the starting position for the retrieved ammoObject
        GameObject ammo = SpawnAmmo(transform.position);
        //Check to make sure SpawnAmmo() returned an AmmonObject. Remember, it is possible that SpawnAmmo() return null if all the pre-instantiated objects are already in use 
        if (ammo!=null)
        {
            //Retrieve a reference to the Arc component if the AmmoObject and save it to the variable arcScript
            Arc arcScript = ammo.GetComponent<Arc>();
            //The value weaponVelocity will be set in the Unity Editor. Dividing 1.0 by weaponVelocity results in a fraction that we will use as the travel duration for an AmmoObject. For example, 1.0/2.0 = 0.5, so the Ammo will take half a second to travel across the screen to its destination 
            float travelDuration = 1.0f/weaponVelocity;
            //Call the TravelArc method we wrote earlier on arcScript. Recall the method signature
            StartCoroutine(arcScript.TravelArc(mousePosition, travelDuration));
        }
    }
    //Set the ammoPool = null to destroy the Object Pool and free up memory
    void OnDestroy()
    {
        ammoPool=null;
    }
    float GetSlope(Vector2 point1, Vector2 point2)
    {
        return (point2.y-point1.y)/(point2.x-point1.x);
    }
    bool HigherThanPositiveSlopeLine(Vector2 inputPosition)
    {
        //Save a reference to the current transform.position for clarity. This script is attached to the Player object, so this will be the Players position.
        Vector2 playerPosition = gameObject.transform.position;
        // Covert the inputPosition, which is the mouse position, to World Space and save a reference
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);
        // Rearrange y = mx+b a bit to solve for b. This will make it easy to compare the y-intercept of each line. The form on this line is b=y-mx
        float yIntercept = playerPosition.y - (positiveSlope*playerPosition.x);
        //Using the rearranged form b = y-mx, find the y-intercept for the positive sloped line created by the inputPosition(the mouse)
        float inputIntercept = mousePosition.y - (positiveSlope*mousePosition.x);
        //Compare the y-intercept of the mouse-click to the y-intercept of the line running through the player and return if the mouse-click was higher
        return inputIntercept>yIntercept;
    }
    bool HigherThanNegativeSlopeLine(Vector2 inputPosition)
    {
        Vector2 playerPosition = gameObject.transform.position;
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);
        float yIntercept = playerPosition.y - (negativeSlope*playerPosition.x);
        float inputIntercept = mousePosition.y - (negativeSlope*mousePosition.x);
        return inputIntercept>yIntercept;
    }
    //Return a Quandrant describing where the user clicked
    Quadrant GetQuadrant()
    {
        //Grab references to where the user clicked and the current player position
        Vector2 mousePosition = Input.mousePosition;
        Vector2 playerPosition = transform.position;
        //Check if the user clicked above (higher than) the positive sloped and negative sloped lines
        bool higherThanPositiveSlopeLine = HigherThanPositiveSlopeLine(Input.mousePosition);
        bool higherThanNegativeSlopeLine = HigherThanNegativeSlopeLine(Input.mousePosition);
        //if else statements to check what the quadrant was returned
        if(!higherThanPositiveSlopeLine&&higherThanNegativeSlopeLine)
        {
            return Quadrant.East;
        }
        else if(!higherThanPositiveSlopeLine&&!higherThanNegativeSlopeLine)
        {
            return Quadrant.South;
        }
        else if(higherThanPositiveSlopeLine&&higherThanNegativeSlopeLine)
        {
            return Quadrant.North;
        }
        else 
        {
            return Quadrant.West;
        }
    }
    void UpdateState()
    {
        if(isFiring)
        {
            Vector2 quadrantVector;
            Quadrant quadEnum = GetQuadrant();
            switch (quadEnum)
            {
                case Quadrant.East:
                    quadrantVector = new Vector2(1.0f, 0.0f);
                    break;
                case Quadrant.South:
                    quadrantVector = new Vector2(0.0f, -1.0f);
                    break;
                case Quadrant.West:
                    quadrantVector = new Vector2(-1.0f, 0.0f);
                    break;
                case Quadrant.North:
                    quadrantVector = new Vector2(0.0f,1.0f);
                    break;
                default:
                    quadrantVector = new Vector2(0.0f, 0.0f);
                    break;
            }
            animator.SetBool("isFiring", true);
            animator.SetFloat("fireXDir", quadrantVector.x);
            animator.SetFloat("fireYDir", quadrantVector.y);
            isFiring=false;
        }
        else
        {
            animator.SetBool("isFiring", false);
        }
    }
    
    
}
