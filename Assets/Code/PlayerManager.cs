using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PlayerManager : MonoBehaviour
{
	public static PlayerManager Instance;

	public List<Goopy> Goopies;
	private int numGoopies;

	public FlockBehavior behavior;
	public Upgrade[] upgrades;

	[Range(10, 500)]
	public int startingCount = 250;
	const float AgentDensity = 0.08f;

	[Range(1f, 100f)]
	public float driveFactor = 10f;
	[Range(1f, 100f)]
	public float maxSpeed = 5f;
	[Range(1f, 10f)]
	public float neighbourRadius = 1.5f;
	[Range(0f, 1f)]
	public float avoidanceRadiusMultiplier = 0.5f;

	float squareMaxSpeed;
	float squareNeighbourRadius;
	float squareAvoidanceRadius;
	public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

	// Start is called before the first frame update
	void Start()
	{
		if (PlayerManager.Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}

		Goopies = FindObjectsOfType<Goopy>().ToList();
        numGoopies = 0;

		squareMaxSpeed = maxSpeed * maxSpeed;
		squareNeighbourRadius = neighbourRadius * neighbourRadius;
		squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
		InitUpgrades();
	}

    private void Update()
    {
        foreach (Goopy agent in Goopies)
        {
            List<Transform> context = GetNearbyObjects(agent);
            Vector2 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.HandleMovement(move);
        }
    }

    public void AddGoopy(Goopy goopy)
	{
		Goopies.Add(goopy);
		numGoopies += 1;
	}

	public void RemoveGoopy(Goopy goopy)
	{
		Goopies.Remove(goopy);
		numGoopies -= 1;
	}

	public void RemoveGoopies(int amount)
	{
		List<Goopy> deadGoopies = new List<Goopy>();
		Goopies.Sort((Goopy goopy1, Goopy goopy2) => goopy1.Age.CompareTo(goopy2.Age));

		for (int i = 0; i < amount; i++)
		{
			deadGoopies.Add(Goopies[0]);
			Goopies.RemoveAt(0);
		}
	}

	public List<Goopy> GetRandomNGoopies(int count)
    {
		List<Goopy> randomSet = new List<Goopy>();
		var rnd = new System.Random();
		for (int i = 0; i < count; i++)
        {
			randomSet.Add(Goopies[rnd.Next(0, Goopies.Count)]);
        }
		return randomSet;
    }

	private List<Transform> GetNearbyObjects(Goopy agent)
	{
		List<Transform> context = new List<Transform>();
		Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighbourRadius);
		foreach (Collider2D c in contextColliders)
		{
			if (c != agent.AgentCollider)
			{
				context.Add(c.transform);
			}
		}
		return context;
	}

	private void InitUpgrades()
    {
		foreach (Upgrade upgrade in upgrades)
        {
			StartCoroutine(upgrade.Apply());
        }
    }
}
