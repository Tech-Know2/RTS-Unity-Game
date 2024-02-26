using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Religion", menuName = "Table Top Data/Religion")]
public class Religion : ScriptableObject
{
    //Standard ID Data
    public string religionName;
    public Color religionColor;
    public string religionCreator;

    //Religious Modifiers
    //Education
    public bool educatedFreedoms;
    public float educationModifier;

    //Religion Social Status
    public bool religionFreedoms;

    //Culture Bonus
    public bool monotheism;
    public bool polytheism;
    public bool religiousFreedom;

    //Diplomacy
    public float diplomacyFavorRate; //How much other empires like you, effects war suport status

    //Moral and Loyalty
    public bool canStableDedicate; //Get stability in that city
    public bool canEconomicDedicate; //Get gold in that city
    public bool canHarvestDedicate; //Get food in that city
    public bool canReligiousDedicate; //Get more of your religious citizens in that city
    public bool canAcademicDedicate; //Get more tech points for your empire
    //unit buffs
    //etc

    //Conquest and Conversions
    public bool friendlyReligion;
    public bool dedicatedReligion;
    public float conversionSpeed;
    public float alternateReligonConversionSpeed;

    //Holy Wars
    public bool canHolyWars;
    public float holyWarLoyaltyModifier;
    public float holyWarEducationCost;

    //Suzerain functions
    public bool canReligiousSuzerain; //Makes empire under control only have tis religion, and no others. Gets all suzerian buffs
    public bool canEconomicSuzerain; //Makes some gold off of religious buildings in citys with this religion as dominant
    public bool canAcademicSuzerain; //Makes some tech points off of religious buildings in citys with this religion as dominant
    public bool canPoliticallySuzerain; //Makes rallying war support against this religion difficult if declaring war against dominant religion creator
    public int suzerainEffectModifier;

    //Religious/Econmic Income
    public float churchIncomeModifier;
}
