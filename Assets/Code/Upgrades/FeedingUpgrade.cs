using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Feeding Upgrade")]
public class FeedingUpgrade : Upgrade
{
    [SerializeField] private float interval;    // the interval betwen feeds in seconds
    [SerializeField] private int feedingCount;  // how many goopies to feed
    [SerializeField] private int feedingAmount; // how many times to feed each goopy
    private string timeSinceLastApply;
    public override IEnumerator Apply()
    {
        // Determine how many seconds has passed since the last application
        System.DateTime currentTime = System.DateTime.UtcNow;
        timeSinceLastApply = timeSinceLastApply == "" ? System.DateTime.UtcNow.ToString() : timeSinceLastApply;
        System.DateTime previousTime = System.DateTime.Parse(timeSinceLastApply);
        double secondsSinceLastApply = (currentTime - previousTime).TotalSeconds;

        int missedCycles = 1;
        float timeRemaining = 0f;
        if (secondsSinceLastApply < interval)
        {
            // if x seconds passed is lest than the interval, wait for seconds for next apply
            yield return new WaitForSeconds((float)(interval - secondsSinceLastApply));
        } else
        {
            // We missed some cycles, determine the cycles and the time to wait for the next cycle
            missedCycles = (int)(secondsSinceLastApply / interval);
            timeRemaining = interval - ((float)secondsSinceLastApply % interval);
        }

        while (true)
        {
            for (int i = 0; i < missedCycles; i++)
            {
                Feed();
            }
            
            missedCycles = 1;   // reset cycles back to one per interval
            timeSinceLastApply = System.DateTime.UtcNow.ToString();
            yield return new WaitForSeconds(interval - timeRemaining);
            timeRemaining = 0;
        }
        
    }

    private void Feed()
    {
        List<Goopy> goopies = PlayerManager.Instance.GetRandomNGoopies(feedingCount);
        foreach (Goopy goopy in goopies)
        {
            goopy.Eat(feedingAmount);
        }
    }
}
