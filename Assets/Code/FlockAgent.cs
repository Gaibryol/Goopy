using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    private Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
        GetComponentsInChildren<Transform>()[1].rotation = Quaternion.Euler(0, 0, transform.rotation.z);
    }
}
