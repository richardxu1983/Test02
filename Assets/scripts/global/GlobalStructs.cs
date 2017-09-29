using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

public struct ST_Condition
{
    public int id;
    public string name;
    public int cover;
    public bool buff;
    public int mood;
    public int duration;
    public UIA deleteAttr;
    public int deleteValue;
    public int deleteTime;
    public int trigTime;
    public int trigCondi;
    public int trigAction;
}

public struct ST_GSURS
{
    public int begin;
    public int end;
}

public struct ST_GSurface
{
    public int id;
    public int type;
    public int typeId;
    public string name;
    public bool canPlant;
}

[Serializable]
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
        if ((object)lhs == null && (object)rhs == null)
        {
            return true;
        }
        if ((object)lhs == null || (object)rhs == null)
        {
            return false;
        }

        if (lhs.x== rhs.x&& lhs.y == rhs.y)
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