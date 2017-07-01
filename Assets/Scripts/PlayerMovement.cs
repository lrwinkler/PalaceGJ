using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float pMovementDuration;
    public Sprite pPlayerUp;
    public Sprite pPlayerDown;
    public Sprite pPlayerRight;
    public Sprite pPlayerLeft;

    private Vector3 mMovementVector;
    private bool mIsMoving;
    private Map mGameMap;

    private enum facingDirection { up, down, left, right };
    private facingDirection mNextFacingDirection;
    private facingDirection mPlayerFacing;

    private SpriteRenderer mSpriteRenderer;

    // Use this for initialization
    void Start()
    {
        mGameMap = FindObjectOfType<Map>();
        pMovementDuration = 0.1f;
        mIsMoving = false;
        mPlayerFacing = facingDirection.down;

        mSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
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
                    //PlayAnimation()
                    Debug.Log(mMovementVector);
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

        for (int i = 0; i <= steps; i++)
        {
            float lerpValue = i * stepSize;
            transform.localPosition = Vector3.Lerp(startingPosition, endingPosition, lerpValue);

            yield return new WaitForSeconds(stepDuration);
        }

        transform.localPosition = endingPosition;
        mIsMoving = false;
    }

    void ChangePlayerFacing()
    {
        if (mPlayerFacing != mNextFacingDirection)
        {
            switch (mNextFacingDirection)
            {
                case facingDirection.up:
                    mSpriteRenderer.sprite = pPlayerUp;
                    mPlayerFacing = facingDirection.up;
                    break;

                case facingDirection.down:
                    mSpriteRenderer.sprite = pPlayerDown;
                    mPlayerFacing = facingDirection.down;
                    break;

                case facingDirection.left:
                    mSpriteRenderer.sprite = pPlayerLeft;
                    mPlayerFacing = facingDirection.left;
                    break;

                case facingDirection.right:
                    mSpriteRenderer.sprite = pPlayerRight;
                    mPlayerFacing = facingDirection.right;
                    break;
            }
        }
    }
}