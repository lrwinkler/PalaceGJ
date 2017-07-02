using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerWinLoose : MonoBehaviour 
{
	public int enemiesPetrified = 0;
	public Text textCounter;
	//public Text textWin;

	private EnemyGenerator enemyGen;
	// Use this for initialization
	void Start () 
	{
		enemyGen = GameObject.FindObjectOfType<EnemyGenerator>();	
	}

	void Update()
	{

	}
	
	public void Die()
	{
		//Show Death UI and wait for a moment
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
	}
	//public void Win()
	//{
	//	Debug.Log("Win!");
	//	textWin.enabled = true;
	//}

	public void Petrified()
	{
		enemiesPetrified++;
		textCounter.text = "ENEMIES LEFT: " + (enemyGen.numOfEnemiesToSpawn - enemiesPetrified);
		if(enemiesPetrified >= enemyGen.numOfEnemiesToSpawn)
		{
			//Initiate scoring screen
		}
	}
}
