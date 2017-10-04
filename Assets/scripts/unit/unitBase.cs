using System.Collections;
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
    mood,
    moodMax,
    moodLevel,
    full,
    fullMax,
    fullDec,
    fullDecSec,
    fullTick,
    hungry,
    exHungry,
    energy,
    energyDec,
    energyDecSec,
    energyMax,
    energyTick,
    tired,
    exhausted,
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

    private int[] iAttr;              //int 属性
    private float[] fAttr;              //float 属性
    private bool m_isDead = false;   //是否死亡
    private int tick = 0;
    private int tickMax = 4;
    private int[] buffExist;

    public GridID targetGrid;
    public entity target;
    public UnitAiBase ai;
    public faceTo playerFace = faceTo.down;//0:down,1:up,2:left,3:right
    public faceTo playerFaceLast = faceTo.down;//0:down,1:up,2:left,3:right

    public Dictionary<int, Condition> buff;       //buff
    public Dictionary<int, Condition> debuff;     //debuff

    [NonSerialized]
    public Color skinColor;
    [NonSerialized]
    public bool bDebugInfo;
    [NonSerialized]
    public GameObject m_Instance;
    [NonSerialized]
    public unitMovement m_movement;
    [NonSerialized]
    public unitUI m_unitUI;
    [NonSerialized]
    public unitSkin m_skin;
    [NonSerialized]
    public bool m_setFinish = false;

    public unitBase(int _type, int _tid) : base(_type, _tid)
    {
        iAttr = new int[256];
        fAttr = new float[16];
        ai = new UnitAiBase();
        buff = new Dictionary<int, Condition>();
        debuff = new Dictionary<int, Condition>();
        buffExist = new int[64];
        targetGrid = new GridID(0, 0);
        iSet(UIA.hungry, unitDefault.Instance.hungry);
        iSet(UIA.exHungry, unitDefault.Instance.exHungey);
        iSet(UIA.tired, unitDefault.Instance.tired);
        iSet(UIA.exhausted, unitDefault.Instance.exhausted);
        ai.init(this);
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
        tick++;
        if (tick >= tickMax)
        {
            //每秒一次
            tick = 0;
            AttrLoop();
            BuffLoop();
        }
        ai.loop();
    }

    void AttrLoop()
    {
        int t = iGet(UIA.fullTick);
        t++;
        if( t >= unitDefault.Instance.fullDecSec )
        {
            int v = iGet(UIA.full);
            if (v > 0)
            {
                v -= iGet(UIA.fullDec);
                full = v;
            }
            t = 0;
        }
        iSet(UIA.fullTick, t);

        t = iGet(UIA.energyTick);
        t++;
        if (t >= unitDefault.Instance.energyDecSec)
        {
            int v = iGet(UIA.energy);
            if (v > 0)
            {
                v -= iGet(UIA.energyDec);
                energy = v;
            }
            t = 0;
        }
        iSet(UIA.energyTick, t);

        SelfLoop();
    }

    public int energy
    {
        get { return iGet(UIA.energy); }
        set
        {
            int v = value;
            v = v < 0 ? 0 : v;
            v = v > iGet(UIA.energyMax) ? iGet(UIA.energyMax) : v;
            if (v < iGet(UIA.tired) && v >= iGet(UIA.exhausted))
            {
                TryAddBuff(4);
            }
            else if (v < iGet(UIA.exhausted))
            {
                //Debug.Log("111111111");
                TryAddBuff(5);
            }
            //Debug.Log("iGet(UIA.energyMax)=" + iGet(UIA.energyMax) + " , value=" + value);
            iSet(UIA.energy, v);
        }
    }

    public int full
    {
        get { return iGet(UIA.full); }
        set
        {
            int v = value;
            v = v < 0 ? 0 : v;
            v = v > iGet(UIA.fullMax) ? iGet(UIA.fullMax) : v;
            if (v < unitDefault.Instance.slightHungry && v >= iGet(UIA.hungry))
            {
                TryAddBuff(0);
            }
            else if (v < iGet(UIA.hungry) && v >= iGet(UIA.exHungry))
            {
                TryAddBuff(1);
            }
            else if (v < iGet(UIA.exHungry))
            {
                TryAddBuff(2);
            }
            //Debug.Log("iGet(UIA.fullMax)=" + iGet(UIA.fullMax) + " , value="+ value);
            iSet(UIA.full, v);
        }
    }

    void BuffLoop()
    {
        foreach (KeyValuePair<int, Condition> v in buff)
        {
            buffCheck(v.Value);
        }
        foreach (KeyValuePair<int, Condition> v in debuff)
        {
            buffCheck(v.Value);
        }
    }

    public void buffCheck(Condition c)
    {
        int t = GTime.Instance.GTick;
        //持续时间
        int dura = conditionData.Instance.get(c.id).duration;
        if (dura > 0)
        {
            if (t - c.startTime >= dura)
            {
                TryDeleteBuff(c.id);
            }
        }

        int trigTime = conditionData.Instance.get(c.id).trigTime;
        //Debug.Log("trigTime=" + trigTime);
        if (trigTime > 0)
        {
            //Debug.Log("t=" + t+ " , c.startTime="+ c.startTime);
            if (t - c.startTime >= trigTime)
            {
                int trigCondi = conditionData.Instance.get(c.id).trigCondi;
                if (trigCondi > -1)
                {
                    TryAddBuff(trigCondi);
                }
            }
        }

        int actionTime = conditionData.Instance.get(c.id).actionTime;
        if (actionTime > 0)
        {
            if (t - c.startTime >= actionTime)
            {
                int trigAction = conditionData.Instance.get(c.id).trigAction;
                //Debug.Log("trigAction=" + trigAction);
                switch (trigAction)
                {
                    case 1:
                        {
                            hp = iGet(UIA.hpMax) * -1;
                            break;
                        }
                }
            }
        }
        c.loop();
        AttrCheck();
    }

    public virtual void SelfLoop()
    {
    }

    public void TryAddBuff(int id)
    {
        //Debug.Log("尝试添加buff:" + id);
        if (buffExist[id] == 0)
        {
            int cover = conditionData.Instance.get(id).cover;

            if (conditionData.Instance.get(id).buff)
            {
                AddBuff(id);
                if (cover > -1)
                {
                    buff[id].onCover(cover);
                    TryDeleteBuff(cover);
                }
            }
            else
            {
                AddDeBuff(id);
                if (cover > -1)
                {
                    debuff[id].onCover(cover);
                    TryDeleteBuff(cover);
                }
            }

            buffExist[id] = 1;
        }
    }

    public void AddBuff(int id)
    {
        Condition c = new Condition(id, this);
        buff.Add(id, c);
        //Debug.Log("buff:" + id+"添加成功");
        AttrCheck();
    }

    public void AddDeBuff(int id)
    {
        Condition c = new Condition(id, this);
        debuff.Add(id, c);
        //Debug.Log("buff:" + id + "添加成功");
        AttrCheck();
    }

    public void TryDeleteBuff(int id)
    {
        buffExist[id] = 0;
        if (conditionData.Instance.get(id).buff)
        {
            buff.Remove(id);
            AttrCheck();
        }
        else
        {
            debuff.Remove(id);
            AttrCheck();
        }
    }

    public void AttrCheck()
    {
        int m = Globals.MOOD_BASE;

        foreach (KeyValuePair<int, Condition> v in buff)
        {
            m += processCondAttr(v.Value);
        }
        foreach (KeyValuePair<int, Condition> v in debuff)
        {
            m += processCondAttr(v.Value);
        }
        mood = m;
    }

    int processCondAttr(Condition c)
    {
        int m = 0;
        m += conditionData.Instance.get(c.id).mood;
        m += c.coverMood;
        m += c.intvalData;
        return m;
    }

    //public int hp() { return iGet(UIA.hp); }

    public int hp
    {
        get { return iGet(UIA.hp); }
        set
        {
            int h = value;
            h = h < 0 ? 0 : h;
            h = h > iGet(UIA.hpMax) ? iGet(UIA.hpMax) : h;
            iSet(UIA.hp, h);
            if (h <= 0 && dead == false)
            {
                die();
            }
        }
    }

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

    public int mood
    {
        get { return iGet(UIA.mood); }
        set
        {
            int m = value;
            m = m < 0 ? 0 : m;
            m = m > 100 ? 100 : m;
            //Debug.Log(m);
            if(m<Globals.MOOD_LEVEL_1)
            {
                iSet(UIA.moodLevel, 1);
            }
            else if(m <= Globals.MOOD_LEVEL_2 && m > Globals.MOOD_LEVEL_1)
            {
                iSet(UIA.moodLevel, 2);
            }
            else if (m <= Globals.MOOD_LEVEL_3 && m > Globals.MOOD_LEVEL_2)
            {
                iSet(UIA.moodLevel, 3);
            }
            else if (m <= Globals.MOOD_LEVEL_4 && m > Globals.MOOD_LEVEL_3)
            {
                iSet(UIA.moodLevel, 4);
            }
            else if(m > Globals.MOOD_LEVEL_4)
            {
                iSet(UIA.moodLevel, 5);
            }
            //Debug.Log("moodLevel="+iGet(UIA.moodLevel));
            iSet(UIA.mood, m);
        }
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
        int h = this.hp;
        if (h > 0)
        {
            h += v;
            hp = h;
        }
    }

    public void onFreeSelect()
    {
        bDebugInfo = false;
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
        pathIndex = 0;
        path = new List<Node>();
    }

    public void loop()
    {
        //Debug.Log("loop");
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
        //Debug.Log("moveToTarget");
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
        //Debug.Log("doMoveTo");
        if (targetUnit!=null)
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
        //Debug.Log("MoveToPos : path.Count="+ path.Count);
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