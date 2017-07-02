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
	public CanvasGroup textDie;

	private EnemyGenerator enemyGen;
    private StatueManager statueManager;
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
		StartCoroutine(DoAndDie());
	}

	IEnumerator DoAndDie()
	{
		textDie.alpha = 1;
		yield return new WaitForSeconds(2);
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
            StartCoroutine(win());
        }
	}

    public void newWave()
    {
        enemyGen.numOfEnemiesToSpawn += enemyGen.howManyToAdd;
        enemyGen.SpawnEnemies(enemyGen.numOfEnemiesToSpawn);
        textCounter.text = "ENEMIES LEFT: " + (enemyGen.numOfEnemiesToSpawn);
        enemiesPetrified = 0;
    }
}
