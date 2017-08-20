using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node{

    public bool block;
    public Vector3 worldPosition;

    public Node(bool _block,Vector3 _worldPosition)
    {
        block = _block;
        worldPosition = _worldPosition;
    }

}
