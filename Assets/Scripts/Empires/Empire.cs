using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Empire", menuName = "Table Top Data/Empire")]
public class Empire : ScriptableObject
{
    //Main Stats
    public string empireName;
    public Color empireColor;
    public Government empireGovernment;
    public Religion religionPlayerStarted;
    public bool isPlayerControlled;
    public bool isVassal = false;
    public Empire leigeEmpire;

    //Empire Wealth
    public int empireGold;
    public int empireTechPoints;
    
    //Empire Lands
    public List<City> empireSettlements = new List<City>();
    public List<Building> empireBuildings = new List<Building>();
    public List<Unit> empireUnits = new List<Unit>();
    public List<GameObject> empireTiles = new List<GameObject>();
    public List<Object> empireObjects = new List<Object>();

    //Empire Politics
    public List<Empire> alliedEmpires = new List<Empire>();
    public List<Empire> warringEmpires = new List<Empire>();
    public List<Empire> neutralEmpires = new List<Empire>();

    //Empire Religion
    public List<Religion> empireReligions = new List<Religion>();

    //Empire Society
    public float totalEmpireLoyalty = 100;
    public float totalEmpireWarSupprt = 100;


}