using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class BuildData : NetworkBehaviour
{
    public Building buildData;

    [ServerRpc]
    public void SetBuildData(Building data)
    {
        buildData = data;
    }

    [ServerRpc]
    public void UpdateBuildData(Building data)
    {
        buildData = data;
    }

    /* Script for the building effect manager to update values on the server side of things
        for(int i = 0; i < settlementServerData.Count; i++)
            {
                if(data == settlementData[i])
                {
                    //Update the data on the server side of things
                    settlementServerData[i].SetBuildData(data);
                    break;
                }
            }
    */
}
