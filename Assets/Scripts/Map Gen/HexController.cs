using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class HexController : NetworkBehaviour
{
    [SyncVar] public bool isOccupied = false;
    [SyncVar] public bool isControlled = false;
    [SyncVar] public Player controlledBy;
    [SyncVar] public bool hasRoad = false;

    public void Start()
    {
        isOccupied = false;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeOccupancy(bool value)
    {
        isOccupied = value;
    }

    [ServerRpc(RequireOwnership = false)]
    public void changeControlValue(bool value)
    {
        isControlled = value;
    }

    [ServerRpc(RequireOwnership = false)]
    public void updateRoadValue(bool value)
    {
        hasRoad = value;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnership(Player player)
    {
        controlledBy = player;
    }
}
