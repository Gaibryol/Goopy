using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager Instance;

	public List<Goopy> Goopies;
	private int numGoopies;

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

		Goopies = new List<Goopy>();
		numGoopies = 0;
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
}
