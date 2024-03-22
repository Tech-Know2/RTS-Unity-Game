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

    //Player Empire Vars
    private Empire playerEmpire;

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

        //Do building effects
        CreateBuilding(data);

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

        //Reasign the data to the objects
        BuildData objBuildData = obj.GetComponent<BuildData>();
        objBuildData.buildData = data;

        data.settlementName = playerScript.empireManager.nameCreator.GenerateCityName();

        playerScript.notificationController.CreateNotification("Settlement Founded", data.settlementName + " has been added to your empire");
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
            BuildingUpKeep(building);
            BuildingProduction(building);
            BuildingProduction(building);
            BuildingTraining(building);
        }
    }

    private void SettlementEffects()
    {
        foreach (Building settlement in settlementData)
        {
            UpdatePopandFood(settlement);
            CalcReligion(settlement);
            UpgradeSettlement(settlement);
            SettlementIncome(settlement);
        }
    }

    public void SettlementIncome(Building data)
    {
        playerScript.gameManager.playerGold += data.settlementPopulation;
    }

    //Functions to calculate and manage settlement needs
    public void UpdatePopandFood(Building data)
    {
        //data.settlementFood -= data.settlementPopulation;
        //data.settlementPopulation += (int) data.settlementFood / 2; //2 food per person
    }

    public void CalcReligion(Building data)
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
        }
    }

    public Building GetSettlementTiles(GameObject obj, Building data)
    {
        float radius = settlementRadius * data.settlementLevel;

        Collider[] colliders = Physics.OverlapSphere(obj.transform.position, radius);

        // Iterate through all colliders and filter objects with desired tags
        foreach (Collider collider in colliders)
        {
            // Check if the collider's tag is one of the desired tags
            if (tileTags.Contains(collider.tag))
            {
                GameObject tile = collider.gameObject;

                data.settlementTiles.Add(tile);
            }
        }

        return data;
    }
    
    public void UpgradeSettlement(Building data)
    {
        //Upgrade if requirements are met (population, building count, income, etc)
        //Increase influence, and add the tiles to the settlement
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
    public void CreateBuilding(Building data) //Does costs, production, assigning, etc
    {
        if(data.requiresASettlement == true)
        {
            Building settlement = data.buildingsParentSettlement;

            settlement.settlementPopulation -= data.peopleInitialCost;
        }
    }

    public void BuildingUpKeep(Building data)
    {
        if(data.buildingsParentSettlement != null)
        {
            Building settlement = data.buildingsParentSettlement;

            playerScript.gameManager.playerGold -= data.goldUpKeep;
            settlement.settlementPopulation -= data.peopleUpKeep;
            settlement.settlementFood -= data.foodUpkeep;

        } else 
        {
            playerScript.gameManager.playerGold -= data.goldUpKeep;
        } 
    }

    public void BuildingProduction(Building data)
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

        } else 
        {
            playerScript.gameManager.playerGold += data.goldProduction;
            playerScript.gameManager.playerTechPoints += data.techPointProduction;
        }
    }

    public void BuildingTraining(Building data) //Might need this, will work on unit creation later
    {
        //Create troops each interval
    }
}
