using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
