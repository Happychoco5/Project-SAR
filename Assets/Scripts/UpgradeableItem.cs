using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class UpgradeableItem : MonoBehaviour
{
    public TMPro.TextMeshPro myText;

    public float gold;
    public float upgradeCost;

    public float time;

    public MMFeedbacks feedback;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= 2)
        {
            GainGold();
            time = 0;
        }
    }

    public void GainGold()
    {
        myText.color = Color.yellow;
        //GameManager.instance.AddCoins(gold);
        myText.text = "+" + gold.ToString("0.0");
        feedback.PlayFeedbacks();
    }

    public void Upgrade()
    {
        //Change the mesh after x amount of times upgraded
        //Upgrade the amount of coins given
        //increase the upgrade cost amount

        gold *= 2.5f;
        upgradeCost *= 4;
    }
}
