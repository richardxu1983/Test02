using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{

    public bool block;
    public Vector3 worldPosition;
    public GridID gridId;
    public int gCost;
    public int hCost;
    public Node parent;
    public int terrainIndex;
    public int surfaceId;
    int heapIndex;

    public Node(bool _block,Vector3 _worldPosition, GridID _gridId)
    {
        block = _block;
        worldPosition = _worldPosition;
        gridId = _gridId;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
