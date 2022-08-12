using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]

public class Wander : MonoBehaviour
{
    CircleCollider2D circleCollider;
    //These three variable will be used to set the speed at which the enemy pursues the Player, the general wandering speed when not in pursuit, and the current speed that will be one of the previous two speeds
    public float pursuitSpeed;
    public float wanderSpeed;
    float currentSpeed;
    //The directionChangeInterval is set via the Unity Editor and will be used to determine how often the enemy should change wandering direction
    public float directionChangeInterval;
    // This script can be attached to any Character in the game to add wandering behavior. You may want to eventually create a type of Character that doesn't chase the player and only wanders about. The followPlayer flag can be set to turn on and off the player chasing behavior
    public bool followPlayer;
    //This variable moveCoroutine is where we will save a reference to the currently running movement Coroutine. This Coroutine will be responsible for moving the enemy a little bit each frame, toward the destination. W need to save a reference to the Coroutine because at some point we will need to stop it, and to do that we need a reference
    Coroutine moveCoroutine;
    
    Rigidbody2D rb2d;
    Animator animator;
    // We use targetTransform when the enemy is pursuing the Player. The script will retrieve the transform from the PlayerObject and assign to targetTransform
    Transform targetTransform = null;
    // The destination where the enemy is wandering
    Vector3 endPosition;
    //When choosing a new direction to wander, a new angle is added to the existing angle. That angle is used to generate a vector, whichs becomes the destine destination
    float currentAngle=0;
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        //Grab and cache the Animator component attached to the current GameObject
        animator = GetComponent<Animator>();
        // Set the current speed to wanderSpeed. THe Enemy starts off wandering at leisurely pace
        currentSpeed = wanderSpeed;
        //We will need a reference to the Rigidbody2D to actually move the enemy Store a reference instead of retrieving it every time we need it
        rb2d = GetComponent<Rigidbody2D>();
        //Start the WanderRoutine 
        StartCoroutine(WanderRoutine());
    }

  
    void OnDrawGizmos()
    {
        //Be sure that we have a reference to the Circle Collider before we try to use it
        if(circleCollider!=null)
        {
            //Call Gizmo.DrawWireSphere() and provide a position and a radius for it to draw
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }
    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rb2d.position, endPosition, Color.red);
    }
    //This method is a Coroutine because it will doubtlessly run over multiple frames 
    public IEnumerator WanderRoutine()
    {
        // We want the enemy to wander indefinitely, so we will use while to loop through the steps indefinitely
        while (true)
        {
            // The chooseNewEndpoint() method does exactly what it sounds like. It chooses a new endpoint but doesn't start the enemy moving toward it.
            ChooseNewEndpoint();
            // Check if the enemy is already moving by checking if moveCoroutine is null or has a value. If it has a value then the enemy may be moving, so we will need to stop it first before moving in a new direction
            if(moveCoroutine!=null)
            {
                //Stop the currently running movement Coroutine
                StopCoroutine(moveCoroutine);
            }
            //Start the move() coroutine and save a reference to tit in movecoroutine. The move() Coroutine is responsible for actually moving the enemy.
            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
            //Yield execution of the coroutine for directionChangeInterval seconds, then start the loop over again and choose a new endpoint
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }
    void ChooseNewEndpoint()
    {
        // Choose a random value between 0 and 360 to represent a new direction to travel toward. This direction is represented as an angle, in degrees. We add it to the current angle
        currentAngle +=Random.Range(0,360);
        //The method Mathf.Repeat(currentAngle, 360) will loop the value: currentAngle so that it's never smaller than 0, and never bigger than 360. We are effectively keeping the new angle in the range of degree: 0 to 360 then replaceing the currentAngle with the result
        currentAngle = Mathf.Repeat(currentAngle, 360);
        // Call a method to convert an Angle to a Vector3 and add the result to the endPosition. The variable endPosition will be used by the Move() Coroutine, as we will soon see
        endPosition += Vector3FromAngle(currentAngle);
    }
    Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        float inputAngleRadians = inputAngleDegrees *Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(inputAngleRadians),Mathf.Sin(inputAngleRadians),0);

    }
    public IEnumerator Move(Rigidbody2D rigiBodyToMove, float speed)
    {
        //The equation below will return a Vector3. We use sqrMagnitude to calculate the magnitude of Vector3
        float remainingDistance = (transform.position-endPosition).sqrMagnitude;
        while (remainingDistance>float.Epsilon)
        {
            //When the Enemy is in pursuit of the Player, the value targetTransform will be set to the Player transform. We then overwrite the original value of the endPosition to use targetTransform instead. This allows the enemy to dynamically follow the Player
            if(targetTransform!=null)
            {

                endPosition = targetTransform.position;

            }
            //The Move() method requires a RigidBody2D and uses it to move the Enemy
            if(rigiBodyToMove!=null)
            {
                //Set the animation para: isWalking to true. This will initiate the state transition to the walking state and play the enemy walking animation
                animator.SetBool("isWalking", true);
                //The Vector3.MoveToward method is used to calculate the movement for a RigidBody2D. It doesn't actually move the RigidBody2D. The method takes three para: a current position, an endpoint, and the distance to move in the frame. Remember that the variable: speed will change, depending on whether the enemy pursuit or leisurely
                Vector3 newPosition = Vector3.MoveTowards(rigiBodyToMove.position, endPosition, speed*Time.deltaTime);
                //Use MovePosition() to move the RigidBody2D to the newPosition, calculated in the previous line
                rb2d.MovePosition(newPosition);
                //Update the remainingDistance
                remainingDistance = (transform.position-endPosition).sqrMagnitude;
            }
            //Yield execution until the next Fixed frame update
            yield return new WaitForFixedUpdate();
        }
        //The has reached enPosition and waiting for a new direction to be selected, so change the animation state to idle
        animator.SetBool("isWalking", false);
    }
    void OnTriggerEnter2D(Collider2D other) {
        //Check tag on the object in the collision to see if it is the Player and also check that followPlayer is current true. This var is set via Unity Editor and used to turn on and off the pursuit behavior
        if(other.gameObject.CompareTag("Player")&&followPlayer)
        {
            //Change speed to pursuit player
            currentSpeed = pursuitSpeed;
            //Set targetTransform equal to the player's transform. The Move() coroutine will check if targetTransform is not null and then use it as the new value of enPosition. This is how the enemy continuously pursues the player
            targetTransform = other.gameObject.transform;
            //If the enemy is currently moving, the moveCoroutine will not be  null It needs to be stopped before started again
            if(moveCoroutine!=null)
            {
                StopCoroutine(moveCoroutine);
            }
            //Because endPosition is now set to the PlayerObject's transform calling Move() will move the Enemy toward the Player
            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        //Check the tag to see if the player is leaving the collider
        if(other.gameObject.CompareTag("Player"))
        {
            //The enemy is confused after losing sight of player and pauses for a moment. Set isWalking to false to change the animation to idle
            animator.SetBool("isWalking", false);
            //Set the currentSpeed to the wanderSpeed, to be used the next time the enemy startmoving
            currentSpeed = wanderSpeed;
            //Because we want the enemy to stop pursuing the player, we need to stop the moveCoroutine 
            if(moveCoroutine!=null)
            {
                StopCoroutine(moveCoroutine);
            }
            //The enemy is no longer following the player so set the targetTransform  to null
            targetTransform = null;
        }
    }
}
