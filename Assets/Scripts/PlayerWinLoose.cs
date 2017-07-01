using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWinLoose : MonoBehaviour 
{



	// Use this for initialization
	void Start () 
	{
		
	}

	void Update()
	{

	}
	
	public void Die()
	{
		//Show Death UI and wait for a moment
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
	}
}
