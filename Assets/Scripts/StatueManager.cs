using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueManager {
    List<GameObject> statues = new List<GameObject>();
	
	public void add(GameObject statue)
    {
        statues.Add(statue);
    }
}
