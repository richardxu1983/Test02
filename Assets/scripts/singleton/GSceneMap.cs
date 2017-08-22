using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GSceneMap : UnitySingleton<GSceneMap>
{


    public void CreateMap()
    {
        GameObject terrain = new GameObject();
        TerrainData _terraindata = new TerrainData();
        //_terraindata.heightmapResolution = 100;
        //_terraindata.alphamapResolution = 100;
        //_terraindata.baseMapResolution = 64;
        _terraindata.SetDetailResolution(32, 32);
        _terraindata.size = new Vector3(32, 1, 32);

        SplatPrototype[] terrainTexture = new SplatPrototype[2];
        terrainTexture[0] = new SplatPrototype();
        terrainTexture[0].texture = (Texture2D)Resources.Load("GroudTextures/Mud01");
        terrainTexture[0].tileSize = new Vector2(2, 2);
        terrainTexture[1] = new SplatPrototype();
        terrainTexture[1].texture = (Texture2D)Resources.Load("GroudTextures/grass01");
        terrainTexture[1].tileSize = new Vector2(2, 2);
        _terraindata.splatPrototypes = terrainTexture;

        terrain = Terrain.CreateTerrainGameObject(_terraindata);
        terrain.transform.position = new Vector3(0,0,0);
        Terrain t = terrain.GetComponent<Terrain>();
        t.materialType = Terrain.MaterialType.BuiltInLegacyDiffuse;
        t.Flush();
    }
}

