using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpireManager : MonoBehaviour
{
    public Player playerScript;
    public GameManager gameManager;
    public NameCreator nameCreator;
    public List<Color> rawEmpireColorOptions = new List<Color>();
    private int playerColorSpot = 0;

    public Empire CreateEmpire()
    {
        Empire empire = Empire.CreateInstance<Empire>();

        return empire;
    }

    public void ConnectToPlayer(Player player)
    {
        playerScript = player;
        gameManager = playerScript.gameManager;
    }

    public void ConnectToNameGen()
    {
        GameObject mapManagerObject = GameObject.FindWithTag("Map Manager");

        nameCreator = mapManagerObject.GetComponent<NameCreator>();
    }

    public Empire SetUpEmpire(Empire empire, Player player)
    {
        playerColorSpot = gameManager.nextPlayerID + gameManager.randomDisplacement;

        while (playerColorSpot > rawEmpireColorOptions.Count)
        {
            playerColorSpot -= rawEmpireColorOptions.Count;
        }

        empire.empireColor = rawEmpireColorOptions[playerColorSpot];
        ConnectToNameGen();

        empire.empireName = nameCreator.GenerateEmpireName();
        empire.playerController = player;

        return empire;
    }

    public void IntervalChanges(Empire empire)
    {
        UpdateEmpireGovernment(empire);
        UpdateEmpireWealth(empire);
        CalculateEmpireMainReligion(empire);
        AddAReligionToEmpire(empire);
        AddObjectToEmpire(empire);
        CalculateTotalEmpireLoyalty(empire);
        AddCityToEmpire(empire);
        AddLandToEmpire(empire);
        AddUnitToEmpire(empire);
        UpdateAlliances(empire);
        UpdateWarStatus(empire);
    }

    public Empire UpdateEmpireGovernment(Empire empire)
    {
        empire.empireGovernment = playerScript.playerGovernment;

        return empire;
    }

    public Empire UpdateEmpireWealth(Empire empire)
    {
        empire.empireGold = gameManager.playerGold;
        empire.empireTechPoints = gameManager.playerTechPoints;

        return empire;
    }

    public Empire CalculateEmpireMainReligion(Empire empire)
    {
        return empire;
    }

    public Empire AddAReligionToEmpire(Empire empire)
    {
        return empire;
    }

    public Empire AddObjectToEmpire(Empire empire)
    {
        return empire;
    }

    public Empire CalculateTotalEmpireLoyalty(Empire empire)
    {
        return empire;
    }

    public Empire AddCityToEmpire(Empire empire)
    {
        return empire;
    }

    public Empire AddLandToEmpire(Empire empire)
    {
        return empire;
    }

    public Empire AddUnitToEmpire(Empire empire)
    {
        return empire;
    }

    public Empire UpdateAlliances(Empire empire)
    {
        return empire;
    }

    public Empire UpdateWarStatus(Empire empire)
    {
        return empire;
    }
}
