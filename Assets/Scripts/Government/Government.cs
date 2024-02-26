using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Government", menuName = "Table Top Data/Government")]
public class Government : ScriptableObject
{
    //Info Data Sets
    public string governmentName;
    public string governmentDescription;

    //Percentage Buffs and Modifiers
    public int settlement = 1;
    public int agrarian = 1;
    public int military = 1;
    public int industrial = 1;
    public int academicAndReligious = 1;
    public int naval = 1;
    public int transportationAndEconomy = 1;
    public int defense = 1;
    public int scouting = 1;
    public int mediaAndSocial = 1;

    //War Support Based Modifiers
    public float democraticWarSupport = 1f;
    public float republicWarSupport = 1f;
    public float theocracyWarSupport = 1f;
    public float fascistWarSupport = 1f;
    public float communistWarSupport = 1f;
    public float monarchyWarSupport = 1f;
    public float oligarchyWarSupport = 1f;

    //Religious and Social Based Modifiers
    public float domesticReligiousSpeedModifier = 1f; //How fast Other Players Relgions Spread Through Your Empire
    public float empireLoyaltyModifier = 1f; //How Good You Are At Keeping Your People Loyal
    public float wholeBoardReligiousSpeedModifier = 1f; //How fast Your Religion Spreads Across the board
}
