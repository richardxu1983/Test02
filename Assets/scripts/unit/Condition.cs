using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public int deleteMethod;
    public int deleteTime;
    public int trigTime;
    public int trigCondi;
    public int trigAction;
    public int actionTime;
    public int intvalTime;
    public int intvalMood;
    public int intvalMaxMood;
    public int delay;
    public int delay_act;
    public UIA delay_attr;
    public int delay_value;
    public int delay_intval;
}


public class conditionData : Singleton<conditionData>
{
    public ST_Condition[] condi;     //buff表

    public void init()
    {
        condi = new ST_Condition[256];
    }


    public ST_Condition get(int v)
    {
        return condi[v];
    }
}

[Serializable]
public class Condition
{

    public int id = -1;
    public int startTime = -1;
    public int AttrTime = -1;
    public bool buff = true;
    public unitBase unit;
    public int tick = 0;
    public int intvalData = 0;
    public int deleteTick = -1;
    public int delayData = 0;
    public int delayTick = 0;
    public bool delayFirst = true;
    public int coverMood = 0;
    public bool down = false;

    public Condition(int _id, unitBase _unit)
    {
        id = _id;
        startTime = GTime.Instance.GTick;
        unit = _unit;
        if (conditionData.Instance.get(id).deleteMethod == 0 && conditionData.Instance.get(id).deleteTime >= 0)
        {
            deleteTick = GTime.Instance.GTick;
        }
    }

    public bool deleteCheck()
    {
        if(conditionData.Instance.get(id).deleteMethod > 0 || conditionData.Instance.get(id).deleteTime>=0)
        {
            if(conditionData.Instance.get(id).deleteMethod==1)
            {
                //Debug.Log("检测1 : "+ conditionData.Instance.get(id).deleteAttr +" = "+ unit.iGet(conditionData.Instance.get(id).deleteAttr) + " : "+ conditionData.Instance.get(id).deleteValue);
                if(unit.iGet(conditionData.Instance.get(id).deleteAttr)>= conditionData.Instance.get(id).deleteValue)
                {
                    if(conditionData.Instance.get(id).deleteTime>=0)
                    {
                        if (deleteTick > 0)
                        {
                            if (GTime.Instance.GTick - deleteTick >= conditionData.Instance.get(id).deleteTime)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            deleteTick = GTime.Instance.GTick;
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (deleteTick > 0)
                    {
                        deleteTick = GTime.Instance.GTick;
                        return false;
                    }
                }
            }
            else if(conditionData.Instance.get(id).deleteMethod == 2)
            {
                if (unit.iGet(conditionData.Instance.get(id).deleteAttr) <= conditionData.Instance.get(id).deleteValue)
                {
                    if (conditionData.Instance.get(id).deleteTime >= 0)
                    {
                        if (deleteTick > 0)
                        {
                            if (GTime.Instance.GTick - deleteTick >= conditionData.Instance.get(id).deleteTime)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            deleteTick = GTime.Instance.GTick;
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (deleteTick > 0)
                    {
                        deleteTick = GTime.Instance.GTick;
                        return false;
                    }
                }
            }
            else
            {
                if (GTime.Instance.GTick - deleteTick >= conditionData.Instance.get(id).deleteTime)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    public void loop()
    {
        int t = conditionData.Instance.get(id).intvalTime;
        if (t > 0)
        {
            tick++;
            if (tick >= t)
            {
                tick = 0;
                processIntaval();
            }
        }

        t = conditionData.Instance.get(id).delay;
        if(t>=0)
        {
            if(GTime.Instance.GTick - startTime > t)
            {
                processDelay();
            }
        }
    }

    void processDelay()
    {
        if(delayFirst)
        {
            switch(conditionData.Instance.condi[id].delay_act)
            {
                case 1:
                    {
                        down = true;
                        break;
                    }
                default:
                    break;
            }
            delayFirst = false;
        }

        int t = conditionData.Instance.get(id).delay_intval;
        if(t>0)
        {
            delayTick++;
            if (delayTick >= t)
            {
                delayTick = 0;

                switch(conditionData.Instance.get(id).delay_attr)
                {
                    case UIA.hp:
                        {
                            unit.hp += conditionData.Instance.get(id).delay_value;
                            break;
                        }
                    default:
                        break;
                }
            }
        }
    }

    void processIntaval()
    {
        int maxMood = conditionData.Instance.get(id).intvalMaxMood;
        if(Math.Abs(intvalData)< Math.Abs(maxMood))
        {
            intvalData += conditionData.Instance.get(id).intvalMood;
        }
    }

    public void onCover(int id)
    {

        if (conditionData.Instance.get(id).buff)
        {
            coverMood += unit.buff[id].coverMood;
        }
        else
        {
            coverMood += unit.debuff[id].coverMood;
        }

    }
}
