using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // Start is called before the first frame update
    public float movementSpeed = 1.2f;
    Vector2 movement = new Vector2();
    Rigidbody2D rgbd2D;
    Animator animator;
    Joystick _fixedJoystick;
    void Start()
    {
        rgbd2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _fixedJoystick = GameObject.Find("MovementJoyStick").GetComponent<Joystick>();
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _fixedJoystick.gameObject.SetActive(true);
        }
        else
        {
            _fixedJoystick.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        //Debug.Log(movement.x);
    }
    private void FixedUpdate() {
        
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || _fixedJoystick.Horizontal != 0 || _fixedJoystick.Vertical != 0)
        {
            Debug.Log("Android move joystick");
            movement.x = _fixedJoystick.Horizontal;
            movement.y = _fixedJoystick.Vertical;
            //_fixedJoystick.AxisOptions
            Debug.Log("Movement x=" + movement.x);
            Debug.Log("Movement y=" + movement.y);
        }
        else
        {
            
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        /*movement.x = _fixedJoystick.Horizontal;
        movement.y = _fixedJoystick.Vertical;*/

        movement.Normalize();
        rgbd2D.velocity = movement*movementSpeed;
    }
    private void UpdateState()
    {
        //Check if the movement vector is approximately equal to 0, indicating the player is standing still
        Debug.Log("Movement x=" + movement.x);
        Debug.Log("Movement y=" + movement.y);
        if (Mathf.Approximately(movement.x, 0)&&Mathf.Approximately(movement.y,0))
        {
            //Because the player is standing still, set isWalking to false
            animator.SetBool("isWalking", false);
        }
        //Otherwise the playere is moving
        else
        {
            animator.SetBool("isWalking", true);
        }
        //Update the animator with the new movement values
        animator.SetFloat("xDir", movement.x);
        animator.SetFloat("yDir", movement.y);
        
    }
}
