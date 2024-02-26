using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Table Top Data/Building")]
public class Building : ScriptableObject
{
    //Display Attributes
    public string buildingName;
    public string buildingDescription;
    public Sprite buildingThumbnail;

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

    //Variables to Keep Track of Building Constraints and Requierments
    public bool requiresASettlement;
    public List<string> acceptableBuildTiles = new List<string>();

    //Producible Objects from Building
    public List<Object> producableObjects = new List<Object>();

    //Bools to check Building Identity
    public bool isReligiousTrainingRelated;
    public bool isPath;
    public bool isDefense;
    public bool isGate;
    public bool isBridge;
    public bool isMountable;
    public bool isSightCapable;

    public int sightRange;
}