﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum UIA
{
    uid,
    hp,
    hpMax,
    skinColorId,
    headSkin,
    bodySkin,
}

public enum UFA
{
    run_speed,
    walk_speed,
}

[Serializable]
public enum OP
{
    idle,
    moving,
    walking,
    die,
}

[Serializable]
public enum AI
{
    idle,
    wander,
    moveTo,
    die,
}

[Serializable]
public enum AIR
{
    none,
    cmd,
    wander_walk,
    wander_idle,
}

[Serializable]
public class unitBase : entity
{

    private bool    m_isDead = false;
    private int[]   iAttr;
    private float[] fAttr;
    public GridID   targetGrid;
    public entity   target;
    public UnitAiBase ai;
    public faceTo playerFace = faceTo.down;//0:down,1:up,2:left,3:right
    public faceTo playerFaceLast = faceTo.down;//0:down,1:up,2:left,3:right

    [NonSerialized]
    public Color            skinColor;
    [NonSerialized]
    public bool             bDebugInfo;
    [NonSerialized]
    public GameObject       m_Instance;
    [NonSerialized]
    public unitMovement     m_movement;
    [NonSerialized]
    public unitUI           m_unitUI;
    [NonSerialized]
    public unitSkin         m_skin;
    [NonSerialized]
    public bool m_setFinish = false;

    public unitBase(int _type,int _tid):base(_type, _tid)
    {
        iAttr = new int[64];
        fAttr = new float[16];
        ai = new UnitAiBase();
        ai.init(this);
        targetGrid = new GridID(0,0);
    }

    public void spawn(int x, int y)
    {
        Vector3 v = GSceneMap.Instance.gridToWorldPosition(new GridID(x, y));
        grid = new GridID(x, y);
        m_pos = v;
        m_Instance = UnityEngine.Object.Instantiate(Resources.Load("Prefab/unit"), v, new Quaternion(0, 0, 0, 0)) as GameObject;
        m_Instance.transform.parent = GameObject.Find("units").transform;
        m_movement = m_Instance.GetComponent<unitMovement>();
        m_skin = m_Instance.GetComponent<unitSkin>();
        m_unitUI = m_Instance.GetComponent<unitUI>();
        m_movement.init(this);
        m_skin.init(this);
        m_unitUI.init(this);
    }

    public void spawn()
    {
        skinColor = utils.Instance.getSkinColor(skinColorId);
        m_Instance = UnityEngine.Object.Instantiate(Resources.Load("Prefab/unit"), m_pos, new Quaternion(0, 0, 0, 0)) as GameObject;
        m_Instance.transform.parent = GameObject.Find("units").transform;
        m_movement = m_Instance.GetComponent<unitMovement>();
        m_skin = m_Instance.GetComponent<unitSkin>();
        m_unitUI = m_Instance.GetComponent<unitUI>();
        m_movement.init(this);
        m_skin.init(this);
        m_unitUI.init(this);
        ai.init(this);
    }

    public void loop()
    {
        ai.loop();
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

    public int skinColorId
    {
        set { iSet(UIA.skinColorId, value); }
        get { return iGet(UIA.skinColorId); }
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

[Serializable]
public class UnitAiBase
{
    public int tick;
    public int m_tickMax;
    public int timeLeft;
    public OP op = OP.idle;
    public AI ai = AI.idle;
    public AIR reason = AIR.none;
    public entity targetUnit;
    public List<Node> path;
    public int pathIndex;
    [NonSerialized]
    public unitBase baseUnit;

    public UnitAiBase()
    {

    }

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

    public void init(unitBase m)
    {
        baseUnit = m;
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
                doMoveTo();
                break;

            default:
                break;
        }
    }

    public void moveTo(GridID v, bool isCmd)
    {
        if (baseUnit.dead)
            return;

        ai = AI.moveTo;
        op = OP.moving;
        baseUnit.targetGrid = v;
        targetUnit = default(unitBase);

        path.Clear();
        PathFind.Instance.FindPath(GSceneMap.Instance.nodeFromGrid(baseUnit.grid), GSceneMap.Instance.nodeFromGrid(baseUnit.targetGrid), ref path);
        pathIndex = 0;

        if (isCmd)
        {
            reason = AIR.cmd;
            tick = m_tickMax;
        }
    }

    public void TryMoveToTarget(entity t, bool isCmd)
    {
        if (baseUnit.dead)
            return;

        if (baseUnit.v3Dis(t) <= 3)
            return;

        targetUnit = t;
        baseUnit.targetGrid = t.grid;
        ai = AI.moveTo;
        op = OP.moving;
        path.Clear();
        PathFind.Instance.FindPath(GSceneMap.Instance.nodeFromGrid(baseUnit.grid), GSceneMap.Instance.nodeFromGrid(t.grid), ref path);
        pathIndex = 0;

        if (isCmd)
        {
            reason = AIR.cmd;
            tick = m_tickMax;
        }
    }

    public void moveToTarget()
    {
        if (baseUnit.dead || baseUnit.v3Dis(targetUnit) <= 3)
        {
            ai = AI.idle;
            op = OP.idle;
            reason = AIR.none;
            targetUnit = default(entity);
            path.Clear();
        }

        if(baseUnit.targetGrid!= targetUnit.grid)
        {
            path.Clear();
            PathFind.Instance.FindPath(GSceneMap.Instance.nodeFromGrid(baseUnit.grid), GSceneMap.Instance.nodeFromGrid(targetUnit.grid), ref path);
            pathIndex = 0;
        }
    }

    public void TryToMoveToVector3(Vector3 pos, bool isCmd)
    {
        Node n = GSceneMap.Instance.nodeFromWorldPoint(pos);
        if (!n.block)
        {
            moveTo(n.gridId, isCmd);
        }
    }

    public GridID nextGrid()
    {
        if (pathIndex >= path.Count)
        {
            return null;
        }
        else
        {
            return path[pathIndex].gridId;
        }
    }

    public void doMoveTo()
    {
        if(targetUnit!=null)
        {
            moveToTarget();
        }
        else
        {
            if (MoveToPos())
            {
                ai = AI.idle;
                op = OP.idle;
                reason = AIR.none;
            }
        }
    }

    public bool MoveToPos()
    {
        if (path.Count > 0)
        {
            if (pathIndex >= path.Count)
            {
                return true;
            }
            else
            {
                //Debug.Log("x:"+path[pathIndex].gridId.x+","+ "y:" + path[pathIndex].gridId.y+ " |  x:" + baseUnit.grid.x + "," + "y:" + baseUnit.grid.y);
                if (path[pathIndex].gridId == baseUnit.grid)
                {
                    //Debug.Log(baseUnit.pos + " : " + path[pathIndex].worldPosition);
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