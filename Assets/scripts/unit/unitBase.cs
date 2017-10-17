using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    down,
    sleep,
    die,
}

[Serializable]
public enum STU
{
    stand,
    down,
}

[Serializable]
public enum EMO
{
    normal,
    hurt,
    die,
    sleep,
}

[Serializable]
public enum AI
{
    idle,
    wander,
    moveTo,
    die,
    sleep,
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
    private int[] iAttr;             //int 属性
    private float[] fAttr;           //float 属性
    private bool m_isDead = false;   //是否死亡
    private int tick = 0;
    private int tickMax = timeData.Instance.TIME_IN_TICK;
    private int[] buffExist;
    public int unitAngle = 0;
    public string emotion = "normal";
    public STU m_sta = STU.stand;
    public EMO m_emo = EMO.normal;

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
        //Debug.Log("ai.op="+ai.op);
        if(!dead)
        {
            //每游戏分钟一次
            ai.loop();
            if (tick >= tickMax)
            {
                //每秒一次
                tick = 0;
                BuffLoop();
                AttrLoop();
                SelfAiLoop();
            }
        }
        else
        {
            if (tick >= tickMax)
            {
                tick = 0;
                int t = iGet(UIA.dead_time);
                if(t >= unitDefault.Instance.data.corpseTime)
                {
                    toDelete = 1;
                }
                else
                {
                    t++;
                    iSet(UIA.dead_time, t);
                }
            }
        }
    }

    void AttrLoop()
    {
        int t = iGet(UIA.fullTick);
        bool isDay = GTime.Instance.IsDay();
        bool isSleep = ai.op == OP.sleep ? true : false;

        t++;
        if( t >= iGet(UIA.fullDecSec))
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

        if (isSleep)
        {
            //Debug.Log("睡眠中 : "+ iGet(UIA.energyRegSec)+" , "+ iGet(UIA.energyReg));
            //如果在睡眠状态，则恢复
            if (t >= iGet(UIA.energyRegSec))
            {
                int v = iGet(UIA.energy);
                v = v + iGet(UIA.energyReg);
                energy = v;
                t = 0;
            }
            iSet(UIA.energyTick, t);
        }
        else
        {
            //不在睡眠状态，消耗
            if (t >= iGet(UIA.energyDecSec))
            {
                int v = iGet(UIA.energy);
                if (v > 0)
                {
                    if (isDay)
                    {
                        //白天
                        v -= iGet(UIA.energyDec);
                    }
                    else
                    {
                        //夜晚
                        v -= iGet(UIA.energyDecNight);
                    }
                    energy = v;
                }
                t = 0;
            }
            iSet(UIA.energyTick, t);
        }
        SelfAttrLoop();
    }

    public int energy
    {
        get { return iGet(UIA.energy); }
        set
        {
            //Debug.Log("value=" + value);
            int v = value;
            v = v < 0 ? 0 : v;
            v = v > iGet(UIA.energyMax) ? iGet(UIA.energyMax) : v;
            if (v < iGet(UIA.tired_slight) && v >= iGet(UIA.tired_medium))
            {
                TryAddBuff(4);
            }
            else if (v < iGet(UIA.tired_medium) && v >= iGet(UIA.tired_extream))
            {
                TryAddBuff(5);
            }
            else if (v < iGet(UIA.tired_extream))
            {
                TryAddBuff(6);
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
            if (v < iGet(UIA.hungry_slight) && v >= iGet(UIA.hungry_medium))
            {
                TryAddBuff(0);
            }
            else if (v < iGet(UIA.hungry_medium) && v >= iGet(UIA.hungry_extream))
            {
                TryAddBuff(1);
            }
            else if (v < iGet(UIA.hungry_extream))
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
        if(c.deleteCheck())
        {
            TryDeleteBuff(c.id);
        }
        else
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
        }
        AttrCheck();
    }

    public virtual void SelfAttrLoop()
    {
    }

    public virtual void SelfAiLoop()
    {
    }

    public bool hasBuff(int id)
    {
        return buffExist[id] == 1 ? true : false;
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
                    //Debug.Log("cover:" + cover + " , buffExist[cover]="+ buffExist[cover]);
                    if (buffExist[cover] == 1)
                    {
                        buff[id].onCover(cover);
                        TryDeleteBuff(cover);
                    }
                }
            }
            else
            {
                AddDeBuff(id);
                if (cover > -1)
                {
                    //Debug.Log("cover:" + cover + " , buffExist[cover]=" + buffExist[cover]);
                    if (buffExist[cover]==1)
                    {
                        debuff[id].onCover(cover);
                        TryDeleteBuff(cover);
                    }
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
        if (dead)
            return;
 
        int m = Globals.MOOD_BASE;
        bool down = false;

        foreach (KeyValuePair<int, Condition> v in buff)
        {
            m += processCondAttr(v.Value);
            down = down || v.Value.down;
        }
        foreach (KeyValuePair<int, Condition> v in debuff)
        {
            m += processCondAttr(v.Value);
            down = down || v.Value.down;
        }
        mood = m;
        if(down&&ai.op!=OP.down)
        {
            setOp(OP.down);
        }
        else if(!down&&ai.op==OP.down)
        {
            setOp(OP.idle);
        }
    }

    int processCondAttr(Condition c)
    {
        int m = 0;
        m += conditionData.Instance.get(c.id).mood;
        m += c.coverMood;
        m += c.intvalData;
        return m;
    }

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
        ai.die();
        //m_skin.die();
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

    public void setOp(OP v)
    {
        ai.op = v;
    }

    public OP getOp()
    {
        return ai.op;
    }

    public void setAi(AI v)
    {
        ai.ai = v;
    }

    public AI getAi()
    {
        return ai.ai;
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
    public int m_tickMax = timeData.Instance.TIME_IN_TICK;
    public int timeLeft;
    public OP m_op = OP.idle;
    public AI m_ai = AI.idle;
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

    public OP op
    {
        get { return m_op; }
        set
        {
            if( value==OP.down || value == OP.sleep || value == OP.die)
            {
                stopMove();
                baseUnit.m_skin.statusTag = 1;//躺下
            }
            if((m_op == OP.down || m_op == OP.sleep)&&(value != OP.down && value != OP.sleep && value != OP.die))
            {
                baseUnit.m_skin.statusTag = 2;//站起
            }

            if (value == OP.down)
            {
                baseUnit.emotion = "hurt";
            }
            else if(value == OP.die)
            {
                baseUnit.emotion = "dead";
            }
            else
            {
                baseUnit.emotion = "normal";
            }

            m_op = value;
            //Debug.Log(m_op);
        }
    }

    public void stopMove()
    {
        path.Clear();
        pathIndex = 0;
        targetUnit = default(unitBase);
    }

    public AI ai
    {
        get { return m_ai; }
        set
        {
            if(value!=AI.sleep)
            {
                if (op == OP.sleep)
                    op = OP.idle;
            }
            m_ai = value;
        }
    }

    public bool cmdMode()
    {
        return reason == AIR.cmd ? true : false;
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
        stopMove();
        ai = AI.die;
        op = OP.die;
        Debug.Log("die");
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

    //移动到某个位置
    public void moveTo(GridID v, bool isCmd)
    {
        if (baseUnit.dead)
            return;

        if (op == OP.down || op == OP.sleep)
            return;

        ai = AI.moveTo;
        op = OP.moving;

        stopMove();

        baseUnit.targetGrid = v;
        
        PathFind.Instance.FindPath(GSceneMap.Instance.nodeFromGrid(baseUnit.grid), GSceneMap.Instance.nodeFromGrid(baseUnit.targetGrid), ref path);

        if (isCmd)
        {
            reason = AIR.cmd;
            tick = m_tickMax;
        }
    }

    //移动到某个实体
    public void TryMoveToTarget(entity t, bool isCmd)
    {
        if (baseUnit.dead)
            return;

        if (op == OP.down || op == OP.sleep)
            return;

        if (baseUnit.v3Dis(t) <= 3)
            return;

        stopMove();

        targetUnit = t;
        baseUnit.targetGrid = t.grid;
        ai = AI.moveTo;
        op = OP.moving;

        PathFind.Instance.FindPath(GSceneMap.Instance.nodeFromGrid(baseUnit.grid), GSceneMap.Instance.nodeFromGrid(t.grid), ref path);

        if (isCmd)
        {
            reason = AIR.cmd;
            tick = m_tickMax;
        }
    }

    public void moveToTarget()
    {
        if (op == OP.down || op == OP.sleep)
            return;

        //Debug.Log("moveToTarget");
        if (baseUnit.dead || baseUnit.v3Dis(targetUnit) <= 3)
        {
            ai = AI.idle;
            op = OP.idle;
            reason = AIR.none;
            stopMove();
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
        if (op == OP.down || op == OP.sleep)
            return;

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
        if (op == OP.down || op == OP.sleep)
            return;
 
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