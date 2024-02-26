using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Table Top Data/Unit")]
public class Unit : ScriptableObject
{
    //Display Information
    public string unitName;
    public string description;
    public string unitCategory;
    public string unitColor;
    public int unitEra;
    public GameObject unitGameObject;

    //Creation Cost Information
    public int initialFoodCost;
    public int initialSilverCost;
    public int initialGoldCost;

    //Upkeep Information
    public int silverCostPerTurn;
    public int goldCostPerTurn;
    public int foodCostPerTurn;
    public int populationCost;

    //Where They Are Produced (Locations)
    public bool requiresTrainingHall;
    public bool requiresBarracks;
    public bool requiresPort;
    public bool requiresChurch;
    public bool requiresDiplomatTrainingFacility;
    public bool requiresNavalAcademy;
    public bool requiresSiegeWorkshop;

    //Unit Stats
    public int stamina;
    public int attack;
    public int defense;
    public bool isNaval;
    public bool isRanged;
    public int range;
    public bool isSiege;
    public bool isPedestalMountable;
    public bool isADiplomat;
    public bool isReligious;
    public int religiousStrength;

    //Unit Backend Information and Requierments
    public bool requiresArmor;
    public bool requiresSword;
    public bool requiresBow;

    //Variables to Manage Unit Upgrades
    public bool isUpgradable;
    public Unit[] upgradeForms;
    public Unit baseUpgradeUnit;
}
