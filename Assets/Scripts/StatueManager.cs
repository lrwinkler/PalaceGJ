using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueManager : MonoBehaviour
{
    List<GameObject> statues = new List<GameObject>();

    private Camera mainCamera;
    private Transform cameraTransform;
	
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

    public void startDeletingSequence()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraTransform = mainCamera.transform;
    }
}
