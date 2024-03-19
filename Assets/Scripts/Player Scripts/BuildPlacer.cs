using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPlacer : MonoBehaviour
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

        Debug.Log("Scaled up");
    }

    private void ScaleDown(GameObject obj)
    {
        Vector3 currentScale = obj.transform.localScale;
        Vector3 newScale = currentScale - scalingFactor;
        obj.transform.localScale = newScale;

        Debug.Log("Scaled down");
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
                    // Do something with the clicked object (e.g., call a method)
                    OnTileClicked(hit.collider.gameObject);
                }
            }
        }
    }

    void OnTileClicked(GameObject tileObject)
    {
        BuildingDataHolder buildingDataHolder = currentBuildSlot.GetComponent<BuildingDataHolder>();
        Building building = buildingDataHolder.attachedBuilding;

        // Make sure the building can be placed on this tile
        if (currentBuildSlot != null && (building.acceptableBuildTiles.Contains(tileObject.tag) || building.acceptableBuildTiles.Count == 0) && ((building.requiresASettlement == true && tileUnderSettlement(tileObject)) || building.requiresASettlement == false))
        {
            Debug.Log(currentBuildSlot);
            playerScript.UIController.RemoveBuildElement(currentBuildSlot);

            ScaleDown(currentBuildSlot);
            currentBuildSlot = null;
            previousBuildSlot = null;

            GameObject newBuilding = Instantiate(buildingDataHolder.buildingObject, new Vector3(tileObject.transform.position.x, tileObject.transform.position.y + 1, tileObject.transform.position.z), tileObject.transform.rotation);

            if(building.isASettlement == true)
            {
                playerScript.buildingEffectManager.AddSettlement(newBuilding);
            } else 
            {
                playerScript.buildingEffectManager.AddBuilding(newBuilding, tileObject);
            }
        } else 
        {
            playerScript.notificationController.CreateNotification("Can't Build There", "This building can not be built on that tile, please try again");
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

            return false;
        }

        return false;
    }

}
