using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    private Queue<Vector3> frontier;
    private List<Vector3> visited;
    private Vector3 current;
    private List<Vector3> currentNeighbors;

    private Vector3 playerLocation;
    private Map mGameMap;
    private GameObject enemies;
    public GameObject debugTile;

    // Use this for initialization
    void Start () {
        frontier = new Queue<Vector3>();
        visited = new List<Vector3>();
        mGameMap = FindObjectOfType<Map>();
        playerLocation = GameObject.Find("Medusa").transform.position;
        enemies = GameObject.Find("EnemySpawner");
        currentNeighbors = new List<Vector3>();
    }
	
	// Update is called once per frame
	void Update () {
        playerLocation = GameObject.Find("Medusa").transform.position;
        enemies = GameObject.Find("EnemySpawner");
    }

    public bool IsBlockedIn()
    {
        frontier.Clear();
        visited.Clear();
        frontier.Enqueue(playerLocation);

        while (frontier.Count != 0)
        {
            currentNeighbors.Clear();
            current = frontier.Dequeue();
            current.x = Mathf.RoundToInt(current.x);
            current.y = Mathf.RoundToInt(current.y);
            current.z = Mathf.RoundToInt(current.z);
            currentNeighbors.Add(current + Vector3.up);
            currentNeighbors.Add(current + Vector3.left);
            currentNeighbors.Add(current + Vector3.right);
            currentNeighbors.Add(current + Vector3.down);
            
            for (int i = 0; i < currentNeighbors.Count; i++)
            {

                //Debug highlighting of pathfinding progression
                //Instantiate(debugTile, current, Quaternion.identity);
               

                if (mGameMap.canPass(currentNeighbors[i]) && !visited.Contains(currentNeighbors[i]))
                {
                    visited.Add(currentNeighbors[i]);
                    frontier.Enqueue(currentNeighbors[i]);
                }
            }
        }

        foreach (Transform child in enemies.transform)
        {
            Vector3 enemyPosition = child.transform.position;
            enemyPosition.x = Mathf.RoundToInt(enemyPosition.x);
            enemyPosition.y = Mathf.RoundToInt(enemyPosition.y);
            enemyPosition.z = Mathf.RoundToInt(enemyPosition.z);
            if (visited.Contains(enemyPosition))
            {
                //Debug.Log(enemyPosition);
                return false;
            }
        }
        return true;

    }
}
