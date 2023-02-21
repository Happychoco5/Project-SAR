using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject UI;
    public GameObject FurnitureDisplay;

    public PlayerScript playerScript;

    public RoomScript currRoom;

    public float coinsTotal;
    public float researchTotal;

    public Slider ExperienceBar;
    int currLevel;
    public float currExp;
    public float maxExp = 60;
    public float percent;

    public bool editMode;
    public bool liveMode;

    public List<GameObject> Rooms;
    public List<GameObject> possibleWaifus;

    private void Awake()
    {
        //If another instance of GameManager is open, destroy the clone
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        //Don't destroy this object on load
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        GetUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetUI()
    {
        UI = GameObject.Find("UI");
        FurnitureDisplay = UI.transform.Find("FurnitureDisplay").gameObject;
    }

    public void AddTheseValues(float coin, float research)
    {
        coinsTotal += coin;
        researchTotal += research;
        UI.transform.Find("CoinsText").GetComponent<Text>().text = "Coins: " + coinsTotal;
    }

    public void RemoveTheseValues(float coin, float research, float quality)
    {
        coinsTotal -= coin;
        UI.transform.Find("CoinsText").GetComponent<Text>().text = "Coins: " + coinsTotal;
    }

    #region Experience and Leveling
    //Controls the handling of gaining expereince and updating the player level
    public void GainExperience(float GainedExperience)
    {
        //Adds the gained experience to the current experience after rounding it to the nearest integer
        //Rounding it prevents it from returning a float
        currExp += Mathf.RoundToInt(GainedExperience);

        //Check if the current experience is equal to or greater than the maxExp.
        if (currExp >= maxExp)
        {
            //When returning true, level up the player.
            LevelUp();
        }
        //Updates the UI
        UpdateUI();
    }

    public void LevelUp()
    {
        //A temporary float that holds the value of the current Exp subtracted by the maximum
        float tempFloat;
        tempFloat = currExp - maxExp;

        //Reset the current experience to 0 and increase the maximum exp needed to level up
        currExp = 0;
        maxExp = maxExp + (Mathf.RoundToInt(maxExp * 0.5f));

        //Check to see if the temp float is larger than 0 so that we don't add negatives.
        if (tempFloat > 0)
        {
            //When true, add the remaining experience to the current experience (so the player doesn't lose exp)
            currExp += tempFloat;
        }
        //Increase the level
        currLevel++;
        Debug.Log("Ding! Level up! My current level: " + currLevel);

        //Updates the UI
        UpdateUI();
    }

    public void UpdateUI()
    {
        ExperienceBar = UI.transform.Find("ExperienceBar").GetComponent<Slider>();
        //Check if current experience is greater than 0 to prevent dividing by 0
        if (currExp > 0)
        {
            //When true, divide for the percent
            percent = (currExp / maxExp) * 100;
        }
        else
        {
            //When false, we know that the percent will always be 0 (because currExp has to be 0)
            percent = 0;
        }
        //Update the value with the new percent
        ExperienceBar.value = percent;

        Text levelText = UI.transform.Find("ExperienceBar").transform.Find("LevelText").GetComponent<Text>();
        levelText.text = currLevel.ToString();
    }
    #endregion
}
