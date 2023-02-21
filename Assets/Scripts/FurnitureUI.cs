using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureUI : MonoBehaviour
{
    public GameObject furniture;
    public int currentFurniture;
    public int furnitureMax;

    public Text furnAmount;
    public Text furnName;

    //comment

    // Start is called before the first frame update
    void Start()
    {
        AddToRoom(GameManager.instance.currRoom);
        currentFurniture = GameManager.instance.currRoom.furnitureAmounts[furnName.text];
        furnAmount.text = currentFurniture.ToString() + "/" + furnitureMax.ToString();
    }

    public void AddToRoom(RoomScript room)
    {
        room.furnitureAmounts.Add(furnName.text, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectFurniture()
    {
        //Set the furniture in playerscript to furniture attached here
        //Call start placement
        if(currentFurniture < furnitureMax)
        {
            GameManager.instance.playerScript.FC.furnitureUI = this;
            GameManager.instance.playerScript.FC.furniture = furniture;
            GameManager.instance.playerScript.FC.StartPlacement();
        }
        else
        {
            Debug.Log("We have the max amount of this furiture!");
        }
    }

    public void UpdateUI(int currFurniture)
    {
        currentFurniture += currFurniture;
        furnAmount.text = currentFurniture.ToString() + "/" + furnitureMax.ToString();
    }
}
