using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //TravelArc() is the method that will move the gameObject along an arc. It makes sense3 to design TravelArc() as a Coroutine because it will execute over the course of several frames. 
    public IEnumerator TravelArc(Vector3 destination, float duration)
    {
        //Grab the current gameObject's position
        var startPosition = transform.position;
        //The percentComplete is used in the Lerp, or Linear Interpolation, calculation used later in this method
        var percentComplete = 0.0f;
        //Check that the percentComplete is less than 1.0. Think of 1.0 as the dicemal form of 100%. We only want this loop to run until percenComplete is 100%. This will make sense when we explain Linear Interpolation in the next line
        while (percentComplete<1.0f)
        {
            //We want to move the AmmoObject smoothly toward its destination. The distance the Ammo will travel each frame is dependent on the duration we want the movement to take place over, and the time already elapsed
            //The amount of time elapsed since the last frame, divided by the total desired duration of the movement, equals a percentage of the total duration
            //Time.deltatime is the amount of time elapsed since the last frame was drawn. The result in that line: percentageComplete, is what we get when we add the percentage of total duration, to the previous percentage completed, to get the total percentage of the duration that has been completed thus far
            percentComplete+=Time.deltaTime/duration;
            //By passing the result of (percentComplete*Mathf.PI) to the sine function, we are effectively traveling PI distance down the sine curve every duration second. The result is assigned to current Height
            var currentHeight = Mathf.Sin(Mathf.PI*percentComplete);
            // To achieve the effect where the AmmoObject appears to move smoothly between two points at a constant speed, we use a widely used technique in game programing called Linear Interpolation
            // Linear Interpolation requires a starting position, an end position, and percentage. When we use Linear Interpolation to determine the distance to travel per frame, the percentage parameter of the Linear Interpolation method: Lerp() , is the percentage of duration completed (percentComplete)
            //Using the duration percentComplete in the Lerp() method means that no matter where we fire the ammo, it will take the same amount of time to get there
            // The lerp() method will return a point between the start and end based on this percentage
            //Vector3.up represent Vector3(0,1,0)
            transform.position=Vector3.Lerp(startPosition, destination, percentComplete) +Vector3.up*currentHeight;
            percentComplete +=Time.deltaTime/duration;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
