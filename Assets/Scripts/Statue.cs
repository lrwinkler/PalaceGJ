using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour {

    public int pPointsWorth;
    public int pNumOfStars;
    public bool pIsSold;

    private string mAnimationState;
    private float mAnimationTime;

    // Use this for initialization
    void Start()
    {
        Debug.Log("statue spawned");
    }

    public void SetData(string animationState, float animationTime)
    {
        mAnimationState = animationState;
        mAnimationTime = animationTime;

        if (mAnimationState == "Soldier_walk_up"
            || mAnimationState == "Soldier_walk_down"
            || mAnimationState == "Soldier_walk_left"
            || mAnimationState == "Soldier_walk_right")
        {
            pPointsWorth = 300;
        }

        if (mAnimationState == "Soldier_attack_up"
            || mAnimationState == "Soldier_attack_down"
            || mAnimationState == "Soldier_attack_left"
            || mAnimationState == "Soldier_attack_right")
        {
            pPointsWorth = 300 + 20 * Mathf.RoundToInt((mAnimationTime % 1) * 100);
        }

        if (mAnimationState == "Soldier_alert")
        {
            pPointsWorth = 500;
        }

        if (pPointsWorth == 300)
        {
            pNumOfStars = 1;
        }
        else if (pPointsWorth > 300 && pPointsWorth < 600)
        {
            pNumOfStars = 2;
        }
        else if (pPointsWorth >= 600 && pPointsWorth < 900)
        {
            pNumOfStars = 3;
        }
        else if (pPointsWorth >= 900 && pPointsWorth < 1200)
        {
            pNumOfStars = 4;
        }
        else if (pPointsWorth > 1200)
        {
            pNumOfStars = 5;
        }

        Debug.Log(pNumOfStars + " " + pPointsWorth + " " + animationTime);
    }

    public void DisplayStars()
    {
        switch (pNumOfStars)
        {
            case 1:
                transform.Find("OneStar").gameObject.SetActive(true);
                break;

            case 2:
                transform.Find("TwoStars").gameObject.SetActive(true);
                break;

            case 3:
                transform.Find("ThreeStars").gameObject.SetActive(true);
                break;

            case 4:
                transform.Find("FourStars").gameObject.SetActive(true);
                break;

            case 5:
                transform.Find("FiveStars").gameObject.SetActive(true);
                break;
        }
    }

    public void HideStars()
    {
        switch (pNumOfStars)
        {
            case 1:
                transform.Find("OneStar").gameObject.SetActive(false);
                break;

            case 2:
                transform.Find("TwoStars").gameObject.SetActive(false);
                break;

            case 3:
                transform.Find("ThreeStars").gameObject.SetActive(false);
                break;

            case 4:
                transform.Find("FourStars").gameObject.SetActive(false);
                break;

            case 5:
                transform.Find("FiveStars").gameObject.SetActive(false);
                break;
        }
    }
}
