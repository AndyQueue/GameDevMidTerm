using UnityEngine;
using UnityEngine.InputSystem;

public class KeyPressTutorialTrigger : TutorialSignTrigger
{
    public string actionName;
    //action name to check for
    public PlayerInput player;
    //utilizes player input system


    protected override bool CheckActionStatus()
    {
        return player.actions[actionName].triggered;
    }
}
