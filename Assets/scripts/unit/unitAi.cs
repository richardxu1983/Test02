using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitAi {

    UnitOp m_op = UnitOp.idle;
    UnitAi m_ai = UnitAi.idle;
    public GridID TargetPos;
    public GridID wanderPos;
    public bool wanderByPoint;
    public int m_tick;
    public int m_tickMax;
    public int timeLeft;
    public UnitAiReason reason = UnitAiReason.none;
    private unitManager m_manager;

    public UnitOp op
    {
        get { return m_op; }
        set { m_op = value; }
    }

    public UnitAi ai
    {
        get { return m_ai; }
        set { m_ai = value; }
    }

    public int tick
    {
        get { return m_tick; }
        set { m_tick = value; }
    }

    public unitAi()
    {
        m_tick = 0;
        m_tickMax = 4;
    }

    public bool arriveTPos()
    {
        //Debug.Log("TargetPos : " + m_manager.ai().TargetPos.x + " , " + m_manager.ai().TargetPos.y);
        //Debug.Log("grid : " + m_manager.grid().x + " , " + m_manager.grid().y);
        return m_manager.ai().TargetPos == m_manager.grid() ? true : false;
    }

    public void init(unitManager m)
    {
        m_manager = m;
    }

    public void setReason(UnitAiReason v)
    {
        reason = v;
    }

    public void loop()
    {
        if(m_tick>=m_tickMax)
        {
            Do();
            m_tick = 0;
        }
        else
        {
            m_tick++;
        }
    }

    public void die()
    {
        ai = UnitAi.die;
        op = UnitOp.die;
    }

    public void Do()
    {
        switch (m_ai)
        {
            case UnitAi.idle:
                wander(true);
                break;

            case UnitAi.moveTo:
                doMoveTo();
                break;

            case UnitAi.wander:
                doWander();
                break;

            default:
                break;
        }
    }

    public void wander(bool byPoint)
    {
        ai = UnitAi.wander;
        setReason(UnitAiReason.wander_walk);
        wanderByPoint = byPoint;
        if(byPoint)
        {
            wanderPos = m_manager.grid();
            TargetPos = wanderPos + getWanderPos();
        }
        else
        {
            TargetPos = m_manager.grid() + getWanderPos();
        }
        timeLeft = 0;
    }

    public GridID getWanderPos()
    {
        int dis = Mathf.RoundToInt( Globals.wander_walk_dis + Globals.rd.Next(0, 2*Globals.wander_walk_ran) - Globals.wander_walk_ran);
        float an = (float)Globals.rd.NextDouble() * 6.28f;
        //Debug.Log(an);
        int x = Mathf.RoundToInt(dis * Mathf.Cos(an));
        int y = Mathf.RoundToInt(dis * Mathf.Sin(an));
        //Debug.Log("Mathf.Cos(an) : " + Mathf.Cos(an) + " , Mathf.Sin(an) : " + Mathf.Sin(an) + " , dis="+ dis);
        return new GridID(x, y);
    }

    public void moveTo(GridID v, bool isCmd)
    {
        if (m_manager.isDead())
            return;

        ai = UnitAi.moveTo;
        op = UnitOp.moving;
        TargetPos = v;
        TargetPos.y = 0;

        if (isCmd)
        {
            reason = UnitAiReason.cmd;
            m_tick = m_tickMax;
        }
    }

    public void doMoveTo()
    {
        if (arriveTPos())
        {
            ai = UnitAi.idle;
            op = UnitOp.idle;
            reason = UnitAiReason.none;
        }
    }

    public void doWander()
    {
        //Debug.Log("doWander");
        if (reason == UnitAiReason.wander_walk)
        {
            if(arriveTPos())
            {
                reason = UnitAiReason.wander_idle;
                timeLeft = Globals.wander_idle_time + Globals.rd.Next(0, 2 * Globals.wander_idle_ran) - Globals.wander_idle_ran;
                op = UnitOp.idle;
            }
            else
            {
                op = UnitOp.moving;
            }
        }
        else if(reason == UnitAiReason.wander_idle)
        {
            timeLeft--;
            if (timeLeft > 0)
            {
                op = UnitOp.idle;
            }
            else
            {
                setReason(UnitAiReason.wander_walk);
                if (wanderByPoint)
                {
                    TargetPos = wanderPos + getWanderPos();
                }
                else
                {
                    TargetPos = m_manager.grid() + getWanderPos();
                }
            }
        }
    }
}
