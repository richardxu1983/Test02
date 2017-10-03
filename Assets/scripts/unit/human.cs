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

        iSet(UIA.fullDec, unitDefault.Instance.fullDec);
        iSet(UIA.energyMax, unitDefault.Instance.energyMax);
        iSet(UIA.energyDec, unitDefault.Instance.energyDec);
        iSet(UIA.fullMax, unitDefault.Instance.fullMax);

        id          = uid;
        typeId      = 1;
        skinColorId = Globals.rd.Next(utils.Instance.maxColors);
        headSkin    = 1000;
        bodySkin    = 1000;
        skinColor   = utils.Instance.getSkinColor(skinColorId);
        name        = "human";
        runSpeed    = unitDefault.Instance.runSpeed;
        hpMax       = unitDefault.Instance.hpMax;
        mood        = Globals.MOOD_BASE;
        hp          = unitDefault.Instance.hpMax;
        energy      = unitDefault.Instance.energyInit;
        full        = unitDefault.Instance.fullInit;
    }

    public override void SelfLoop()
    {
    }
}
