using UnityEngine;

public class TimerTutorialTrigger : TutorialSignTrigger
{
    public float timeToWait;

    private float timer = 0f;

    protected override bool CheckActionStatus()
    {
        timer += Time.deltaTime;
        if (timer >= timeToWait)
        {
            return true;
        }
        return false;
    }
}

