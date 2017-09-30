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

    public Condition(int _id, unitBase _unit)
    {
        id = _id;
        startTime = GTime.Instance.GTick;
        unit = _unit;
    }

    public void loop()
    {

    }

}
