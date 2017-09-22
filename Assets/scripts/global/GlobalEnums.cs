using System;
using UnityEngine;

[Serializable]
public enum faceTo
{
    up,
    down,
    right,
    left,
    none,
};

public enum UnitOp
{
    idle,
    moving,
    walking,
    die,
}

public enum UnitAi
{
    idle,
    wander,
    moveTo,
    die,
}

public struct ColorNew
{
    public float r;
    public float g;
    public float b;
    public void set(float rr,float gg,float bb)
    {
        r = rr;
        g = gg;
        b = bb;
    }
}

public enum UnitAiReason
{
    none,
    cmd,
    wander_walk,
    wander_idle,
}


public enum Side : byte { Right, Left, Top, Bottom };
public enum Position : byte { TopLeft, TopCenter, TopRight, MiddleLeft, MiddleCenter, MiddleRight, BottomLeft, BottomCenter, BottomRight }

public class utils : UnitySingleton<utils>
{

    public Color[] SkinColors;
    public int maxColors;


    public void init()
    {
        SkinColors = new Color[Globals.MAX_SKINCOLOR_CONFIG_NUM];
        maxColors = 0;
    }

    public Color getSkinColor(int v)
    {
        return SkinColors[v];
    }

    public Color getrandomColor()
    {
        int skinColorId = Globals.rd.Next(maxColors);
        return SkinColors[skinColorId];
    }

}