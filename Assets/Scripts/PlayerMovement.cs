using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float pMovementDuration;

    private Vector3 mMovementVector;
    private bool mIsMoving;


    // Use this for initialization
    void Start()
    {
        pMovementDuration = 0.1f;
        mIsMoving = false;
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
            }
            if (Input.GetKey("right"))
            {
                mMovementVector = (Vector3.right);
            }
            if (Input.GetKey("up"))
            {
                mMovementVector = (Vector3.up);
            }
            if (Input.GetKey("down"))
            {
                mMovementVector = (Vector3.down);
            }

            // Checks if mMovementVector doesn't imply diagonal movement, checks if tile moved to is passable terrain, then starts moving the player.
            if ((mMovementVector.x == 0 && mMovementVector.y != 0)
                || (mMovementVector.x != 0 && mMovementVector.y == 0))
            {
                //if(checkIfPassable())
                //{
                //PlayAnimation()
                Debug.Log(mMovementVector);
                mIsMoving = true;
                StartCoroutine(MovePlayer(transform.localPosition, transform.localPosition + mMovementVector, pMovementDuration));
                //ChangePlayerFacing();
                //}
                //else
                //{
                //ChangePlayerFacing();
                //}
                //mTimeCounter = 0.0f;
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

    /* TODO: Implement a check whether the tile to be moved to is passable terrain or not.
    private bool checkIfPassable()
    {

    }
    */
}