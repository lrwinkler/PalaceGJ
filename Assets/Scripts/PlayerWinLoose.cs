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

    public int defaultPoint = 100;
    public int proximityBonus = 50;
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
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
	}

    IEnumerator win()
    {
        textWin.enabled = true;
        yield return new WaitForSeconds(2.5f);
        textWin.enabled = false;
        newWave();
    }

    public void Petrified(float distance)
	{
		enemiesPetrified++;
        updateScore(distance);
        Debug.Log(distance);
		if(enemiesPetrified >= enemyGen.numOfEnemiesToSpawn)
		{
            //Initiate scoring screen
            StartCoroutine(win());
        }
    }

    void updateScore(float distance)
    {
        currentScore += defaultPoint + Mathf.Max(0, (proximityBonus * (int)(5 - distance)));
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
