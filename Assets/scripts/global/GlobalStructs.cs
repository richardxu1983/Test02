using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ST_AnimalConfig
{
    public int typeId;
    public int bodyId;
    public int headId;
    public string name;
    public float speed;
    public float r;
    public float g;
    public float b;
}

public class GridID
{
    public int x;
    public int y;

    public GridID(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public static GridID operator +(GridID lhs, GridID rhs)
    {
        GridID result = new GridID(lhs.x, lhs.y);
        result.x += rhs.x;
        result.y += rhs.y;
        return result;
    }

    public static bool operator ==(GridID lhs, GridID rhs)
    {
        if(lhs.x== rhs.x&& lhs.y == rhs.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator !=(GridID lhs, GridID rhs)
    {
        return !(lhs == rhs);
    }
}