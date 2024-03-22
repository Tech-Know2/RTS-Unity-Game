using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

[System.Serializable]
public class EffectManagerList
{
    public bool requiresMultipleTurns; //Enter the amount of turns the card effect lasts
    public bool goesForever;
    public int turnEffectLength; //Length of the effect
    public int effectCost; //List the amount of change (-10 loyalty, -10 food, etc)
    public string costType; //List types from above
    public string religionName; //Name if the cost is related to religion
    public Government governmentType; //Name of the government your effecting war support for
    public int turnsActive = 0; //Dont Mess With This Var. It stores how many turns that this card has been active.
    public bool requiresTiles; //Select true if the output of the cards effect is determined by tiles within the city
    public bool requiresASettlement;
    public bool globalEffect;
    public List<string> effectTiles = new List<string>(); //List the tiles here that will determine scale of the effect
    public Building settlementPlayedOn;
    public bool usesChance; //Mostly used for event cards to determine what should happen and the chance of it happening
}

[System.Serializable]
public class RelgiousEffectsList
{
    public Religion religion; //Scriptable Object containing the information about the religon
    public bool isPlayersReligion;
    public bool isDedication;
    public bool enablesSuzerain;
    public string suzerainEffectType;
    public bool suzerainEffect;
    public string religiousEffect;
    public bool usesChance;
    public bool usesAModifier;
    public bool needsModifierSetup;
}

[CreateAssetMenu(fileName = "New Card", menuName = "Table Top Data/Card")]
public class Card : ScriptableObject
{
    //Card Display Variables
    public string cardName;
    public string description;
    public string cardCategory;
    public Color cardColor;
    public string cardTech;
    public int cardEra;
    public bool actionCard;
    public bool isReligiousCard;

    //Multiplayer Vars
    public Player originalPlayer;

    //Government Vars
    public bool governmentRelated = false;
    public bool assignsGovernment;
    public Government governmentType;
    public bool isGovernmentWarSupportCard;
    public bool isAllianceCard;
    public string governmentAllianceType;
    public bool isDeclareWarCard;
    public string governmentTypeForWar;
    public bool isPeaceTreatyCard;

    //Standard Vars
    public bool isPurchasable;
    public int goldCost;
    public bool buildsSomething = false;

    //Religious Vars
    public bool createAReligion;

    //Effects
    [TextArea(12, 40)]
    public string effectDescriptionEditorDisplay =
        "Strings to determine the effects on the game: Don't Alter Won't Change\n" +
        "If the effect only lasts for 1 turn, don't select the Requires Multiple Turns Bool.\n" +
        "If it only lasts for one turn, don't put in anything\n" +
        "\"Gold Cost\" to take gold per turn\n" +
        "\"Loyalty Cost\" to loss/take loyalty per turn\n" +
        "\"Silver Cost\" to take silver per turn\n" +
        "\"Food Cost\" to take food per turn\n" +
        "\"People Cost\" to take people per turn\n" +
        "\"War Propaganda Cost\" to increase War Support per turn against a certain government\n" +
        "\"Religion Cost\" to take away religion from area\n" +
        "\"Weariness Cost\" to effect war weariness\n" +
        "\"Spread Distrust Cost\" to effect the loyalty of your people \n" +
        "\"Tech Cost\" to effect your empire's tech points \n";

    public List<EffectManagerList> effectManagerList = new List<EffectManagerList>();

    [TextArea(12, 40)]
    public string religousEffectsEditorDisplay =
        "Strings to determine the effects on player religion\n" +
        "If the effect only lasts for 1 turn, don't select the Requires Multiple Turns Bool.\n" +
        "If it only lasts for one turn, don't put in anything\n" +
        "\"Mono or Poly\" to determine if mono or poly thestic\n" +
        "\"Education Freedoms\" to allow or not allow educated freedoms\n" +
        "\"Religious Freedoms\" religious tolerance\n" +
        "\"Stable Dedication\" stability dedication\n" +
        "\"Economic Dedication\" economic dedication\n" +
        "\"Harvest Dedication\" food dedication\n" +
        "\"Religious Dedication\" religious dedication\n" +
        "\"Academic Dedication\" tech point dedication\n" +
        "\"Holy War\" will start a holy war \n" +
        "\"Allow Religious Suzerain\" will grant control/influence/power over other empires religion \n" +
        "\"Allow Economic Suzerain\" will grant control/influence/power over other empires economy \n" +
        "\"Allow Academic Suzerain\" will grant control/influence/power over other empires education \n" +
        "\"Allow Politcal Suzerain\" will grant control/influence/power over other empires politics \n" +
        "\"Economic Yield\" will grant you income over follower churches \n" +
        "\"Religious Expansion\" will add followers to all non dominant cities globally \n" +
        "\"Religious Safety\" will add followers to all dominant cities globally \n";

    [TextArea(12, 40)]
    public string suzerainEffectTypes =
        "Strings to determine the suzerzin effect\n" +
        "If the effect only lasts for 1 turn, don't select the Requires Multiple Turns Bool.\n" +
        "If it only lasts for one turn, don't put in anything\n" +
        "\"Academic\" education based suzerain card\n" +
        "\"Political\" political based suzerain card\n" +
        "\"Economic\" economic based suzerain card\n";

    public List<RelgiousEffectsList> religoiusEffectLists = new List<RelgiousEffectsList>();
    
    //Building Based Vars
    public List<GameObject> buildingGameObjects = new List <GameObject>();

    [TextArea(12, 40)]
    public string desierdTileString = 
        "Strings to determine the effects on the game: Don't Alter Won't Change\n" +
        "\"Indicates which tile the card can effect\" \n" +
        "\"Deep Water\" \n" +
        "\"Shallow Water\" \n" +
        "\"Sand\" \n" +
        "\"Grass\" \n" +
        "\"Mountain\" \n";
    public List<string> desiredTilesList = new List<string>();

    //Unit Based Vars
    public List<Unit> units = new List<Unit>();
}