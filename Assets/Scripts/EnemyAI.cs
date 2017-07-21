using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float movementDelay = 1;
    public float steps = 10;
    public float stepDuration = 0.001f;
    public Vector3 destinationVector;

    private Vector3 currentDirection;
    private Vector3 currentEnemyPosition;
    private GameObject player;
    private Vector3 currentPlayerPosition;
    private Map gameMap;
    private float moveStep;

    private bool isPetrified;

    public enum facingDirection { up, down, left, right };
    private facingDirection mNextFacingDirection;
    public facingDirection pEnemyFacing;

    private AudioSource myAudio;

    private Animator animator;

    private Transform horizontalAnimationChild;
    private Transform upAnimationChild;
    private Transform downAnimationChild;
    private bool isAlerted;
    private float timer;


    void Start()
    {
        timer = 1;
        moveStep = 1 / steps;
        gameMap = FindObjectOfType<Map>();
        //InvokeRepeating("MoveEnemy", 0, movementDelay);
        pEnemyFacing = facingDirection.down;
        myAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        horizontalAnimationChild = transform.Find("HorizontalAnimation");
        upAnimationChild = transform.Find("UpAnimation");
        downAnimationChild = transform.Find("DownAnimation");
        player = GameObject.Find("Medusa");
    }

    private void Update()
    {
        currentEnemyPosition = transform.position;
        currentPlayerPosition = player.transform.position;
        animator.SetBool("isAttacking", false);
        animator.SetBool("facingUp", false);
        animator.SetBool("facingDown", false);
        animator.SetBool("facingLeft", false);
        animator.SetBool("facingRight", false);
        animator.SetBool("isAlerted", false);

        if (GameObject.Find("Medusa").GetComponent<PlayerWinLoose>().isDead)
        {
            isAlerted = false;
        }

        if (isAlerted)
        {
            transform.GetComponent<EnemyPathfinding>().FindPath();
        }

        timer += Time.deltaTime;

        if (animator.GetBool("isAttacking") == false)
        {
            if ((currentEnemyPosition + Vector3.right == currentPlayerPosition && pEnemyFacing == facingDirection.right)
                || (currentEnemyPosition + Vector3.up == currentPlayerPosition && pEnemyFacing == facingDirection.up)
                || (currentEnemyPosition + Vector3.down == currentPlayerPosition && pEnemyFacing == facingDirection.down)
                || (currentEnemyPosition + Vector3.left == currentPlayerPosition && pEnemyFacing == facingDirection.left))
            {
                animator.SetBool("isAttacking", true);
            }
            else if (timer >= movementDelay)
            {
                MoveEnemy();

                timer = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        switch (pEnemyFacing)
        {
            case facingDirection.down:
                RaycastHit2D hitDown = Physics2D.Raycast(transform.position + new Vector3(0,0,-1), Vector2.down);
                //Debug.Log(hitDown.collider.name);
                if (hitDown.collider.name == "Medusa")
                {
                    Alert();
                    //Debug.Log("Spotted!");
                }
                break;

            case facingDirection.up:
                RaycastHit2D hitUp = Physics2D.Raycast(transform.position + new Vector3(0, 0, -1), Vector2.up);
                //Debug.Log(hitUp.collider.name);
                if (hitUp.collider.name == "Medusa")
                {
                    Alert();
                    //Debug.Log("Spotted!");
                }
                break;

            case facingDirection.left:
                RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + new Vector3(0, 0, -1), Vector2.left);
                //Debug.Log(hitLeft.collider.name);
                if (hitLeft.collider.name == "Medusa")
                {
                    Alert();
                    //Debug.Log("Spotted!");
                }
                break;

            case facingDirection.right:
                RaycastHit2D hitRight = Physics2D.Raycast(transform.position + new Vector3(0, 0, -1), Vector2.right);
                //Debug.Log(hitRight.collider.name);
                if (hitRight.collider.name == "Medusa")
                {
                    Alert();
                    //Debug.Log("Spotted!");
                }
                break;
        }
    }

    void MoveEnemy()
    {
        StartCoroutine(LerpEnemy());
    }

    IEnumerator LerpEnemy()
    {
        if (isAlerted)
        {
            currentDirection = transform.GetComponent<EnemyPathfinding>().nextStep;
        }
        else
        {
            currentDirection = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0);
        }
        //No movement in both axis at once!!
        if (currentDirection.x != 0 && currentDirection.y != 0)
        {
            currentDirection.y = 0;
        }
        //Move if it's free tile.
        if (gameMap != null)
        {
            destinationVector = new Vector3(transform.position.x + currentDirection.x, transform.position.y + currentDirection.y, transform.position.z);
            if (gameMap.canPass(destinationVector))
            {
                if (!isBlockedByEnemy())
                {
                    if (!isBlockedByPlayer())
                    {
                        if (currentDirection.x == 1)
                            mNextFacingDirection = facingDirection.right;
                        else if (currentDirection.x == -1)
                            mNextFacingDirection = facingDirection.left;
                        else if (currentDirection.y == 1)
                            mNextFacingDirection = facingDirection.up;
                        else if (currentDirection.y == -1)
                            mNextFacingDirection = facingDirection.down;
                        ChangeEnemyFacing();
                        Vector3 newDirection = new Vector3(transform.position.x + currentDirection.x, transform.position.y + currentDirection.y, transform.position.z);
                        Vector3 oldPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                        for (int i = 0; i <= steps; i++)
                        {
                            //Debug.Log(oldPosition);
                            transform.position = Vector3.Lerp(oldPosition, newDirection, moveStep * i);
                            yield return new WaitForSeconds(stepDuration);
                        }
                    }
                    else
                    {
                        if (currentDirection.x == 1)
                            mNextFacingDirection = facingDirection.right;
                        else if (currentDirection.x == -1)
                            mNextFacingDirection = facingDirection.left;
                        else if (currentDirection.y == 1)
                            mNextFacingDirection = facingDirection.up;
                        else if (currentDirection.y == -1)
                            mNextFacingDirection = facingDirection.down;
                        ChangeEnemyFacing();
                    }
                }
                else
                {
                    yield return new WaitForSeconds(0);                    
                }
            }
            else
            {
                MoveEnemy();
            }
        }
        yield return new WaitForSeconds(0);
    }

    void ChangeEnemyFacing()
    {
        if (horizontalAnimationChild != null && upAnimationChild != null && downAnimationChild != null)
        {
            if (pEnemyFacing != mNextFacingDirection)
            {
                switch (mNextFacingDirection)
                {
                    case facingDirection.up:

                        pEnemyFacing = facingDirection.up;

                        animator.SetBool("facingUp", true);
                        animator.SetBool("facingDown", false);
                        animator.SetBool("facingLeft", false);
                        animator.SetBool("facingRight", false);
                        break;

                    case facingDirection.down:

                        pEnemyFacing = facingDirection.down;

                        animator.SetBool("facingUp", false);
                        animator.SetBool("facingDown", true);
                        animator.SetBool("facingLeft", false);
                        animator.SetBool("facingRight", false);
                        break;

                    case facingDirection.left:

                        pEnemyFacing = facingDirection.left;

                        animator.SetBool("facingUp", false);
                        animator.SetBool("facingDown", false);
                        animator.SetBool("facingLeft", true);
                        animator.SetBool("facingRight", false);
                        break;

                    case facingDirection.right:

                        pEnemyFacing = facingDirection.right;

                        animator.SetBool("facingUp", false);
                        animator.SetBool("facingDown", false);
                        animator.SetBool("facingLeft", false);
                        animator.SetBool("facingRight", true);
                        break;
                }
            }
        }
        else
        {
            Debug.Log("It done fucked up!");
        }
    }

    public facingDirection GetFacingDirection()
    {
        return pEnemyFacing;
    }

    public void Petrify()
    {
        //get coordinates and round them
        Vector3 enemyCoords;

        enemyCoords.x = Mathf.Round(transform.position.x);
        enemyCoords.y = Mathf.Round(transform.position.y);
        enemyCoords.z = Mathf.Round(transform.position.z);

        string animationState = "";

        AnimatorStateInfo animationInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (animationInfo.IsName("Soldier_walk_left"))
        {
            animationState = "Soldier_walk_left";
        }

        if (animationInfo.IsName("Soldier_walk_right"))
        {
            animationState = "Soldier_walk_right";
        }

        if (animationInfo.IsName("Soldier_walk_up"))
        {
            animationState = "Soldier_walk_up";
        }

        if (animationInfo.IsName("Soldier_walk_down"))
        {
            animationState = "Soldier_walk_down";
        }

        if (animationInfo.IsName("Soldier_attack_down"))
        {
            animationState = "Soldier_attack_down";
        }

        if (animationInfo.IsName("Soldier_attack_left"))
        {
            animationState = "Soldier_attack_left";
        }

        if (animationInfo.IsName("Soldier_attack_right"))
        {
            animationState = "Soldier_attack_right";
        }

        if (animationInfo.IsName("Soldier_attack_up"))
        {
            animationState = "Soldier_attack_up";
        }

        //Debug.Log(animationState);

        float animationFrame = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
        //Debug.Log(animationState);

        gameMap.spawnStatue(1, enemyCoords, animationFrame, animationState);
        myAudio.Play();
        transform.localScale = Vector3.zero;
        Destroy(gameObject, myAudio.clip.length);
        isPetrified = true;
        transform.GetComponent<EnemyAI>().enabled = false;
    }

    void AttackFinish()
    {
        switch(pEnemyFacing)
        {
            case facingDirection.down:
                if (currentEnemyPosition + Vector3.down == currentPlayerPosition)
                {
                    if (!isPetrified)
                    {
                        player.GetComponent<PlayerWinLoose>().Die(false);
                        Debug.Log("SLAIN!");
                    }
                }
                break;

            case facingDirection.up:
                if (currentEnemyPosition + Vector3.up == currentPlayerPosition)
                {
                    if (!isPetrified)
                    {
                        player.GetComponent<PlayerWinLoose>().Die(false);
                        Debug.Log("SLAIN!");
                    }
                }
                break;

            case facingDirection.left:
                if (currentEnemyPosition + Vector3.left == currentPlayerPosition)
                {
                    if (!isPetrified)
                    {
                        player.GetComponent<PlayerWinLoose>().Die(false);
                        Debug.Log("SLAIN!");
                    }
                }
                break;

            case facingDirection.right:
                if (currentEnemyPosition + Vector3.right == currentPlayerPosition)
                {
                    if (!isPetrified)
                    {
                        player.GetComponent<PlayerWinLoose>().Die(false);
                        Debug.Log("SLAIN!");
                    }
                }
                break;
        }
    }

    void AttackAnimationFinish()
    {
        animator.SetBool("isAttacking", false);

        switch(pEnemyFacing)
        {
            case facingDirection.down:
                animator.SetBool("facingDown", true);
                break;

            case facingDirection.up:
                animator.SetBool("facingUp", true);
                break;

            case facingDirection.left:
                animator.SetBool("facingLeft", true);
                break;

            case facingDirection.right:
                animator.SetBool("facingRight", true);
                break;
        }
    }

    public void Alert()
    {
        if (!GameObject.Find("Medusa").GetComponent<PlayerWinLoose>().isDead && !isAlerted)
        {
            animator.SetBool("isAlerted", true);
            isAlerted = true;
        }     
    }

    private bool isBlockedByEnemy()
    {
        foreach (Transform child in GameObject.Find("EnemySpawner").transform)
        {
            if (child != transform)
            {
                if (destinationVector == child.GetComponent<EnemyAI>().destinationVector)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool isBlockedByPlayer()
    {
        if (destinationVector == player.transform.position)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
