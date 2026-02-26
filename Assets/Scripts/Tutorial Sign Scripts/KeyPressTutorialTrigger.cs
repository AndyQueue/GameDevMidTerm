using UnityEngine;

public class KeyPressTutorialTrigger : TutorialSignTrigger
{
    public KeyCode[] targetKeys;

    protected override bool CheckActionStatus()
    {
        foreach (KeyCode key in targetKeys)
        {
            //do I need to change this
            if (Input.GetKeyDown(key))
            {
                return true;
            }
        }
        return false;
    }
}
