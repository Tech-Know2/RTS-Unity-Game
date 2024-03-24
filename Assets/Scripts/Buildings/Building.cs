using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Table Top Data/Building")]
public class Building : ScriptableObject
{
    //Display Attributes
    public string buildingName;
    public string buildingDescription;

    //Game Modifier Attribute Variables for Consumption
    public int peopleInitialCost;
    public int goldUpKeep;
    public int peopleUpKeep;
    public int foodUpkeep;
    public float health;

    //Game Modifier Attribute Variables for Production
    public int goldProduction;
    public int foodProduction;
    public float techPointProduction;
    public Building buildingsParentSettlement;

    //Variables to Keep Track of Building Constraints and Requierments
    public bool requiresASettlement;
    public bool isASettlement;
    public string settlementName;
    public List<string> acceptableBuildTiles = new List<string>();

    //Settlement Info
    public bool isCapital = false;
    public List<Object> settlementObjectInventory = new List<Object>();
    public List<Object> settlementReligiousFollowers = new List<Object>();
    public List<Unit> settlementUnits = new List<Unit>();
    public List<Religion> settlementReligions = new List<Religion>();
    public List<Building> settlementBuildings = new List<Building>();
    public List<GameObject> settlementTiles = new List<GameObject>();
    public Religion mainReligion;
    public int settlementFood = 50;
    public int settlementLevel = 1;
    public int settlementPopulation = 25;
    public int settlementGoldProduction = 0;
    public int settlementTechPointProduction = 0;

    //Producible Objects from Building
    public List<Object> producableObjects = new List<Object>();

    //Bools to check Building Identity
    public bool isMovementRelated;
    public bool isDefenseRelated;
    public bool isReligiousTrainingRelated;
    public bool isPath;
    public bool isDefense;
    public bool isGate;
    public bool isBridge;
    public bool isMountable;
    public bool isSightCapable;

    public int sightRange;
}