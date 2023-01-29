using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Cohesion")]
public class CohesionBehavior : FlockBehavior
{
    // Cohesion takes in the average position of an objects nearest neighbours and moves towards
    // that direction
    public override Vector2 CalculateMove(Goopy agent, List<Transform> context, PlayerManager flock)
    {
        // if no neighbors, return no adjustment
        if (context.Count == 0) return Vector2.zero;

        // add all points together and average
        Vector2 cohesionMove = Vector2.zero;
        foreach (Transform item in context)
        {
            cohesionMove += (Vector2)item.position;
        }
        cohesionMove /= context.Count;

        // create offset from agent position
        cohesionMove -= (Vector2)agent.transform.position;
        return cohesionMove;
    }
}
