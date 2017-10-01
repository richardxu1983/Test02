using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class human : unitBase
{
    public human(int _tid):base(4, _tid)
    {

    }

    public void create(int uid)
    {
        id          = uid;
        typeId      = 1;
        skinColorId = Globals.rd.Next(utils.Instance.maxColors);
        headSkin    = 1000;
        bodySkin    = 1000;
        skinColor   = utils.Instance.getSkinColor(skinColorId);
        name        = "human";
        runSpeed    = 4;
        hpMax       = 100;
        mood        = Globals.MOOD_BASE;
        hp          = 100;
 
        iSet(UIA.full, 70);
        iSet(UIA.fullMax, 100);
        iSet(UIA.fullDec, 1);
    }

    public override void SelfLoop()
    {
    }
}
