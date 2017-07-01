using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float pMovementDuration;

    private Vector3 mMovementVector;
    private float mTimeCounter;


    // Use this for initialization
    void Start()
    {
        mTimeCounter = 1.0f;
        pMovementDuration = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        mTimeCounter += Time.deltaTime;

        if (mTimeCounter >= pMovementDuration*3)
        {
            MovementInput();
        }
    }

    private void MovementInput()
    {
        mTimeCounter = 0.0f;

        mMovementVector.Set((Input.GetAxisRaw("Horizontal")), Input.GetAxisRaw("Vertical"), 0);

        // Checks if mMovementVector doesn't imply diagonal movement, checks if tile moved to is passable terrain, then starts moving the player.
        if ((mMovementVector.x == 0 && mMovementVector.y != 0)
            || (mMovementVector.x != 0 && mMovementVector.y == 0))
        {
            //if(checkIfPassable())
            //{
            //PlayAnimation()
            Debug.Log(mMovementVector);
            StartCoroutine(MovePlayer(transform.localPosition, transform.localPosition + mMovementVector, pMovementDuration));
            //}
            //else
            //{
            //
        }

    }

    //Interpolates the move and ensures smooth transition between coordinates
    IEnumerator MovePlayer(Vector3 startingPosition, Vector3 endingPosition, float moveDuration)
    {
        int steps = (int) Mathf.Ceil(moveDuration * 60);

        float stepDuration = moveDuration / steps;

        float stepSize = 1.0f / steps;

        for (int i = 0; i <= steps; i++)
        {
            float lerpValue = i * stepSize;
            transform.localPosition = Vector3.Lerp(startingPosition, endingPosition, lerpValue);

            yield return new WaitForSeconds(stepDuration);
        }

        transform.localPosition = endingPosition;
    }

    /* TODO: Implement a check whether the tile to be moved to is passable terrain or not.
    private bool checkIfPassable()
    {

    }
    */
}