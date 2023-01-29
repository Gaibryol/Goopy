using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goopy : MonoBehaviour
{
	[SerializeField] private GameObject GoopyPrefab;
	[SerializeField] private Animator anim;
	[SerializeField] private AnimationClip eat;

	[SerializeField, Header("Values")] private int maxAge;
	[SerializeField] private int numSpawn;

	public int Age;

	public void Eat(int amount)
	{
		Age += amount;
		anim.SetBool("Eating", true);

		StartCoroutine(EatCoroutine());
	}

	private IEnumerator EatCoroutine()
	{
		yield return new WaitForSeconds(eat.length);
		anim.SetBool("Eating", false);
	}

	private void HandleMovement()
	{

	}

	private void HandleAge()
	{
		if (Age > maxAge)
		{
			// Die and spawn more goopies
			for (int i = 0; i < numSpawn; i++)
			{
				// Make sure goopy spawns in the screen
				Vector3 randPos = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
				Vector3 spawnPos = transform.position + randPos;
				while (!ValidSpawnPosition(spawnPos))
				{
					randPos = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
					spawnPos = transform.position + randPos;
				}

				// Spawn goopy
				GameObject spawnedGoopy = Instantiate(GoopyPrefab, spawnPos, Quaternion.identity);
				PlayerManager.Instance.AddGoopy(spawnedGoopy.GetComponent<Goopy>());
			}

			// Kill old goopy
			PlayerManager.Instance.RemoveGoopy(this.gameObject.GetComponent<Goopy>());
			Destroy(this.gameObject);
		}
	}

	private void HandleAnimation()
	{
		anim.SetInteger("Age", Age);
	}

	private bool ValidSpawnPosition(Vector3 spawnPos)
	{
		Vector3 bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		if (spawnPos.x < 0 || spawnPos.y < 0)
		{
			return false;
		}
		else if (spawnPos.x > bounds.x || spawnPos.y > bounds.y)
		{
			return false;
		}

		return true;
	}

	// Start is called before the first frame update
	private void Start()
	{
		Age = 0;
	}

	// Update is called once per frame
	private void Update()
	{
		HandleMovement();
		HandleAge();
		HandleAnimation();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Cursor")
		{
			CursorController.Instance.EnterHoverGoopy(this.gameObject.GetComponent<Goopy>());
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Cursor")
		{
			CursorController.Instance.ExitHoverGoopy(this.gameObject.GetComponent<Goopy>());
		}
	}
}