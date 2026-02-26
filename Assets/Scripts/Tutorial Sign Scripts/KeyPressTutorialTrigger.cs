using UnityEngine;

public class KeyPressTutorialTrigger : TutorialSignTrigger
{
    public KeyCode[] targetKeys;

    protected override bool CheckActionStatus()
    {
        foreach (KeyCode key in targetKeys)
        {
            //do i need to make this better based on out input system??
            if (Input.GetKeyDown(key))
            {
                return true;
            }
        }
        return false;
    }
}
