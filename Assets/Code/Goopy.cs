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
	[SerializeField] private float delayToMove;	// how many seconds to wait before being able to move again

	public int Age;
	private bool isEat;

	private float moveTimer;
	private float movePauseTime;    // how many seconds object can move before stopping
	public Vector2 wanderDirection;

	private Vector2 currentVelocity;	
	// Used for Flocking algorithm
	private Collider2D agentCollider;
	public Collider2D AgentCollider { get { return agentCollider; } }

	public void Eat(int amount)
	{
		Age += amount;
		anim.SetBool("Eating", true);
		anim.SetBool("Moving", false);

		isEat = true;
		StartCoroutine(EatCoroutine());
		PlayerManager.Instance.Save();

	}

	private IEnumerator EatCoroutine()
	{
		yield return new WaitForSeconds(eat.length);
		anim.SetBool("Eating", false);
		isEat = false;
		moveTimer = delayToMove;
	}

	public void HandleMovement(Vector2 velocity)
	{
		if (isEat) return;
		moveTimer -= Time.deltaTime;
		if (moveTimer <= 0)	// Able to move
        {
			// This is used to smooth out the flipping
			currentVelocity = Vector2.Lerp(currentVelocity, velocity, 0.1f);
			anim.SetBool("Moving", currentVelocity.magnitude > 0.15f);
			transform.up = currentVelocity;
			
			transform.position += (Vector3)currentVelocity * Time.deltaTime;
			anim.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z);
			if (currentVelocity.x > 0.2f || currentVelocity.x < -0.2f)
			{
				anim.transform.localScale = new Vector3(Mathf.Sign(currentVelocity.x), 1, 1);
			}
		}
		else
        {
			anim.SetBool("Moving", false);
		}


		if (moveTimer < -movePauseTime)
        {
			// Set delay to move
			moveTimer = delayToMove;
			// Choose another random pause timer
			movePauseTime = Random.Range(2f, 5f);
			// Choose another wander destination
			SetWanderDestination();
		}
	}

	private void SetWanderDestination()
    {
		float screenX = Random.Range(50f, Camera.main.pixelWidth - 50f);
		float screenY = Random.Range(50f, Camera.main.pixelHeight - 50f);
		Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(screenX, screenY, 0));
		wanderDirection = (point-transform.position).normalized;
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
			PlayerManager.Instance.Save();
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
		agentCollider = GetComponent<Collider2D>();
		isEat = false;
		movePauseTime = Random.Range(2f, 5f);
		SetWanderDestination();
	}

	// Update is called once per frame
	private void Update()
	{
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
