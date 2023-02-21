using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacerScript : MonoBehaviour
{

    public CharacterLevelBar levelBar;

    public GameObject lookObject;

    //Character stats
    public int intelligence;
    public int stamina;
    public int charisma;
    public int fitness;
    public int creativity;

    //Character level stats
    public float characterExp;
    public float maxExp;
    public int characterLevel;

    float percent;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GainExperience(float gainedExperience)
    {
        float remainder;
        characterExp += gainedExperience;

        if (characterExp >= maxExp)
        {
            remainder = characterExp - maxExp;
            if(remainder > 0)
            {
                characterExp = remainder;
            }
            else
            {
                characterExp = 0;
            }
            //Play level up sound
            characterLevel++;
            maxExp = maxExp * 1.5f;
        }
        percent = (characterExp / maxExp);
        levelBar.StartAnim();
        levelBar.localScale.x = percent;
        levelBar.transform.localScale = levelBar.localScale;
        
    }
}
