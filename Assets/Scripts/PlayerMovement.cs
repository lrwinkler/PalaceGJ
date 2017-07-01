using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Vector2 pCoordinates;
    public float mAllowedMovementFrequency;

    private Vector2 mMovementVector;
    private float mTimeCounter;
    private float mAnimationTime;

    // Use this for initialization
    void Start()
    {
        pCoordinates.Set(0, 0);
        mTimeCounter = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        mTimeCounter += Time.deltaTime;
        MovementInput();
    }

    private void MovementInput()
    {

        mMovementVector.Set((Input.GetAxisRaw("Horizontal")), Input.GetAxisRaw("Vertical"));

        if (mTimeCounter >= mAllowedMovementFrequency)
        {

            mTimeCounter = 0.0f;

            // Checks if mMovementVector doesn't imply diagonal movement, checks if tile moved to is passable terrain, then moves player object by the vector value.
            if ((mMovementVector.x == 0 && mMovementVector.y != 0)
                || (mMovementVector.x != 0 && mMovementVector.y == 0))
            {
                //if(checkIfPassable())
                //{
                //PlayAnimation()
                transform.Translate(mMovementVector.x, mMovementVector.y, 0);
                pCoordinates.x += mMovementVector.x;
                pCoordinates.y += mMovementVector.y;
                //}
            }
        }
    }

    /* TODO: Implement a check whether the tile to be moved to is passable terrain or not.
    private bool checkIfPassable()
    {

    }
    */
}