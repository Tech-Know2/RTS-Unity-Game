using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class BuildData : NetworkBehaviour
{
    public Building buildData;

    [ServerRpc(RequireOwnership = false)]
    public void UpdateBuildData(Building data)
    {
        Debug.Log("Update Build Data Called");

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
