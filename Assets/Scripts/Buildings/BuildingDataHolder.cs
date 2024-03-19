using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingDataHolder : MonoBehaviour
{
    public Building attachedBuilding;
    public GameObject buildingObject;
    public TextMeshProUGUI buildingTitle, buildingDescription;
    public UIController uiController;
    public Player player;

    public void Start()
    {
        GetPlayerScript();
    }

    private void GetPlayerScript()
    {
        if(uiController.playerScript != null)
        {
            player = uiController.playerScript;
        }
    } 

    //Initiate the card placements
    public void GetBuilding()
    {
        if(player != null)
        {
            //Start the card placement process
            player.buildPlacer.BuildingSlotClick(gameObject);

        } else 
        {
            GetPlayerScript();
            GetBuilding();
        }
    }

    public void Display(Building buildingData)
    {
        attachedBuilding = buildingData;

        buildingTitle.text = buildingData.buildingName;
        buildingDescription.text = buildingData.buildingDescription;
    }
}
