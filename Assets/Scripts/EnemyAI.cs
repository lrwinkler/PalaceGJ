using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour 
{
	public float movementDelay = 1;
	public float steps = 10;
	public float stepDuration = 0.001f;

	public Sprite pEnemyUp;
	public Sprite pEnemyDown;
	public Sprite pEnemyRight;
	public Sprite pEnemyLeft;

	private Vector3 currentDirection;
	private Map gameMap;
	private float moveStep;

	public enum facingDirection { up, down, left, right };
	private facingDirection mNextFacingDirection;
	public facingDirection pEnemyFacing;

	private SpriteRenderer mSpriteRenderer;

	private AudioSource myAudio;
	// Use this for initialization
	void Start () 
	{
		moveStep = 1 / steps;
		gameMap = FindObjectOfType<Map>();
		InvokeRepeating("MoveEnemy", 0, movementDelay);
		mSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		pEnemyFacing = facingDirection.down;
		myAudio = GetComponent<AudioSource>();
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
		if (pEnemyFacing != mNextFacingDirection)
		{
			switch (mNextFacingDirection)
			{
				case facingDirection.up:
					mSpriteRenderer.sprite = pEnemyUp;
					pEnemyFacing = facingDirection.up;
					break;

				case facingDirection.down:
					mSpriteRenderer.sprite = pEnemyDown;
					pEnemyFacing = facingDirection.down;
					break;

				case facingDirection.left:
					mSpriteRenderer.sprite = pEnemyLeft;
					pEnemyFacing = facingDirection.left;
					break;

				case facingDirection.right:
					mSpriteRenderer.sprite = pEnemyRight;
					pEnemyFacing = facingDirection.right;
					break;
			}
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
}
