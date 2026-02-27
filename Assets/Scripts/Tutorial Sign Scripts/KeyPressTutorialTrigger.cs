using UnityEngine;
using UnityEngine.InputSystem;

public class KeyPressTutorialTrigger : TutorialSignTrigger
{
    public string actionName;
    public PlayerInput player;


    protected override bool CheckActionStatus()
    {
        return player.actions[actionName].triggered;
    }
}
