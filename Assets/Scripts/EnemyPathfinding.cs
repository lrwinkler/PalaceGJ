using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour {

    private Queue<Vector3> frontier;
    private Dictionary<Vector3, Vector3> cameFrom;
    private Vector3 current;
    private List<Vector3> currentNeighbors;
    private Vector3 start;
    private List<Vector3> path;

    private Vector3 playerLocation;
    private Map mGameMap;

    public Vector3 nextStep;

    // Use this for initialization
    void Start () {
        frontier = new Queue<Vector3>();
        cameFrom = new Dictionary<Vector3, Vector3>();
        mGameMap = FindObjectOfType<Map>();
        playerLocation = GameObject.Find("Medusa").transform.position;
        currentNeighbors = new List<Vector3>();
        start = transform.position;
        start.x = Mathf.RoundToInt(start.x);
        start.y = Mathf.RoundToInt(start.y);
        start.z = Mathf.RoundToInt(start.z);
        path = new List<Vector3>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        playerLocation = GameObject.Find("Medusa").transform.position;
        start = transform.position;
        start.x = Mathf.RoundToInt(start.x);
        start.y = Mathf.RoundToInt(start.y);
        start.z = Mathf.RoundToInt(start.z);
    }

    public void FindPath()
    {
        frontier.Clear();
        cameFrom.Clear();

        frontier.Enqueue(start);

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


                if (mGameMap.canPass(currentNeighbors[i]) && !cameFrom.ContainsKey(currentNeighbors[i]))
                {
                    cameFrom[currentNeighbors[i]] = current;
                    frontier.Enqueue(currentNeighbors[i]);
                }
            }
        }
        playerLocation.x = Mathf.RoundToInt(playerLocation.x);
        playerLocation.y = Mathf.RoundToInt(playerLocation.y);
        playerLocation.z = Mathf.RoundToInt(playerLocation.z);

        current = playerLocation;
        path.Clear();
        path.Add(current);
        
        while (current != start)
        {
            current = cameFrom[current];
            path.Add(current);
        }

        path.Reverse();
        
        if (path.Count >= 1)
        {
            nextStep = path[1] - start;
        }
        else
        {
            nextStep = path[0] - start;
        }
    }
}
