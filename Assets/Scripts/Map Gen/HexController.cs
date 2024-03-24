using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class HexController : NetworkBehaviour
{
    public bool isOccupied = false;

    public void Start()
    {
        isOccupied = false;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeOccupancy(bool value)
    {
        isOccupied = value;
    }
}
