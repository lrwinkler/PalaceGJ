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

    private Vector3 mMovementVector;
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
        mGameMap = FindObjectOfType<Map>();
        pMovementDuration = 0.1f;
        mIsMoving = false;
        mPlayerFacing = facingDirection.down;

        //mSpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        myAudio = GetComponent<AudioSource>();
        myWinLoose = GetComponent<PlayerWinLoose>();
        mAnimator = GetComponent<Animator>();
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
                RaycastHit2D hitDown = Physics2D.Raycast(transform.localPosition, Vector2.down);
                //Debug.Log(hitDown.collider.name);
                if (hitDown.collider.name == "Soldier(Clone)")
                {
                    //Debug.Log(hitDown.collider.name);
                    facingDirection enemyFacingUp = (PlayerMovement.facingDirection)hitDown.collider.GetComponent<EnemyAI>().GetFacingDirection();
                    if (enemyFacingUp == facingDirection.up)
                    {
                        Debug.Log(hitDown.collider.name + " contact below!");
                        myWinLoose.Petrified();
                        hitDown.collider.GetComponent<EnemyAI>().Petrify();
                    }
                }
                break;

            case facingDirection.up:
                RaycastHit2D hitUp = Physics2D.Raycast(transform.localPosition, Vector2.up);
                //Debug.Log(hitUp.collider.name);
                if (hitUp.collider.name == "Soldier(Clone)")
                {
                    //Debug.Log(hitUp.collider.name);
                    facingDirection enemyFacingDown = (PlayerMovement.facingDirection)hitUp.collider.GetComponent<EnemyAI>().GetFacingDirection();
                    if (enemyFacingDown == facingDirection.down)
                    {
                        Debug.Log(hitUp.collider.name + " contact above!");
                        myWinLoose.Petrified();
                        hitUp.collider.GetComponent<EnemyAI>().Petrify();
                    }
                }
                break;

            case facingDirection.left:
                RaycastHit2D hitLeft = Physics2D.Raycast(transform.localPosition, Vector2.left);
                //Debug.Log(hitLeft.collider.name);
                if (hitLeft.collider.name == "Soldier(Clone)")
                {
                    //Debug.Log(hitLeft.collider.name);
                    facingDirection enemyFacingRight = (PlayerMovement.facingDirection)hitLeft.collider.GetComponent<EnemyAI>().GetFacingDirection();
                    if (enemyFacingRight == facingDirection.right)
                    {
                        Debug.Log(hitLeft.collider.name + " contact left!");
                        myWinLoose.Petrified();
                        hitLeft.collider.GetComponent<EnemyAI>().Petrify();
                    }
                }
                break;

            case facingDirection.right:
                RaycastHit2D hitRight = Physics2D.Raycast(transform.localPosition, Vector2.right);
                //Debug.Log(hitRight.collider.name);
                if (hitRight.collider.name == "Soldier(Clone)")
                {
                    //Debug.Log(hitRight.collider.name);
                    facingDirection enemyFacingLeft = (PlayerMovement.facingDirection)hitRight.collider.GetComponent<EnemyAI>().GetFacingDirection();
                    if (enemyFacingLeft == facingDirection.left)
                    {
                        Debug.Log(hitRight.collider.name + " contact right!");
                        myWinLoose.Petrified();
                        hitRight.collider.GetComponent<EnemyAI>().Petrify();
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

            // Checks if mMovementVector doesn't imply diagonal movement, checks if tile moved to is passable terrain, then starts moving the player.
            if ((mMovementVector.x == 0 && mMovementVector.y != 0)
                || (mMovementVector.x != 0 && mMovementVector.y == 0))
            {
                if (mGameMap.canPass(transform.localPosition + mMovementVector))
                {

                    mIsMoving = true;
                    StartCoroutine(MovePlayer(transform.localPosition, transform.localPosition + mMovementVector, pMovementDuration));
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
}