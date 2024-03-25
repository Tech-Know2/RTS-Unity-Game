using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEffectManager : MonoBehaviour
{
    //Script Connections
    public Player playerScript;
    public UIController uiController;

    public List<Building> settlementData = new List<Building>();
    public List<GameObject> settlementObjs = new List<GameObject>();
    public float settlementRadius = 1f;
    public List<string> tileTags = new List<string>();

    public List<Building> buildingData = new List<Building>();
    public List<GameObject> buildingObjs = new List<GameObject>();
    public List<BuildData> buildingServerData = new List<BuildData>();

    //Player Empire Vars
    private Empire playerEmpire;
    
    //Settlement Upgrade Vars
    public int peoplePerSettlementUpgrade = 50;
    public int buildingsPerSettlementUpgrade = 6;
    public int maxSettlementLevel = 3;

    public void Start()
    {
        uiController = playerScript.UIController;
        playerEmpire = playerScript.playerEmpire;
    }

    public void AddBuilding(GameObject obj, GameObject tile)
    {
        //Duplicate the data
        Building preData = obj.GetComponent<BuildData>().buildData;
        Building data = Instantiate(preData);

        //Store the data and the objects
        buildingData.Add(data);
        buildingObjs.Add(obj);

        //Manipulate scriptable object up here, then reasign down there
        if(data.requiresASettlement == true)
        {
            foreach (Building settlement in settlementData)
            {
                if(settlement.settlementTiles.Contains(tile))
                {
                    data.buildingsParentSettlement = settlement;
                }
            }
        }

        //Reasign the data to the objects
        BuildData objBuildData = obj.GetComponent<BuildData>();
        objBuildData.buildData = data;

        //Store the data on the server/client side of things
        buildingServerData.Add(objBuildData);

        //Do building effects
        CreateBuilding(data, objBuildData);

        playerScript.notificationController.CreateNotification("Building Built", data.buildingName + " has been built in your empire");
    }

    public void AddSettlement(GameObject obj)
    {
        //Duplicate the data
        Building preData = obj.GetComponent<BuildData>().buildData;
        Building data = Instantiate(preData);

        //Get the tiles under the settlement and add them to the settlement data
        data = GetSettlementTiles(obj, data);
        data.isCapital = isCapital(data);

        //Store the data and the objects
        settlementData.Add(data);
        settlementObjs.Add(obj);

        data.settlementName = playerScript.empireManager.nameCreator.GenerateCityName();

        playerScript.notificationController.CreateNotification("Settlement Founded", data.settlementName + " has been added to your empire");

        //Reasign the data to the objects
        BuildData objBuildData = obj.GetComponent<BuildData>();
        objBuildData.SetBuildData(data);

        //Update on the server
        buildingServerData.Add(objBuildData);
    }

    public void IntervalEffects()
    {
        BuildingEffects();
        SettlementEffects();
    }

    private void BuildingEffects()
    {
        foreach (Building building in buildingData)
        {
            int index = buildingData.IndexOf(building);
            BuildData buildData = buildingServerData[index];

            BuildingUpKeep(building, buildData);
            BuildingProduction(building, buildData);
            BuildingProduction(building, buildData);
            BuildingTraining(building, buildData);
        }
    }

    private void SettlementEffects()
    {
        foreach (Building settlement in settlementData)
        {
            int index = buildingData.IndexOf(settlement);
            BuildData buildData = buildingServerData[index];

            UpdatePopandFood(settlement, buildData);
            CalcReligion(settlement, buildData);
            UpgradeSettlement(settlement, buildData);
            SettlementIncome(settlement, buildData);
        }
    }

    public void SettlementIncome(Building data, BuildData buildData)
    {
        playerScript.gameManager.playerGold += data.settlementPopulation;
    }

    //Functions to calculate and manage settlement needs
    public void UpdatePopandFood(Building data, BuildData buildData)
    {
        /*int foodConsumed = data.settlementPopulation * 2; // 1 pop consumes 2 food

        // Ensure that food consumption does not exceed available food
        if (foodConsumed > data.settlementFood)
        {
            foodConsumed = data.settlementFood;
        }

        // Deduct food consumed
        data.settlementFood -= foodConsumed;

        // Calculate food available for population growth (half of remaining food after consumption)
        int foodForGrowth = (data.settlementFood) / 2;

        // Calculate new population based on available food for growth
        int newPopulation = foodForGrowth;

        // Update population
        data.settlementPopulation += newPopulation;

        // Deduct food used for population growth
        data.settlementFood -= newPopulation * 2;
        */
    }

    public void CalcReligion(Building data, BuildData buildData)
    {
        if((data.settlementReligiousFollowers.Count != 0) || (data.settlementReligions.Count != 0))
        {
            List<int> followerCount = new List<int>();

            foreach (Object follower in data.settlementReligiousFollowers)
            {
                Religion currentReligion = follower.followerReligion;

                if (!data.settlementReligions.Contains(follower.followerReligion))
                {
                    data.settlementReligions.Add(follower.followerReligion);

                    followerCount.Add(0);
                }
            }

            for (int r = 0; r < data.settlementReligions.Count; r++)
            {
                Religion religion = data.settlementReligions[r];

                for (int f = 0; f < data.settlementReligiousFollowers.Count; f++)
                {
                    Object follower = data.settlementReligiousFollowers[f];

                    if (follower.followerReligion == religion)
                    {
                        followerCount[r] += 1;
                    }
                }
            }

            int currentMaxElement = 0; // Store element id
            int currentMaxNum = 0; // Store max follower count

            for (int i = 0; i < followerCount.Count; i++)
            {
                if (followerCount[i] > currentMaxNum)
                {
                    currentMaxNum = followerCount[i];
                    currentMaxElement = i;
                }
            }

            data.mainReligion = data.settlementReligions[currentMaxElement];

            //Update values on the server side of things
            buildData.SetBuildData(data);
        }
    }

    public Building GetSettlementTiles(GameObject obj, Building data)
    {
        float radius = settlementRadius * data.settlementLevel;

        Collider[] colliders = Physics.OverlapSphere(obj.transform.position, radius);

        // Iterate through all colliders and filter objects with desired tags
        foreach (Collider collider in colliders)
        {
            // Check to see if colliders match
            if (tileTags.Contains(collider.tag))
            {
                GameObject tile = collider.gameObject;
                HexController hexController = tile.GetComponent<HexController>();
                
                // Check to make sure tile is not already apart of the settlement
                // Check to make sure that the tile is not apart of a settlement already
                if(!data.settlementTiles.Contains(tile) && tileNotOwned(tile, hexController) == true)
                {
                    data.settlementTiles.Add(tile);
                    hexController.changeControlValue(true);
                    hexController.ChangeOwnership(playerScript);
                }
            }
        }

        return data;
    }

    private bool tileNotOwned(GameObject tile, HexController hexController)
    {
        if(hexController.isControlled == true)
        {
            return false;

        } else 
        {
            return true;
        }
    }
    
    public void UpgradeSettlement(Building data, BuildData buildData)
    {
        //Upgrade if requirements are met (population, building count, income, etc)
        //Increase influence, and add the tiles to the settlement

        if((data.settlementPopulation > peoplePerSettlementUpgrade * data.settlementLevel) || (data.settlementBuildings.Count > buildingsPerSettlementUpgrade * data.settlementLevel) && data.settlementLevel <= maxSettlementLevel)
        {
            GameObject settlementObj = null;

            data.settlementLevel++;

            for(int i = 0; i < settlementData.Count; i++)
            {
                if(settlementData[i] == data)
                {
                    settlementObj = settlementObjs[i];
                    break;
                }
            }

            if (settlementObj != null)
            {
                SphereCollider sphereCollider = settlementObj.GetComponent<SphereCollider>();

                // Increase the sphere collider's radius based on the settlement level
                sphereCollider.radius *= data.settlementLevel;

                GetSettlementTiles(settlementObj, data);
            }
        }

        //Update values on the server side of things
        buildData.SetBuildData(data);
    }

    public bool isCapital(Building data)
    {
        if(settlementData.Count == 0)
        {
            return true;
        }

        return false;
    }

    //Functions to calculate and manage building needs
    public void CreateBuilding(Building data, BuildData buildData) //Does costs, production, assigning, etc
    {
        if(data.requiresASettlement == true)
        {
            Building settlement = data.buildingsParentSettlement;

            settlement.settlementPopulation -= data.peopleInitialCost;

            //Update the settlement data
            for(int i = 0; i < buildingServerData.Count; i++)
            {
                if(settlement == settlementData[i])
                {
                    //Update the data on the server side of things
                    buildingServerData[i].SetBuildData(settlement);
                    break;
                }
            }
        }

        //Update values on the server side of things
        buildData.SetBuildData(data);
    }

    public void BuildingUpKeep(Building data, BuildData buildData)
    {
        if(data.buildingsParentSettlement != null)
        {
            Building settlement = data.buildingsParentSettlement;

            playerScript.gameManager.playerGold -= data.goldUpKeep;
            settlement.settlementPopulation -= data.peopleUpKeep;
            settlement.settlementFood -= data.foodUpkeep;

            //Update the settlement data
            for(int i = 0; i < buildingServerData.Count; i++)
            {
                if(settlement == settlementData[i])
                {
                    //Update the data on the server side of things
                    buildingServerData[i].SetBuildData(settlement);
                    break;
                }
            }

        } else 
        {
            playerScript.gameManager.playerGold -= data.goldUpKeep;
        } 
    }

    public void BuildingProduction(Building data, BuildData buildData)
    {
        if(data.buildingsParentSettlement != null)
        {
            Building settlement = data.buildingsParentSettlement;

            playerScript.gameManager.playerGold += data.goldProduction;
            settlement.settlementFood += data.foodProduction;
            playerScript.gameManager.playerTechPoints += data.techPointProduction;

            //Object production
            foreach (Object item in data.producableObjects)
            {
                settlement.settlementObjectInventory.Add(item);
            }

            //Update the settlement data
            for(int i = 0; i < buildingServerData.Count; i++)
            {
                if(settlement == settlementData[i])
                {
                    //Update the data on the server side of things
                    buildingServerData[i].SetBuildData(settlement);
                    break;
                }
            }

        } else 
        {
            playerScript.gameManager.playerGold += data.goldProduction;
            playerScript.gameManager.playerTechPoints += data.techPointProduction;
        }
    }

    public void BuildingTraining(Building data, BuildData buildData) //Might need this, will work on unit creation later
    {
        //Create troops each interval
    }
}
