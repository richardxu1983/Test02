using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float gridDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        gridDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / gridDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / gridDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0 ; x < gridSizeX ; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * gridDiameter + nodeRadius) + Vector3.forward * (y * gridDiameter + nodeRadius);
                bool block = Globals.rd.Next(0, 10) > 7 ? true : false;
                grid[x, y] = new Node(block, worldPoint);
            }
        }
    }

    public Node nodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int gridX = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int gridY = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[gridX, gridY];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if(grid!=null)
        {
            foreach(Node n in grid)
            {
                Gizmos.color = n.block ? Color.red : Color.white;
                Gizmos.DrawCube(n.worldPosition, new Vector3(1,.1f,1) * (gridDiameter - .1f));
            }
        }
    }

}
