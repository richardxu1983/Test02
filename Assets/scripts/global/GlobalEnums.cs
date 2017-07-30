using System;
using UnityEngine;

public enum faceTo
{
    up,
    down,
    right,
    left,
    none,
};

public enum UnitIntAttr
{
    uid,
    hp,
    hpMax,
    skinColor,
    headSkin,
    bodySkin,
    cloth,
    grid_x,
    grid_y,
}

public enum UnitFloatAttr
{
    run_speed,
    walk_speed,
}

public enum UnitOp
{
    idle,
    moving,
    walking,
}

public enum UnitAi
{
    idle,
    wander,
    moveTo,
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


public static class Globals
{
    public static int wander_walk_dis = 5;
    public static int wander_walk_ran = 1;
    public static int wander_idle_time = 3;
    public static int wander_idle_ran = 1;
    public static int humanType = 1000;
    public static int animalType = 2000;
    public static System.Random rd = new System.Random();
}

public class utils : UnitySingleton<utils>
{

    public Color[] humanSkinColors;
    public ColorNew[] humanSkin255;
    public int[] humanHeads;
    public int[] humanBodys;
    public int[] animalBodys;

    public void init()
    {

        humanSkin255 = new ColorNew[10];
        humanSkin255[0].set(255.0f, 255.0f, 255.0f);
        humanSkin255[1].set(128.0f, 128.0f, 128.0f);
        humanSkin255[2].set(128.0f, 128.0f, 255.0f);
        humanSkin255[3].set(255.0f, 128.0f, 128.0f);
        humanSkin255[4].set(128.0f, 255.0f, 128.0f);
        humanSkin255[5].set(63.0f, 128.0f, 255.0f);
        humanSkin255[6].set(255.0f, 63.0f, 128.0f);
        humanSkin255[7].set(128.0f, 255.0f, 63.0f);
        humanSkin255[8].set(63.0f, 255.0f, 128.0f);
        humanSkin255[9].set(128.0f, 63.0f, 63.0f);

        humanSkinColors = new Color[10];
        for(int i=0;i<10;i++)
        {
            humanSkinColors[i].r = humanSkin255[i].r / 255.0f;
            humanSkinColors[i].g = humanSkin255[i].g / 255.0f;
            humanSkinColors[i].b = humanSkin255[i].b / 255.0f;
            humanSkinColors[i].a = 1.0f;
        }

        humanHeads = new int[1];
        humanHeads[0] = 1000;

        humanBodys = new int[1];
        humanBodys[0] = 1000;

        animalBodys = new int[1];
        animalBodys[0] = 2000;
    }

    public Color getHumanColor(int v)
    {
        return humanSkinColors[v];
    }

    public int getHumanHeadById(int v)
    {
        return humanHeads[0];
    }

    public int getHumanBodyById(int v)
    {
        return humanBodys[0];
    }

    public int getAnimalBodyById(int v)
    {
        return animalBodys[0];
    }
}