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
    public int grassIndex = -1;
    public int treeIndex = -1;
    public grass m_grass;
    public tree m_tree;
    int heapIndex;

    public Node(bool _block,Vector3 _worldPosition, GridID _gridId)
    {
        block = _block;
        worldPosition = _worldPosition;
        gridId = _gridId;
    }

    public void deleteGrass()
    {
        m_grass.delete();
        m_grass = default(grass);
    }

    public void deleteTree()
    {
        m_tree.delete();
        m_tree = default(tree);
        block = false;
    }

    public void growGrass()
    {
        m_grass = new grass(gridId.x, gridId.y);
        m_grass.spawn();
    }

    public void growTree()
    {
        m_tree = new tree(gridId.x, gridId.y);
        m_tree.spawn();
        block = true;
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
