using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJoyStick : MonoBehaviour
{
    public FixedJoystick _joystick;
    float vel = 1.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = _joystick.Horizontal;
        float y = _joystick.Vertical;
        gameObject.transform.Translate(new Vector3(transform.position.x - x*vel, transform.position.y - y*vel));
    }
}
