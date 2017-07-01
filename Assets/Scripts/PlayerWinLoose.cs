using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerWinLoose : MonoBehaviour 
{
	public int enemiesPetrified = 0;
	public Text textCounter;


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

	public void Petrified()
	{
		enemiesPetrified++;
		textCounter.text = "ENEMIES PETRIFIED: " + enemiesPetrified;
	}
}
