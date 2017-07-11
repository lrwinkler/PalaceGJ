using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorControl : MonoBehaviour
{

    Vector3 mMovementVector;
    Transform selectorTransform;
    int statuesSold;

    // Use this for initialization
    void Start()
    {
        mMovementVector = Vector3.zero;
        selectorTransform = transform;
        statuesSold = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
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
            selectorTransform.position = selectorTransform.position + mMovementVector;
        }

        mMovementVector.Set(0, 0, 0);

    }
}
