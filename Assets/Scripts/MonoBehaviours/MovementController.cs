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
    void Start()
    {
        rgbd2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        //Debug.Log(movement.x);
    }
    private void FixedUpdate() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        rgbd2D.velocity = movement*movementSpeed;
    }
    private void UpdateState()
    {
        //Check if the movement vector is approximately equal to 0, indicating the player is standing still
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
