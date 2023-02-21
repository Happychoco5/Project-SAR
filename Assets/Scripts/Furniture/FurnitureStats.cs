using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureStats : MonoBehaviour
{
    //List of the stats
    public string objectName;
    public float cost;
    public float upgradeCost;

    public float coins;
    public float quality;
    public float researchPoints;
    public float experiencePoints;

    public TMPro.TextMeshPro expText;
    public TMPro.TextMeshPro qualityText;
    public TMPro.TextMeshPro researchText;

    public MMFeedbacks[] feedbacks;

    public AudioClip[] gainCoin;
    public AudioClip[] gainExp;
    public AudioClip[] gainResearch;

    private void Start()
    {
        GameManager.instance.playerScript.valueHolder.coinText.text = "+" + coins;
        GameManager.instance.playerScript.valueHolder.expText.text = "+" + experiencePoints;
        GameManager.instance.playerScript.valueHolder.qualityText.text = "+" + quality;
        GameManager.instance.playerScript.valueHolder.researchText.text = "+" + researchPoints;
    }

    public void PlayFeedbacks()
    {

    }

    public void PlayAudio()
    {
        //play exp sound
        if (experiencePoints > 0)
        {
            int num = Random.Range(0, gainExp.Length);
            AudioSource.PlayClipAtPoint(gainExp[num], transform.position);
        }
        //play coin gain sound
        if (coins > 0)
        {
            int num = Random.Range(0, gainCoin.Length);
            AudioSource.PlayClipAtPoint(gainCoin[num], transform.position);
        }
        //play research gain sound
        if (researchPoints > 0)
        {
            int num = Random.Range(0, gainResearch.Length);
            AudioSource.PlayClipAtPoint(gainResearch[num], transform.position);
        }
    }
}
