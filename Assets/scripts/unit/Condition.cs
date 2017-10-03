using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public int coverMood = 0;

    public Condition(int _id, unitBase _unit)
    {
        id = _id;
        startTime = GTime.Instance.GTick;
        unit = _unit;
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
        if(conditionData.Instance.get(id).buff)
        {
            coverMood += unit.buff[id].coverMood;
        }
        else
        {
            coverMood += unit.debuff[id].coverMood;
        }
    }
}
