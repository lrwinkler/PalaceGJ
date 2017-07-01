using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour 
{
	public float movementDelay = 1;

	private Vector3 currentDirection;
	private Map gameMap;
	// Use this for initialization
	void Start () 
	{
		gameMap = FindObjectOfType<Map>();
		InvokeRepeating("MoveEnemy", 0, movementDelay);
	}

	void MoveEnemy()
	{
		currentDirection = new Vector3(Random.Range(0, 3), Random.Range(0, 3), 0);
		//No movement in both axis at once!!
		if (currentDirection.x != 0 && currentDirection.y != 0)
		{
			currentDirection.y = 0;
		}
		//Move if it's free tile.
		if (gameMap != null)
		{
			if (gameMap.canPass(currentDirection))
			{
				transform.position = currentDirection;
			}
			else
			{
				MoveEnemy();
			}
		}
	}
}
