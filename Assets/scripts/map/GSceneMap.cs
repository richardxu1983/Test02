using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GSceneMap : UnitySingleton<GSceneMap>
{
    public int gridNum = 200;
    int gridSizeX, gridSizeY;
    float mapWidth, mapLength;
    float gridSize = 2.2f;
    public Vector2 gridWorldSize;
    public Node[,] grid;
    Vector3 worldBottomLeft;
    private float _seedX = 0.0f, _seedZ = 0.0f;
    private float _relief = 15f;
    float xSample, zSample;

    [NonSerialized]
    GameObject terrain;
    [NonSerialized]
    TerrainData _terraindata;
    [NonSerialized]
    SplatPrototype[] terrainTexture;
    [NonSerialized]
    Terrain t;

    public void CreateMap()
    {
        CreateData();
        CreateTerrain();
    }

    public void LoadMap()
    {
        gridSizeX = gridNum;
        gridSizeY = gridNum;
        mapWidth = gridSizeX * gridSize;
        mapLength = gridSizeY * gridSize;
        gridWorldSize = new Vector2(mapWidth, mapLength);
        grid = new Node[gridSizeX, gridSizeY];
        worldBottomLeft = new Vector3(0, 0, 0) - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
    }

    public void CreateTerrain()
    {
        int sur;
  
        terrain = new GameObject();
        _terraindata = new TerrainData();
        //_terraindata.heightmapResolution = 100;
        _terraindata.alphamapResolution = 200;
        //_terraindata.baseMapResolution = 64;
        //_terraindata.SetDetailResolution(32, 32);
        _terraindata.size = new Vector3(gridWorldSize.x, 1, gridWorldSize.y);
        terrainTexture = new SplatPrototype[XMLLoader.Instance.MaxGSurConfig];
        for(int i=0;i< XMLLoader.Instance.MaxGSurConfig;i++)
        {
            terrainTexture[i] = new SplatPrototype();
            terrainTexture[i].texture = (Texture2D)Resources.Load("GroudTextures/"+ XMLLoader.Instance.GsurIndex[i].name);
            terrainTexture[i].tileSize = new Vector2(gridSize, gridSize);
        }
        _terraindata.splatPrototypes = terrainTexture;

        float[,,] map = new float[_terraindata.alphamapWidth, _terraindata.alphamapHeight, XMLLoader.Instance.MaxGSurConfig];
        for (var y = 0; y < _terraindata.alphamapHeight; y++)
        {
            for (var x = 0; x < _terraindata.alphamapWidth; x++)
            {
                sur = grid[x, y].terrainIndex;
                map[x, y, sur] = 1;
                for(int j=0;j< XMLLoader.Instance.MaxGSurConfig;j++)
                {
                    if(j!=sur)
                    {
                        map[x, y, j] = 0;
                    }
                }
            }
        }
        _terraindata.SetAlphamaps(0, 0, map);

        terrain = Terrain.CreateTerrainGameObject(_terraindata);
        terrain.transform.position = new Vector3(0 - mapWidth / 2, 0, 0 - mapLength / 2);
        t = terrain.GetComponent<Terrain>();
        t.materialType = Terrain.MaterialType.BuiltInLegacyDiffuse;
        t.Flush();
    }

    void CreateData()
    {
        Debug.Log("CreateData");
        gridSizeX = gridNum;
        gridSizeY = gridNum;
        mapWidth = gridSizeX * gridSize;
        mapLength = gridSizeY * gridSize;
        gridWorldSize = new Vector2(mapWidth, mapLength);
        grid = new Node[gridSizeX, gridSizeY];
        worldBottomLeft = new Vector3(0, 0, 0) - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        CreateGrid();
    }

    public Node nodeFromWorldPoint(Vector3 worldPosition)
    {
        int gridX = Mathf.RoundToInt((worldPosition.x - worldBottomLeft.x - gridSize / 2) / gridSize);
        int gridY = Mathf.RoundToInt((worldPosition.z - worldBottomLeft.z - gridSize / 2) / gridSize);
        //Debug.Log(worldPosition);
        //Debug.Log(gridX + " , "+gridY);
        return grid[gridX, gridY];
    }

    public Node nodeFromGrid(GridID g)
    {
        return grid[g.x, g.y];
    }

    public Vector3 gridToWorldPosition(int x, int y)
    {
        return grid[x,y].worldPosition;
    }

    public Vector3 gridToWorldPosition(GridID id)
    {
        return grid[id.x, id.y].worldPosition;
    }

    public int Dis(GridID id1, GridID id2)
    {
        return (int)(Math.Round(Math.Sqrt((id1.x - id2.x) * (id1.x - id2.x) + (id1.y - id2.y) * (id1.y - id2.y))));
    }

    public int Dis(Node n1, Node n2)
    {
        return (int)(Math.Round(Math.Sqrt((n1.gridId.x - n2.gridId.x) * (n1.gridId.x - n2.gridId.x) + (n1.gridId.y - n2.gridId.y) * (n1.gridId.y - n2.gridId.y))));
    }

    void CreateGrid()
    {
        Vector3 worldPoint;
        int sur;
        int h;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                worldPoint = worldBottomLeft + Vector3.right * (x * gridSize + gridSize/2) + Vector3.forward * (y * gridSize + gridSize / 2);
                
                grid[x, y] = new Node(false, worldPoint, new GridID(x,y));
                sur = Globals.rd.Next(XMLLoader.Instance.GSurSearch[Globals.BASIC_MAP_SUR].begin, XMLLoader.Instance.GSurSearch[Globals.BASIC_MAP_SUR].end + 1);
                grid[x, y].terrainIndex = sur;
                grid[x, y].surfaceId = XMLLoader.Instance.GsurIndex[sur].id;
                xSample = (x + _seedX) / _relief;
                zSample = (y + _seedZ) / _relief;
                h = (int)(Mathf.PerlinNoise(xSample, zSample) * 20);
                if (h > 10)
                {
                    grid[x, y].growGrass();
                }
                bool tree = Globals.rd.Next(0, 100) > 95 ? true : false;
                if (tree)
                {
                    grid[x, y].growTree();
                }
            }
        }
    }

    public void spawnAll()
    {
        Debug.Log("spawnAll");
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if(grid[x, y].m_grass!=null)
                {
                    grid[x, y].m_grass.spawn();
                }
                if (grid[x, y].m_tree != null)
                {
                    grid[x, y].m_tree.spawn();
                }
            }
        }
    }

    public void Clear()
    {
        Debug.Log("Clear");
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (grid[x, y].m_grass != null)
                {
                    grid[x, y].deleteGrass();
                }
                if (grid[x, y].m_tree != null)
                {
                    grid[x, y].deleteTree();
                }
            }
        }
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridId.x + x;
                int checkY = node.gridId.y + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }
}

