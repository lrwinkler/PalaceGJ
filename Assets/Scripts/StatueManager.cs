using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueManager {
    List<Statue> statues;

	// Use this for initialization
	void Start () {
        statues = new List<Statue>();
    }
	
	public void add(Statue statue)
    {
        statues.Add(statue);
    }
}
