using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueControl : MonoBehaviour
{
    public TMPro.TextMeshPro coinText;
    public TMPro.TextMeshPro expText;
    public TMPro.TextMeshPro qualityText;
    public TMPro.TextMeshPro researchText;

    public MMFeedbacks[] feedbacks;

    public void PlayFeedbacks()
    {
        //for (int i = 0; i < feedbacks.Length; i++)
        //{
        //feedbacks[i].PlayFeedbacks();
        //}

        feedbacks[0].PlayFeedbacks();
        feedbacks[1].PlayFeedbacks();
        feedbacks[2].PlayFeedbacks();
    }

    public void PlayValuesOnUse(float coins, float research)
    {
        coinText.text = "+" + coins;
        researchText.text = "+" + research;
        feedbacks[2].PlayFeedbacks();
        feedbacks[3].PlayFeedbacks();
    }
}
