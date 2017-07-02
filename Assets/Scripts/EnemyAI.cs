using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour 
{
	public float movementDelay = 1;
	public float steps = 10;
	public float stepDuration = 0.001f;

	private Vector3 currentDirection;
	private Map gameMap;
	private float moveStep;

	public enum facingDirection { up, down, left, right };
	private facingDirection mNextFacingDirection;
	public facingDirection pEnemyFacing;

	private AudioSource myAudio;

    private Animator animator;

    private Transform horizontalAnimationChild;
    private Transform upAnimationChild;
    private Transform downAnimationChild;
	// Use this for initialization
	void Start () 
	{
		moveStep = 1 / steps;
		gameMap = FindObjectOfType<Map>();
        InvokeRepeating("MoveEnemy", 0, movementDelay);
		pEnemyFacing = facingDirection.down;
		myAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        horizontalAnimationChild = transform.Find("HorizontalAnimation");
        upAnimationChild = transform.Find("UpAnimation");
        downAnimationChild = transform.Find("DownAnimation");
	}

	void MoveEnemy()
	{
		StartCoroutine(LerpEnemy());
	}

	IEnumerator LerpEnemy()
	{
		currentDirection = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0);

		//No movement in both axis at once!!
		if (currentDirection.x != 0 && currentDirection.y != 0)
		{
			currentDirection.y = 0;
		}
		//Move if it's free tile.
		if (gameMap != null)
		{
			if (gameMap.canPass(new Vector3(transform.position.x + currentDirection.x, transform.position.y + currentDirection.y, transform.position.z)))
			{
				if (currentDirection.x == 1)
					mNextFacingDirection = facingDirection.right;
				else if (currentDirection.x == -1)
					mNextFacingDirection = facingDirection.left;
				else if (currentDirection.y == 1)
					mNextFacingDirection = facingDirection.up;
				else if (currentDirection.y == -1)
					mNextFacingDirection = facingDirection.down;
				ChangePlayerFacing();
				Vector3 newDirection = new Vector3(transform.position.x + currentDirection.x, transform.position.y + currentDirection.y, transform.position.z);
				Vector3 oldPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
				for (int i = 0; i <= steps; i++)
				{
					//Debug.Log(oldPosition);
					transform.position = Vector3.Lerp(oldPosition, newDirection, moveStep*i);
					yield return new WaitForSeconds(stepDuration);
				}
			}
			else
			{
				MoveEnemy();
			}
		}
		yield return new WaitForSeconds(0);
	}

	void ChangePlayerFacing()
	{
        if (horizontalAnimationChild != null && upAnimationChild != null && downAnimationChild != null)
        {
            if (pEnemyFacing != mNextFacingDirection)
            {
                switch (mNextFacingDirection)
                {
                    case facingDirection.up:

                        pEnemyFacing = facingDirection.up;

                        horizontalAnimationChild.gameObject.SetActive(false);
                        downAnimationChild.gameObject.SetActive(false);
                        upAnimationChild.gameObject.SetActive(true);

                        //animator.Play("Soldier_walk_up", 0);
                        break;

                    case facingDirection.down:

                        pEnemyFacing = facingDirection.down;
                        horizontalAnimationChild.gameObject.SetActive(false);
                        downAnimationChild.gameObject.SetActive(true);
                        upAnimationChild.gameObject.SetActive(false);

                        //animator.Play("Soldier_walk_down", 0);
                        break;

                    case facingDirection.left:

                        pEnemyFacing = facingDirection.left;
                        horizontalAnimationChild.gameObject.SetActive(true);
                        downAnimationChild.gameObject.SetActive(false);
                        upAnimationChild.gameObject.SetActive(false);

                        //animator.Play("Soldier_walk_left", 0);
                        break;

                    case facingDirection.right:

                        pEnemyFacing = facingDirection.right;
                        horizontalAnimationChild.gameObject.SetActive(true);
                        downAnimationChild.gameObject.SetActive(false);
                        upAnimationChild.gameObject.SetActive(false);

                        //animator.Play("Soldier_walk_right", 0);
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
        //get coordinates
        gameMap.spawnStatue(1, transform.position);
        myAudio.Play();
		transform.localScale = Vector3.zero;
        Destroy(gameObject, myAudio.clip.length);
        //display statue
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.GetComponent<PlayerWinLoose>().Die();
		}

	}
}
