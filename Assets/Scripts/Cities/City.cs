using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New City", menuName = "Table Top Data/City")]
public class City : ScriptableObject
{
    public string cityName;
    public int cityLevel;
    public int cityPopulation;
    public float calculatedLoyalty = 100f;
    public float calculatedWeariness = 100f;
    public int cityFood;

    public List<Object> storedObjects = new List<Object>();
    public List<Object> objectsProducedEachTurn = new List<Object>();

    //Create a list for the loyalty of the citizens
    public List<Object> settlementReligiousFollowers = new List<Object>();
    public List<Building> settlementBuildings = new List<Building>();
    public List<GameObject> tilesUnderCityControl = new List<GameObject>();
    public List<Unit> settlementUnits = new List<Unit>();
}
