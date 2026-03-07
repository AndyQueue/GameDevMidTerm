using UnityEngine;

public class TimerTutorialTrigger : TutorialSignTrigger
{
    public float timeToWait; 

    private float timer = 0f;
    //we keep a timer to check if the sign has been up long enough

    protected override bool CheckActionStatus()
    {
        timer += Time.deltaTime;
        //increment timer by how long its been since the function was last called
        //since this function is called by update we are checking every frame 
        if (timer >= timeToWait)
        {
            return true;
            //the sign should disappear so the action is completed
        }
        return false;
    }
}

