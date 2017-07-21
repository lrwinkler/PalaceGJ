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
    public Text gameOverText;

    public int defaultPoint = 100;
    public int proximityBonus = 50;
    private int currentScore = 0;
    private int statuesSold = 0;
    public int statuesToSell = 2;

    private EnemyGenerator enemyGen;
    private StatueManager statueManager;
    private Animator medusaAnimator;
    private Pathfinding pathfinding;
    private SelectorControl selectorControl;
    public bool isDead;

    // Use this for initialization
    void Start()
    {
        enemyGen = GameObject.FindObjectOfType<EnemyGenerator>();
        medusaAnimator = GameObject.Find("Medusa").GetComponent<Animator>();
        pathfinding = GameObject.Find("Medusa").GetComponent<Pathfinding>();
    }

    void Update()
    {
    }

    public void Die(bool starved)
    {
        //Show Death UI and wait for a moment

        StartCoroutine(DieDie(starved));
    }

    IEnumerator DieDie(bool starved)
    {
        isDead = true;

        GameObject.Find("Medusa").GetComponent<PlayerMovement>().enabled = false;
        GameObject.Find("Medusa").GetComponent<BoxCollider2D>().enabled = false;

        medusaAnimator.SetBool("FacingUp", false);
        medusaAnimator.SetBool("FacingDown", false);
        medusaAnimator.SetBool("FacingRight", false);
        medusaAnimator.SetBool("FacingLeft", false);
        medusaAnimator.SetBool("IsDead", true);
        yield return new WaitForSeconds(0.1f);
        medusaAnimator.SetBool("IsDead", false);
        yield return new WaitForSeconds(2);

        if (starved)
        {
            gameOverText.text = "YOU STARVED\nFinal Score: " + currentScore;
        }
        else
        {
            gameOverText.text = "YOU DIED\nFinal Score: " + currentScore;
        }
        diedText.alpha = 1;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Level0");
    }

    IEnumerator win()
    {
        GameObject.Find("Medusa").GetComponent<PlayerMovement>().enabled = false;
        //StatueSelling();
        textWin.enabled = true;
        yield return new WaitForSeconds(2.5f);
        GameObject.Find("Medusa").GetComponent<PlayerMovement>().enabled = true;
        textWin.enabled = false;
        newWave();
    }

    public void Petrified(float distance)
    {
        enemiesPetrified++;
        updateScore(distance);
        //Debug.Log(distance);
        if (enemiesPetrified >= enemyGen.numOfEnemiesToSpawn)
        {
            //Initiate scoring screen
            StartCoroutine(win());
        }
        else if (pathfinding.IsBlockedIn())
        {
            Die(true);
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

    public void StatueSelling()
    {
        GameObject selector = Instantiate(Resources.Load<GameObject>("Selector"));
        selectorControl = selector.GetComponent<SelectorControl>();
        selectorControl.Select();        
    }
}