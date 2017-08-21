using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GSceneMap : UnitySingleton<GSceneMap>
{


    public void CreateMap()
    {
        GameObject terrain = new GameObject();
        TerrainData _terraindata = new TerrainData();
        _terraindata.heightmapResolution = 100;
        _terraindata.alphamapResolution = 100;
        _terraindata.size = new Vector3(100, 1, 100);
        terrain = Terrain.CreateTerrainGameObject(_terraindata);
        terrain.transform.position = new Vector3(0,0,0);
        Terrain t = terrain.GetComponent<Terrain>();
        t.Flush();
    }
}

