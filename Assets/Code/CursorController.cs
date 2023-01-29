using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
	public static CursorController Instance;

	[SerializeField] private GameObject cursor;

	[SerializeField, Header("Values")] private int ageIncrement;
	[SerializeField] private float radiusIncrement;

	private CircleCollider2D cursorColl;

	private List<Goopy> hoverGoopies;

	private void HandleMovement()
	{
		cursor.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	private void HandleClicks()
	{
		if (Input.GetMouseButtonDown(0))
		{
			foreach (Goopy goopy in hoverGoopies)
			{
				goopy.Eat(ageIncrement);
			}
		}
	}

	public void EnterHoverGoopy(Goopy goopy)
	{
		hoverGoopies.Add(goopy);
	}

	public void ExitHoverGoopy(Goopy goopy)
	{
		hoverGoopies.Remove(goopy);
	}

	public void IncrementCursorSize()
	{
		cursorColl.radius += radiusIncrement;
	}

    private void Awake()
    {
		if (CursorController.Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}
		cursorColl = cursor.GetComponent<CircleCollider2D>();
	}

    private void Start()
	{

		hoverGoopies = new List<Goopy>();
	}

	// Update is called once per frame
	void Update()
	{
		HandleMovement();
		HandleClicks();
	}
}
