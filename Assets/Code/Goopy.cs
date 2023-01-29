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
				Vector3 randPos = new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), 0);
				Vector3 spawnPos = transform.position + randPos;

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
