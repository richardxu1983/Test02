using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UIA
{
    uid,
    hp,
    hpMax,
    skinColor,
    headSkin,
    bodySkin,
}

public enum UFA
{
    run_speed,
    walk_speed,
}

public enum OP
{
    idle,
    moving,
    walking,
    die,
}

public enum AI
{
    idle,
    wander,
    moveTo,
    die,
}

public enum AIR
{
    none,
    cmd,
    wander_walk,
    wander_idle,
}

public class unitBase : entity
{

    private bool    m_isDead = false;
    private int[]   iAttr;
    private float[] fAttr;
    public GridID   targetGrid;
    public entity   target;
    public Vector3  pos;
    public Color    skinColor;
    public bool     bDebugInfo;

    public unitBase(int _type,int _tid):base(_type, _tid)
    {
        iAttr = new int[64];
        fAttr = new float[16];
    }

    public void loop()
    {
    }

    public int hp() { return iGet(UIA.hp); }

    public void die()
    {
        dead = true;
        toDelete = 1;
    }

    public bool dead
    {
        set { m_isDead = value; }
        get { return m_isDead; }
    }

    public int id
    {
        set { iSet(UIA.uid, value); }
        get { return iGet(UIA.uid); }
    }

    public int hpMax
    {
        set { iSet(UIA.hpMax, value); }
        get { return iGet(UIA.hpMax); }
    }

    public float runSpeed
    {
        set { fSet(UFA.run_speed, value); }
        get { return fGet(UFA.run_speed); }
    }

    public int bodySkin
    {
        set { iSet(UIA.bodySkin, value); }
        get { return iGet(UIA.bodySkin); }
    }

    public int headSkin
    {
        set { iSet(UIA.headSkin, value); }
        get { return iGet(UIA.headSkin); }
    }

    public int iGet(UIA v)
    {
        return iAttr[(int)v];
    }

    public void iSet(UIA k, int v)
    {
        iAttr[(int)k] = v;
    }

    public float fGet(UFA v)
    {
        return fAttr[(int)v];
    }

    public void fSet(UFA k, float v)
    {
        fAttr[(int)k] = v;
    }

    public void hpAdd(int v)
    {
        int hp = this.hp();
        if (hp > 0)
        {
            hp += v;
            hp = hp < 0 ? 0 : hp;
            hp = hp > hpMax ? hpMax : hp;
            iSet(UIA.hp, hp);
        }
        if (hp <= 0 && dead == false)
        {
            die();
        }
    }

    public void onFreeSelect()
    {
        bDebugInfo = false;
    }

    public void setName(string v)
    {
        name = v;
    }
}

public class UnitAiBase
{
    public OP op = OP.idle;
    public AI ai = AI.idle;
    public int tick;
    public int m_tickMax;
    public int timeLeft;
    public AIR reason = AIR.none;
    public List<Node> path;
    public int pathIndex;
    public unitBase baseUnit;

    public void loop()
    {
        wayPointCheck();

        if (tick >= m_tickMax)
        {
            Do();
            tick = 0;
        }
        else
        {
            tick++;
        }
    }
    


    void wayPointCheck()
    {
        if (path.Count > 0)
        {
            if (path[pathIndex].gridId == baseUnit.grid)
            {
                pathIndex++;
                if (pathIndex >= path.Count)
                {
                    path.Clear();
                    pathIndex = 0;
                }
            }
        }
    }

    public void die()
    {
        ai = AI.die;
        op = OP.die;
    }

    public void Do()
    {
        switch (ai)
        {
            case AI.idle:
                break;

            case AI.moveTo:
                break;

            case AI.wander:
                break;

            default:
                break;
        }
    }

    public bool arriveTPos()
    {
        if (path.Count > 0)
        {
            if (pathIndex >= path.Count)
            {
                return true;
            }
            else
            {
                if (path[pathIndex].gridId == baseUnit.grid)
                {
                    if (Mathf.Abs(baseUnit.pos.x - path[pathIndex].worldPosition.x) >= .1f || Mathf.Abs(baseUnit.pos.z - path[pathIndex].worldPosition.z) >= .1f)
                    {
                        return false;
                    }
                    else
                    {
                        pathIndex++;
                        if (pathIndex >= path.Count)
                        {
                            path.Clear();
                            pathIndex = 0;
                            return true;
                        }
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            return true;
        }
    }
}