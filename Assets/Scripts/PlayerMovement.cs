using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float pMovementDuration;
    //public Sprite pPlayerUp;
    //public Sprite pPlayerDown;
    //public Sprite pPlayerRight;
    //public Sprite pPlayerLeft;
    public float audioPitchRange = 0.5f;

    private GameObject enemySpawner;
    private Vector3 mMovementVector;
    private Vector3 mDestinationVector;
    private bool mIsMoving;
    private Map mGameMap;

    private enum facingDirection { up, down, left, right };
    private facingDirection mNextFacingDirection;
    private facingDirection mPlayerFacing;

    //private SpriteRenderer mSpriteRenderer;

    private AudioSource myAudio;
    private PlayerWinLoose myWinLoose;
    private Animator mAnimator;

    // Use this for initialization
    void Start()
    {
        enemySpawner = GameObject.Find("EnemySpawner");
        mGameMap = FindObjectOfType<Map>();
        pMovementDuration = 0.1f;
        mIsMoving = false;
        mPlayerFacing = facingDirection.down;

        //mSpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        myAudio = GetComponent<AudioSource>();
        myWinLoose = GetComponent<PlayerWinLoose>();
        mAnimator = GetComponent<Animator>();
        mAnimator.SetBool("IsDead", false);
    }

    // Update is called once per frame
    void Update()
    {
        mAnimator.SetBool("FacingUp", false);
        mAnimator.SetBool("FacingDown", false);
        mAnimator.SetBool("FacingRight", false);
        mAnimator.SetBool("FacingLeft", false);
        MovementInput();
    }

    private void FixedUpdate()
    {
        switch (mPlayerFacing)
        {
            case facingDirection.down:
                RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down);
                //Debug.Log(hitDown.collider.name);
                if (hitDown.collider.name == "Soldier(Clone)")
                {
                    //Debug.Log(hitDown.collider.name);
                    facingDirection enemyFacingUp = (PlayerMovement.facingDirection)hitDown.collider.GetComponent<EnemyAI>().GetFacingDirection();
                    if (enemyFacingUp == facingDirection.up)
                    {
                        //Debug.Log(hitDown.collider.name + " contact below!");                        
                        hitDown.collider.GetComponent<EnemyAI>().Petrify();
                        myWinLoose.Petrified(hitDown.distance);
                    }
                }
                break;

            case facingDirection.up:
                RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up);
                //Debug.Log(hitUp.collider.name);
                if (hitUp.collider.name == "Soldier(Clone)")
                {
                    //Debug.Log(hitUp.collider.name);
                    facingDirection enemyFacingDown = (PlayerMovement.facingDirection)hitUp.collider.GetComponent<EnemyAI>().GetFacingDirection();
                    if (enemyFacingDown == facingDirection.down)
                    {
                        //Debug.Log(hitUp.collider.name + " contact above!");                        
                        hitUp.collider.GetComponent<EnemyAI>().Petrify();
                        myWinLoose.Petrified(hitUp.distance);
                    }
                }
                break;

            case facingDirection.left:
                RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left);
                //Debug.Log(hitLeft.collider.name);
                if (hitLeft.collider.name == "Soldier(Clone)")
                {
                    //Debug.Log(hitLeft.collider.name);
                    facingDirection enemyFacingRight = (PlayerMovement.facingDirection)hitLeft.collider.GetComponent<EnemyAI>().GetFacingDirection();
                    if (enemyFacingRight == facingDirection.right)
                    {
                        //Debug.Log(hitLeft.collider.name + " contact left!");                        
                        hitLeft.collider.GetComponent<EnemyAI>().Petrify();
                        myWinLoose.Petrified(hitLeft.distance);
                    }
                }
                break;

            case facingDirection.right:
                RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right);
                //Debug.Log(hitRight.collider.name);
                if (hitRight.collider.name == "Soldier(Clone)")
                {
                    //Debug.Log(hitRight.collider.name);
                    facingDirection enemyFacingLeft = (PlayerMovement.facingDirection)hitRight.collider.GetComponent<EnemyAI>().GetFacingDirection();
                    if (enemyFacingLeft == facingDirection.left)
                    {
                        //Debug.Log(hitRight.collider.name + " contact right!");                        
                        hitRight.collider.GetComponent<EnemyAI>().Petrify();
                        myWinLoose.Petrified(hitRight.distance);
                    }
                }
                break;
        }
    }

    private void MovementInput()
    {

        if (!mIsMoving)
        {

            if (Input.GetKey("left"))
            {
                mMovementVector = (Vector3.left);
                mNextFacingDirection = facingDirection.left;
            }
            if (Input.GetKey("right"))
            {
                mMovementVector = (Vector3.right);
                mNextFacingDirection = facingDirection.right;
            }
            if (Input.GetKey("up"))
            {
                mMovementVector = (Vector3.up);
                mNextFacingDirection = facingDirection.up;
            }
            if (Input.GetKey("down"))
            {
                mMovementVector = (Vector3.down);
                mNextFacingDirection = facingDirection.down;
            }
            if (Input.GetKey("space"))
            {
                Hiss();
                return;
            }

            // Checks if mMovementVector doesn't imply diagonal movement, checks if tile moved to is passable terrain, then starts moving the player.
            if ((mMovementVector.x == 0 && mMovementVector.y != 0)
                || (mMovementVector.x != 0 && mMovementVector.y == 0))
            {
                mDestinationVector = transform.localPosition + mMovementVector;

                if (mGameMap.canPass(mDestinationVector) && !isEnemyThere())
                {
                    mIsMoving = true;
                    StartCoroutine(MovePlayer(transform.localPosition, mDestinationVector, pMovementDuration));
                    ChangePlayerFacing();
                }
                else
                {
                    ChangePlayerFacing();
                }
            }

            mMovementVector.Set(0, 0, 0);
        }
    }

    //Interpolates the move and ensures smooth transition between coordinates
    IEnumerator MovePlayer(Vector3 startingPosition, Vector3 endingPosition, float moveDuration)
    {
        int steps = (int)Mathf.Ceil(moveDuration * 60);

        float stepDuration = moveDuration / steps;

        float stepSize = 1.0f / steps;
        myAudio.pitch = Random.Range(1 - audioPitchRange, 1 + audioPitchRange);
        myAudio.Play();
        for (int i = 0; i <= steps; i++)
        {
            float lerpValue = i * stepSize;
            transform.localPosition = Vector3.Lerp(startingPosition, endingPosition, lerpValue);

            yield return new WaitForSeconds(stepDuration);
        }

        transform.localPosition = endingPosition;
        mIsMoving = false;
    }

    //Rotates player character by changing the used animation.
    void ChangePlayerFacing()
    {
        if (mPlayerFacing != mNextFacingDirection)
        {
            switch (mNextFacingDirection)
            {
                case facingDirection.up:
                    //mSpriteRenderer.sprite = pPlayerUp;
                    mAnimator.SetBool("FacingUp", true);
                    mAnimator.SetBool("FacingDown", false);
                    mAnimator.SetBool("FacingRight", false);
                    mAnimator.SetBool("FacingLeft", false);

                    mPlayerFacing = facingDirection.up;
                    break;

                case facingDirection.down:
                    //mSpriteRenderer.sprite = pPlayerDown;
                    mAnimator.SetBool("FacingUp", false);
                    mAnimator.SetBool("FacingDown", true);
                    mAnimator.SetBool("FacingRight", false);
                    mAnimator.SetBool("FacingLeft", false);

                    mPlayerFacing = facingDirection.down;
                    break;

                case facingDirection.left:
                    //mSpriteRenderer.sprite = pPlayerLeft;
                    mAnimator.SetBool("FacingUp", false);
                    mAnimator.SetBool("FacingDown", false);
                    mAnimator.SetBool("FacingRight", false);
                    mAnimator.SetBool("FacingLeft", true);

                    mPlayerFacing = facingDirection.left;
                    break;

                case facingDirection.right:
                    //mSpriteRenderer.sprite = pPlayerRight;
                    mAnimator.SetBool("FacingUp", false);
                    mAnimator.SetBool("FacingDown", false);
                    mAnimator.SetBool("FacingRight", true);
                    mAnimator.SetBool("FacingLeft", false);

                    mPlayerFacing = facingDirection.right;
                    break;
            }
        }
    }

    private bool isEnemyThere()
    {
        foreach (Transform child in enemySpawner.transform)
        {
            if (child.gameObject.transform.position == mDestinationVector)
            {
                child.GetComponent<EnemyAI>().Alert(false);
                return true;
            }
        }
        return false;
    }

    private void Hiss()
    {
        Vector3 playerPosition = transform.position;
        playerPosition.x = Mathf.RoundToInt(playerPosition.x);
        playerPosition.y = Mathf.RoundToInt(playerPosition.y);
        ContactFilter2D contactFilter = new ContactFilter2D();

        Collider2D[] area = new Collider2D[enemySpawner.transform.childCount];


        Physics2D.OverlapArea(new Vector2(playerPosition.x - 2, playerPosition.y + 2), new Vector2(playerPosition.x + 2, playerPosition.y - 2), contactFilter, area);

        foreach(Transform child in enemySpawner.transform)
        {
            if (((IList<Collider2D>)area).Contains(child.GetComponent<Collider2D>()))
            {
                child.GetComponent<EnemyAI>().Alert(true);
            }
        }
    }
}