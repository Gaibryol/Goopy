using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Wander")]
public class WanderBehavior : FlockBehavior
{
    // Wander is calculated in the agent because we don't want to calculate a new wander direction
    // every frame.
    public override Vector2 CalculateMove(Goopy agent, List<Transform> context, PlayerManager flock)
    {
        return agent.wanderDirection ;
    }
}
