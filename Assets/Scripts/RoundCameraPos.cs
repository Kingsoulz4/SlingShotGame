using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Import Cinemachine framework to write an extention component that attach to the Virtual Cam
using Cinemachine;
// Components that hook into Cinemachine's processing pipeline must inherit from CinnemachineExtension
public class RoundCameraPos : CinemachineExtension
{
    // Start is called before the first frame update
    //Pixel per unit
    public float PPU = 32;
    // This method is required by all classes that inherit from CinemachineExtension. It's called by Cinemachine after the Confiner is done processing
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, 
        float deltaTime
        
    )
    {
        // The vcam has a post-processing pipeline consisting of several stages. We perform this check to see whate stage of the cam are in
        if(stage == CinemachineCore.Stage.Body)
        {
            //Retrieve the vcam final position
            Vector3 pos = state.FinalPosition;
            //Set new position
            Vector3 pos2 = new Vector3(Round(pos.x), Round(pos.y), pos.z);
            state.PositionCorrection += pos2-pos;
        }   
    }
    float Round(float x)
    {
        return Mathf.Round(x*PPU)/PPU;
    }
}
