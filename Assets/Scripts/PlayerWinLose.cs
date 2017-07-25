using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerWinLose : MonoBehaviour
{
    int enemiesPetrified = 0;
    public Text textCounter;
    public Text textWin;
    public Text textPoints;
    public CanvasGroup diedText;
    public Text gameOverText;
    public Text statueText;

    //public int defaultPoint = 100;
    //public int proximityBonus = 50;
    private int currentScore = 0;
    private int statuesSold = 0;
    public int statuesToSell = 2;
    private bool isSelling;
    private List<GameObject> statuesSelected;

    private EnemyGenerator enemyGen;
    private StatueManager statueManager;
    private Animator medusaAnimator;
    private Pathfinding pathfinding;
    public bool isDead;

    private GameObject selector;
    private int positionInList = 0;
    private Map map;

    // Use this for initialization
    void Start()
    {
        map = GameObject.Find("Map").GetComponent<Map>();
        enemyGen = GameObject.FindObjectOfType<EnemyGenerator>();
        medusaAnimator = GameObject.Find("Medusa").GetComponent<Animator>();
        pathfinding = GameObject.Find("Medusa").GetComponent<Pathfinding>();
        statueManager = GameObject.Find("Map").GetComponent<StatueManager>();
        isSelling = false;
        statuesSelected = new List<GameObject>();
        isDead = false;
    }

    void Update()
    {
            if (isSelling)
            {
                if (Input.GetKeyDown("left"))
                {
                    if (positionInList != 0)
                    {
                        selector.transform.position = statueManager.statues[positionInList - 1].transform.position;
                        positionInList = positionInList - 1;
                    }
                    else
                    {
                        selector.transform.position = statueManager.statues[statueManager.statues.Count - 1].transform.position;
                        positionInList = statueManager.statues.Count - 1;
                    }
                }
                if (Input.GetKeyDown("right"))
                {
                    if (positionInList != statueManager.statues.Count - 1)
                    {
                        selector.transform.position = statueManager.statues[positionInList + 1].transform.position;
                        positionInList = positionInList + 1;
                    }
                    else
                    {
                        selector.transform.position = statueManager.statues[0].transform.position;
                        positionInList = 0;
                    }
                }
                if (Input.GetKeyDown("space"))
                {
                    if (statueManager.statues[positionInList].GetComponent<Statue>().pIsSold)
                    {                        
                        statueManager.statues[positionInList].GetComponent<Statue>().pIsSold = false;
                        statuesSold -= 1;
                        statueManager.statues[positionInList].transform.Find("SelectionIndicator").gameObject.SetActive(false);
                        statuesSelected.Remove(statueManager.statues[positionInList]);
                    }
                    else if (!statueManager.statues[positionInList].GetComponent<Statue>().pIsSold)
                    {
                        if (statuesSold < 2)
                        {
                            statueManager.statues[positionInList].GetComponent<Statue>().pIsSold = true;
                            statuesSold += 1;
                            statueManager.statues[positionInList].transform.Find("SelectionIndicator").gameObject.SetActive(true);
                            statuesSelected.Add(statueManager.statues[positionInList]);
                        }
                    }
                }
                if (Input.GetKeyDown("return"))
                {
                    if (statuesSold == 2)
                    {
                        foreach(GameObject statue in statuesSelected)
                        {
                            //TODO: Implement auction screen
                            currentScore += statue.GetComponent<Statue>().pPointsWorth;
                            map.removeStatue(statue.transform.position);
                            Destroy(statue);
                        }
                        Destroy(selector);
                        statuesSelected.Clear();
                        statuesSold = 0;
                        isSelling = false;
                        updateScore();
                        foreach (GameObject statue in statueManager.statues)
                        {
                            statue.GetComponent<Statue>().HideStars();
                        }
                        GameObject.Find("Medusa").GetComponent<Animator>().speed = 1;
                        StartCoroutine(NextWave());
                    }
                }
            }
    }

    public void Die(bool starved)
    {
        //Show Death UI and wait for a moment
        if (!isDead)
        {
            StartCoroutine(DieDie(starved));
        }
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

    IEnumerator Win()
    {
        GameObject.Find("Medusa").GetComponent<PlayerMovement>().enabled = false;
        GameObject.Find("Medusa").GetComponent<AudioSource>().mute = true;
        GameObject.Find("Medusa").GetComponent<Animator>().speed = 0;

        statueText.enabled = true;
        yield return new WaitForSeconds(2);
        statueText.enabled = false;

        selector = Instantiate(Resources.Load<GameObject>("Selector"));
        selector.transform.position = statueManager.statues[positionInList].transform.position;

        foreach (GameObject statue in statueManager.statues)
        {
            statue.GetComponent<Statue>().DisplayStars();
        }

        isSelling = true;
    }

    public void Petrified()
    {
        enemiesPetrified++;
        updateScore();
        //Debug.Log(distance);
        if (enemiesPetrified >= enemyGen.numOfEnemiesToSpawn)
        {
            //Initiate scoring screen
            StartCoroutine(Win());
        }
        else if (pathfinding.IsBlockedIn())
        {
            Die(true);
        }

    }

    void updateScore()
    {
        textCounter.text = "HEROES LEFT: " + (enemyGen.numOfEnemiesToSpawn - enemiesPetrified);
        textPoints.text = "DRACHMAE: " + (currentScore);
    }

    public void newWave()
    {
        enemyGen.numOfEnemiesToSpawn += enemyGen.howManyToAdd;
        enemyGen.SpawnEnemies(enemyGen.numOfEnemiesToSpawn);
        textCounter.text = "HEROES LEFT: " + (enemyGen.numOfEnemiesToSpawn);
        enemiesPetrified = 0;
    }

    IEnumerator NextWave()
    {
        textWin.enabled = true;
        yield return new WaitForSeconds(2.5f);
        GameObject.Find("Medusa").GetComponent<PlayerMovement>().enabled = true;
        textWin.enabled = false;
        newWave();
    }
}