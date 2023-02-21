using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public bool activeRoom;

    public GameObject frontWall;

    public GameObject character;

    public Transform respawnLocation;

    public List<GameObject> interactableFurniture;
    public List<GameObject> furniture;

    public Dictionary<string, int> furnitureAmounts = new Dictionary<string, int>();

    public ValueControl valueHolder;

    //Room stats
    public float roomCoins;
    public float roomQuality;
    public float roomExperience;
    public float roomResearch;

    public void PayRent()
    {
        float payout;
        payout = roomQuality * 5;
        GameManager.instance.AddTheseValues(payout, 0);
    }
}
