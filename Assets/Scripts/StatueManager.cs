using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueManager : MonoBehaviour
{
    public List<GameObject> statues = new List<GameObject>();
	
	public void add(GameObject statue)
    {
        statues.Add(statue);
    }

	public void Remove(Vector3 point)
	{
		foreach(GameObject statue in statues)
		{
			if(statue.transform.position.Equals(point))
			{
				statues.Remove(statue);
				return;
			}
		}
	}

}
