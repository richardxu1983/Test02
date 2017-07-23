using System;

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
    public static Random rd = new Random();
}