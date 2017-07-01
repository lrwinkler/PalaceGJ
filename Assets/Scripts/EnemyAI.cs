using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour 
{
	public float movementDelay = 1;
	public float steps = 10;
	public float stepDuration = 0.001f;

	private Vector3 currentDirection;
	private Map gameMap;
	private float moveStep;
	// Use this for initialization
	void Start () 
	{
		moveStep = 1 / steps;
		gameMap = FindObjectOfType<Map>();
		InvokeRepeating("MoveEnemy", 0, movementDelay);
	}

	void MoveEnemy()
	{
		StartCoroutine(LerpEnemy());
	}

	IEnumerator LerpEnemy()
	{
		currentDirection = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0);

		//No movement in both axis at once!!
		if (currentDirection.x != 0 && currentDirection.y != 0)
		{
			currentDirection.y = 0;
		}
		//Move if it's free tile.
		if (gameMap != null)
		{
			if (gameMap.canPass(new Vector3(transform.position.x + currentDirection.x, transform.position.y + currentDirection.y, transform.position.z)))
			{
				Vector3 newDirection = new Vector3(transform.position.x + currentDirection.x, transform.position.y + currentDirection.y, transform.position.z);
				Vector3 oldPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
				for (int i = 0; i <= steps; i++)
				{
					Debug.Log(oldPosition);
					transform.position = Vector3.Lerp(oldPosition, newDirection, moveStep*i);
					yield return new WaitForSeconds(stepDuration);
				}
			}
			else
			{
				MoveEnemy();
			}
		}
	}
}
