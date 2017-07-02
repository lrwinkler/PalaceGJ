using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerWinLoose : MonoBehaviour 
{
	public int enemiesPetrified = 0;
	public Text textCounter;
	public Text textWin;

	private EnemyGenerator enemyGen;
    private StatueManager statueManager;
	// Use this for initialization
	void Start () 
	{
		enemyGen = GameObject.FindObjectOfType<EnemyGenerator>();
        StartCoroutine(win());
    }

	void Update()
	{   
    }

    public void Die()
	{
		//Show Death UI and wait for a moment
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
	}

    IEnumerator win()
    {
        textWin.enabled = true;
        yield return new WaitForSeconds(2.5f);
        textWin.enabled = false;
        newWave();
    }

    public void Petrified()
	{
		enemiesPetrified++;
		textCounter.text = "ENEMIES LEFT: " + (enemyGen.numOfEnemiesToSpawn - enemiesPetrified);
		if(enemiesPetrified >= enemyGen.numOfEnemiesToSpawn)
		{
            //Initiate scoring screen
            statueManager.startDeletingSequence();
        }
	}

    public void newWave()
    {
        enemyGen.SpawnEnemies(enemiesPetrified + enemyGen.howManyToAdd);
    }
}
