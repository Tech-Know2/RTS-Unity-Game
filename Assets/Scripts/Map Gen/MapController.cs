using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Connection; 

[System.Serializable]
public class TerrainObjectData
{
    public string terrainObjectName;
    public GameObject terrainObject;
    public int lowerSpawnRate;
    public int upperSpawnRate;
    public List<string> acceptableTileSpawns;
}

[System.Serializable]
public class Tiles
{
    public string tileName;
    public GameObject tile;
    public float lowerSpawnRange;
    public float upperSpawnRange;
}

public class MapController : NetworkBehaviour
{
    [SerializeField]
    public int mapWidth = 50, mapHeight = 50;

    // Map Generation Data
    public List<TerrainObjectData> terrainObjectData;
    public List<Tiles> tileList;

    // Handles the storage of map generation/tile map data
    private List<TileData> terrainData = new List<TileData>();

    public int seedToken = 0;

    [System.Serializable]
    public struct TileData
    {
        public Vector3 position;
        public int tileElement;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsServer)
        {
            Debug.Log("Is the server owner");
            gameObject.GetComponent<MapController>().enabled = true;

            //Generate Seed
            int seed = Random.Range(0,999999);

            seedToken = seed;

            CreateMap(seed);
        } else 
        {
            gameObject.GetComponent<MapController>().enabled = false;
        }
    }

    [ObserversRpc]
    private void CreateMap(int seed)
    {
        Debug.Log("Creating the Map");

        Random.InitState(seed);

        float xPos = Random.Range(-10000f, 10000f);
        float zPos = Random.Range(-10000f, 10000f);

        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapWidth; z++)
            {
                float sampleX = (x + xPos);
                float sampleZ = (z + zPos);

                float yNoise = Mathf.PerlinNoise(sampleX * 0.07f, sampleZ * 0.07f);

                for (int i = 0; i < tileList.Count; i++)
                {
                    if (yNoise >= tileList[i].lowerSpawnRange && yNoise < tileList[i].upperSpawnRange)
                    {
                        Vector3 hexPosition = new Vector3(x, 0, z);
                        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                        GameObject hex = Instantiate(tileList[i].tile, hexPosition, rotation);
                        hex.tag = tileList[i].tileName;

                        // Set the hex as a child of the MapController
                        hex.transform.SetParent(this.transform);

                        SpawnTile(hex);

                        terrainData.Add(new TileData
                        {
                            position = hexPosition,
                            tileElement = i
                        });
                    }
                }
            }
        }

        //RpcReceiveTerrainData(terrainData.ToArray());
    }

    public void SpawnTile(GameObject hex)
    {
        base.Spawn(hex, base.Owner); //Add ", base.Owner" if the objects need to have an owner
    }

    /*

    [ObserversRpc(ExcludeOwner = true)]
    private void RpcReceiveTerrainData(TileData[] receivedData)
    {
        ApplyTerrainData(receivedData);
    }

    private void ApplyTerrainData(TileData[] receivedData)
    {
        Debug.Log("Client Side Map Generation");

        for (int i = 0; i < receivedData.Length; i++)
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
            GameObject hex = Instantiate(tileList[receivedData[i].tileElement].tile, receivedData[i].position, rotation);
            hex.transform.SetParent(this.transform);
        }
    }

    */
}
