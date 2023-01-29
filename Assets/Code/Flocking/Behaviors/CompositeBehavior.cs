using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Composite")]
public class CompositeBehavior : FlockBehavior
{
    // Takes in an array of behaviours to determine the final move vector after applying weightings
    public FlockBehavior[] behaviors;
    public float[] weights;

    public override Vector2 CalculateMove(Goopy agent, List<Transform> context, PlayerManager flock)
    {
        // handle data mismatch
        if (weights.Length != behaviors.Length)
        {
            Debug.LogError("Data mismatch in " + name, this);
            return Vector2.zero;
        }

        // set up move
        Vector2 move = Vector2.zero;

        // iterate though behaviors
        for (int i = 0; i < behaviors.Length; i++)
        {
            Vector2 partialMove = behaviors[i].CalculateMove(agent, context, flock) * weights[i];
            Debug.Log("beh: " + behaviors[i].name + " " + partialMove);
            if (partialMove != Vector2.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }
                move += partialMove;
            }
        }
        return move;
    }
}
