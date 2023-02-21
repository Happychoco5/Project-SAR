using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingControl : MonoBehaviour
{
    public GameObject button;

    public GameObject[] neighborSlots;

    public OverviewControl overviewControl;

    public GameObject housePrefab;
    public GameObject tempHouse;

    public void PointerDown()
    {
        //Get darker
    }
    public void PointerUp()
    {
        //Build the room
        BuildRoom();
    }

    public void PointerEnter()
    {
        //Get darker
    }
    public void PointerExit()
    {
        //Restore color
    }

    public void BuildRoom()
    {
        //Build the room

        tempHouse = Instantiate(housePrefab, new Vector3(overviewControl.houseDistance, 0, 0), housePrefab.transform.rotation);
        GameManager.instance.Rooms.Add(tempHouse);
        foreach(FurnitureUI furnUI in GameManager.instance.playerScript.FC.furnitureUIs)
        {
            furnUI.AddToRoom(tempHouse.GetComponent<RoomScript>());
        }

        for (int i = 0; i < neighborSlots.Length; i++)
        {
            neighborSlots[i].SetActive(true);
        }

        AstarPath.active.Scan();
        overviewControl.houseDistance += 5;

        Destroy(gameObject);
    }
}
