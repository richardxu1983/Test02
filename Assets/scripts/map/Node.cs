using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node{

    public bool block;
    public Vector3 worldPosition;
    public GridID gridId;

    public Node(bool _block,Vector3 _worldPosition, GridID _gridId)
    {
        block = _block;
        worldPosition = _worldPosition;
        gridId = _gridId;
    }

}
