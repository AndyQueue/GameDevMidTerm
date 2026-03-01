using UnityEngine;
using UnityEngine.InputSystem;

public class KeyPressTutorialTrigger : TutorialSignTrigger
{
    public string actionName;
    //asks for action name to check for
    public PlayerInput player;
    //utilizes player input system


    protected override bool CheckActionStatus()
    {
        //checks if the player has completed the action even if it corresponds to multiple keys
        
        // player.actions is the action map so player.actions["stringName"] returns object for that action
        // .triggered checks if the action was started and compelted in this frame (gave other options 
        // like .IsPressed and .wasPressedThisFrame and I decided on .triggered)

        return player.actions[actionName].triggered;
    }
}
