using System.Linq;
using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet;
using Steamworks;
using HeathenEngineering.SteamworksIntegration;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    // Static variable to keep track of the next available player ID
    [SyncVar]
    public int nextPlayerID = 0;
    [SyncVar]
    public int randomDisplacement = 0;

    // Networking Vars and Data Collection
    private CSteamID[] members;

    //Client Specific Vars and Data Collection
    public int playerElement;
    public int eraLength = 30;
    public int gameEra = 1;
    public int yearsPerInterval = 25;

    //Economy Vars
    public int playerGold = 0;
    public float playerTechPoints = 1;

    //Player Vars
    public Player playerScript;
    
    //

    private void Awake()
    {
        AssignPlayerID();
    }

    private void AssignPlayerID()
    {
        this.playerElement = nextPlayerID;

        nextPlayerID++;

        Debug.Log($"Player {playerElement + 1} joined with ID {playerElement}");
    }

    public void ConnectToPlayer(Player playerScript)
    {
        this.playerScript = playerScript;

        if(this.playerScript == null)
        {
            Debug.Log("Player Script did not connect");
        } else 
        {
            Debug.Log("Player Script connected");
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsOwner)
        {
            gameObject.GetComponent<GameManager>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<GameManager>().enabled = false;
        }

        if(base.IsServer)
        {
            randomDisplacement = Random.Range(0,20);
        }
    }

    public void UpdateEra()
    {
        if(Player.interval < eraLength)
        {
            gameEra = 1;
        } else if (Player.interval < eraLength * 2)
        {
            gameEra = 2;
        } else 
        {
            gameEra = 3;
        }
    }

    public int CalcYear()
    {
        return Player.interval * yearsPerInterval;
    }

    public int GetEra()
    {
        return gameEra;
    }

    public int GetOpenCardSlotCount()
    {
        return playerScript.UIController.GetOpenSlot();
    }
}
