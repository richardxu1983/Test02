﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GSceneMap : UnitySingleton<GSceneMap>
{
    int gridSizeX, gridSizeY;
    float mapWidth, mapLength;
    float gridSize = 3.0f;

    GameObject terrain;
    TerrainData _terraindata;
    Terrain t;

    public Vector2 gridWorldSize;
    Node[,] grid;
    Vector3 worldBottomLeft;


    public void CreateMap()
    {
        CreateData();
        CreateTerrain();
    }

    void CreateTerrain()
    {
        terrain = new GameObject();
        _terraindata = new TerrainData();
        //_terraindata.heightmapResolution = 100;
        //_terraindata.alphamapResolution = 100;
        //_terraindata.baseMapResolution = 64;
        //_terraindata.SetDetailResolution(32, 32);
        _terraindata.size = new Vector3(gridWorldSize.x, 1, gridWorldSize.y);

        SplatPrototype[] terrainTexture = new SplatPrototype[2];
        terrainTexture[0] = new SplatPrototype();
        terrainTexture[0].texture = (Texture2D)Resources.Load("GroudTextures/Mud01");
        terrainTexture[0].tileSize = new Vector2(gridSize, gridSize);
        terrainTexture[1] = new SplatPrototype();
        terrainTexture[1].texture = (Texture2D)Resources.Load("GroudTextures/grass01");
        terrainTexture[1].tileSize = new Vector2(gridSize, gridSize);
        _terraindata.splatPrototypes = terrainTexture;

        terrain = Terrain.CreateTerrainGameObject(_terraindata);
        terrain.transform.position = new Vector3(0 - mapWidth / 2, 0, 0 - mapLength / 2);
        t = terrain.GetComponent<Terrain>();
        t.materialType = Terrain.MaterialType.BuiltInLegacyDiffuse;
        t.Flush();
    }

    void CreateData()
    {
        gridSizeX = 200;
        gridSizeY = 200;
        mapWidth = gridSizeX * gridSize;
        mapLength = gridSizeY * gridSize;
        gridWorldSize = new Vector2(mapWidth, mapLength);
        CreateGrid();
    }

    public Node nodeFromWorldPoint(Vector3 worldPosition)
    {
        int gridX = Mathf.RoundToInt((worldPosition.x - worldBottomLeft.x - gridSize / 2) / gridSize);
        int gridY = Mathf.RoundToInt((worldPosition.z - worldBottomLeft.z - gridSize / 2) / gridSize);

        return grid[gridX, gridY];
    }

    public Vector3 gridToWorldPosition(int x, int y)
    {
        Vector3 pos = new Vector3(worldBottomLeft.x + x * gridSize + gridSize / 2, 1, worldBottomLeft.z + y * gridSize + gridSize / 2);
        return pos;
    }

    public Vector3 gridToWorldPosition(GridID id)
    {
        Vector3 pos = new Vector3(worldBottomLeft.x + id.x * gridSize + gridSize / 2, 1, worldBottomLeft.z + id.y * gridSize + gridSize / 2);
        return pos;
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        worldBottomLeft = new Vector3(0,0,0) - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        //Debug.Log("worldBottomLeft : "+ worldBottomLeft);
        Vector3 worldPoint;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                worldPoint = worldBottomLeft + Vector3.right * (x * gridSize + gridSize/2) + Vector3.forward * (y * gridSize + gridSize / 2);
                bool block = Globals.rd.Next(0, 10) > 7 ? true : false;
                grid[x, y] = new Node(block, worldPoint, new GridID(x,y));
            }
        }
    }
}
