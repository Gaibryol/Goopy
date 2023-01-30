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
	public List<Upgrade> upgrades;

	[SerializeField] private PlayerData playerData;

	[Header("Flocking variables")]
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

	[SerializeField] private GameObject[] GoopyPrefabs;

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
		Load();
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

	public void Save()
    {
		if (playerData == null)
        {
			playerData = new PlayerData();
        }
		List<SavedGoopy> savedGoopies = new List<SavedGoopy>();
		List<SavedUpgrade> savedUpgrades = new List<SavedUpgrade>();
		foreach (Goopy goopy in Goopies)
        {
			savedGoopies.Add(new SavedGoopy(goopy.Age, "base"));
        }

		foreach (Upgrade upgrade in upgrades)
        {
			savedUpgrades.Add(new SavedUpgrade(upgrade));
        }

		playerData.Save(savedGoopies, savedUpgrades);
    }

	private void Load()
    {
		if (playerData == null)
        {
			// Load default
			return;
        }
		Goopies.Clear();
		upgrades.Clear();
		foreach (SavedGoopy savedGoopy in playerData.SavedGoopies)
        {
			Vector3 randPos = new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), 0);
			Vector3 spawnPos = transform.position + randPos;

			// Spawn goopy
			GameObject spawnedGoopy = Instantiate(GetGoopyPrefab(savedGoopy.Type), spawnPos, Quaternion.identity);
			Goopy goopy = spawnedGoopy.GetComponent<Goopy>();
			goopy.Age = savedGoopy.Age;
			AddGoopy(goopy);
		}

		foreach (SavedUpgrade savedUpgrade in playerData.SavedUpgrades)
        {
			upgrades.Add(savedUpgrade.Upgrade);
        }

		InitUpgrades();
    }

	private GameObject GetGoopyPrefab(string type)
    {
		// add more when we have differnet tyhpe of goopioes
		if (type == "base")
        {
			return GoopyPrefabs[0];
        }
		return null;
    }
}
