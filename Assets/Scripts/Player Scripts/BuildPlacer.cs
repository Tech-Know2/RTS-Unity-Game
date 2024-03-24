using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class BuildPlacer : NetworkBehaviour
{
    //Script Connections
    public BuildingEffectManager buildingEffectManager;
    public Player playerScript;
    public UIController uiController;

    //Make sure that the right hting is being clicked
    public List<string> tileTags = new List<string>();

    //Display Vars
    public Vector3 scalingFactor;
    private Vector3 normalScaling;

    //Building vars
    public GameObject currentBuildSlot, previousBuildSlot, tileClicked;
    public Material previewMat;
    //public bool isPreviewing = false;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if(!base.IsOwner)
        {
            GetComponent<BuildPlacer>().enabled = false;
        }
    }

    public void Start()
    {
        uiController = playerScript.UIController;
    }

    public void BuildingSlotClick(GameObject obj)
    {
        currentBuildSlot = obj;

        if(previousBuildSlot == null)
        {
            //If this is the first thing clicked, scale it up
            previousBuildSlot = currentBuildSlot;

            ScaleUp(currentBuildSlot);

        } else if (previousBuildSlot != null && previousBuildSlot != currentBuildSlot) //If something before it was already scaled up, scale the previous down, and then scale the new one up
        {
            ScaleDown(previousBuildSlot);
            ScaleUp(currentBuildSlot);

            previousBuildSlot = currentBuildSlot;

        } else if (previousBuildSlot == currentBuildSlot)
        {
            ScaleDown(currentBuildSlot);

            previousBuildSlot = null;
            currentBuildSlot = null;
        }
    }

    private void ScaleUp(GameObject obj)
    {
        //Scale up the newly clicked object
        Vector3 currentScale = obj.transform.localScale;
        Vector3 newScale = currentScale + scalingFactor;
        obj.transform.localScale = newScale;
    }

    private void ScaleDown(GameObject obj)
    {
        Vector3 currentScale = obj.transform.localScale;
        Vector3 newScale = currentScale - scalingFactor;
        obj.transform.localScale = newScale;
    }

    public void Update()
    {
        // Check if the left mouse button is clicked and a building slot has been clicked
        if (Input.GetMouseButtonDown(0) && currentBuildSlot != null)
        {
            // Cast a ray from the mouse position into the scene
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits an object
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object's tag is contained in the tileTags list
                if (tileTags.Contains(hit.collider.tag))
                {
                    // Get the bounds of the clicked object
                    Bounds bounds = hit.collider.bounds;

                    // Calculate the y height of the top of the object
                    float topYHeight = bounds.max.y;

                    // Do something with the clicked object (e.g., call a method)
                    OnTileClicked(hit.collider.gameObject, topYHeight);
                }
            }
        }
    }

    void OnTileClicked(GameObject tileObject, float yHeight)
    {
        BuildingDataHolder buildingDataHolder = currentBuildSlot.GetComponent<BuildingDataHolder>();
        Building building = buildingDataHolder.attachedBuilding;

        // Make sure the building can be placed on this tile
        if (currentBuildSlot != null && isOccupied(tileObject) == false &&(building.acceptableBuildTiles.Contains(tileObject.tag) || building.acceptableBuildTiles.Count == 0) && ((building.requiresASettlement == true && tileUnderSettlement(tileObject) == true) || building.requiresASettlement == false) && ((building.isASettlement == true && settlementNotInArea(tileObject) == true) || building.isASettlement == false) && (hasPopRequired(tileObject, building) == true))
        {
            SpawnBuilding(building, buildingDataHolder.buildingObject, tileObject, yHeight, playerScript);
            
        } else 
        {
            playerScript.notificationController.CreateNotification("Can't Build There", "This building can not be built on that tile, please try again");
        }
    }

    [ServerRpc]
    private void SpawnBuilding(Building building, GameObject buildingObj, GameObject tileObject, float yHeight, Player player)
    {
        Debug.Log(currentBuildSlot);
        player.UIController.RemoveBuildElement(currentBuildSlot);

        ScaleDown(currentBuildSlot);
        currentBuildSlot = null;
        previousBuildSlot = null;

        HexController hex = tileObject.GetComponent<HexController>();
        hex.ChangeOccupancy(true);

        GameObject newBuilding = Instantiate(buildingObj, new Vector3(tileObject.transform.position.x, yHeight, tileObject.transform.position.z), tileObject.transform.rotation);
        
        //Have it be spawned on all clients pov
        base.Spawn(newBuilding, base.Owner);
            
        //Parent to the player
        player.ParentToMe(newBuilding);

        if(building.isASettlement == true)
        {
            player.buildingEffectManager.AddSettlement(newBuilding);
        } else 
        {
            player.buildingEffectManager.AddBuilding(newBuilding, tileObject);
        }
    }

    public bool tileUnderSettlement(GameObject tile)
    {
        foreach (Building settlement in buildingEffectManager.settlementData)
        {
            if(settlement.settlementTiles.Contains(tile))
            {
                return true;
            }
        }

        return false;
    }

    public bool settlementNotInArea(GameObject tile)
    {
        foreach (Building settlement in buildingEffectManager.settlementData)
        {
            if(settlement.settlementTiles.Contains(tile))
            {
                return false;
            }
        }

        return true;
    }

    public bool isOccupied(GameObject tile)
    {
        HexController hexData = tile.GetComponent<HexController>();

        if(hexData.isOccupied == true)
        {
            playerScript.notificationController.CreateNotification("Tile Occupied", "You are unable to build there as there is a building already occuping that tile");

            return true;
        } else 
        {
            return false;
        }
    }

    public Building getSettlementData(GameObject tile)
    {
        foreach (Building settlement in buildingEffectManager.settlementData)
        {
            if(settlement.settlementTiles.Contains(tile))
            {
                return settlement;
            }
        }

        return null;
    }

    public bool hasPopRequired(GameObject tile, Building building)
    {
        Building settlement = getSettlementData(tile);

        if(building.peopleInitialCost != 0)
        {
            if(settlement.settlementPopulation >= building.peopleInitialCost)
            {
                return true;
            } else 
            {
                return false;
            }
        } else 
        {
            return true;
        }
    }
}