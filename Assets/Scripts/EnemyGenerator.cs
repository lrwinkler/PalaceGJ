using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {
	public int startNumberOfEnemies = 3;
	public int howManyToAdd = 1;
	public GameObject enemyPref;
    GameObject newEnemy;

    private Map gameMap;
	private Transform playerTransform;
	public int numOfEnemiesToSpawn;
	private int maxMapWidth;
	private int maxMapHeight;
	// Use this for initialization
	void Start () {
		gameMap = FindObjectOfType<Map>();
		numOfEnemiesToSpawn = startNumberOfEnemies;
		maxMapWidth = gameMap.mapBlueprint.width;
		maxMapHeight = gameMap.mapBlueprint.height;

		SpawnEnemies(numOfEnemiesToSpawn);
	}

	public void SpawnEnemies(int howMany)
	{
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

		for(int i = 0; i < howMany; i++)
		{
			Vector3 newPosition = RandomizePosition();
			newEnemy = GameObject.Instantiate(enemyPref, newPosition, transform.rotation, transform);
		}
	}

	Vector3 RandomizePosition()
	{
		Vector3 newPosition = new Vector3(Random.Range(1, maxMapWidth - 1), Random.Range(1, maxMapHeight - 1), transform.position.z);

        if (gameMap.canPass(newPosition) && newPosition.Equals(playerTransform.position) == false)
        {
            foreach (Transform child in GameObject.Find("EnemySpawner").transform)
            {
                if (newPosition == child.position && newEnemy != child.gameObject)
                {
                    return RandomizePosition();
                }
            }
            return newPosition;
        }
        else
            return RandomizePosition();
	}

	
}
