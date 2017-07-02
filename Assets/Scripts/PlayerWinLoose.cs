using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerWinLoose : MonoBehaviour 
{
	int enemiesPetrified = 0;
	public Text textCounter;
	public Text textWin;
    public Text textPoints;
	public CanvasGroup diedText;

    public int defaultPoint = 100;
    private int currentScore = 0;

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
		//Show Death UI and wait for a moment

		StartCoroutine(DieDie());
	}

	IEnumerator DieDie()
	{
		diedText.alpha = 1;
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
        updateScore();
		if(enemiesPetrified >= enemyGen.numOfEnemiesToSpawn)
		{
            //Initiate scoring screen
            StartCoroutine(win());
        }
       

    }

    void updateScore()
    {
        currentScore += defaultPoint;
        textCounter.text = "ENEMIES LEFT: " + (enemyGen.numOfEnemiesToSpawn - enemiesPetrified);
        textPoints.text = "POINTS: " + currentScore;
    }

    public void newWave()
    {
        enemyGen.numOfEnemiesToSpawn += enemyGen.howManyToAdd;
        enemyGen.SpawnEnemies(enemyGen.numOfEnemiesToSpawn);
        textCounter.text = "ENEMIES LEFT: " + (enemyGen.numOfEnemiesToSpawn);
        enemiesPetrified = 0;
    }
}
