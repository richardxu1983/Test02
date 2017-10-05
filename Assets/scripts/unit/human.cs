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

        unitData data = unitDefault.Instance.data;

        iSet(UIA.fullDec, data.fullDec);
        iSet(UIA.energyMax, data.energyMax);
        iSet(UIA.energyDec, data.energyDec);
        iSet(UIA.fullMax, data.fullMax);

        iSet(UIA.hungry_slight, data.hungry_slight);
        iSet(UIA.hungry_medium, data.hungry_medium);
        iSet(UIA.hungry_extream, data.hungry_extream);
        iSet(UIA.tired_slight, data.tired_slight);
        iSet(UIA.tired_medium, data.tired_medium);
        iSet(UIA.tired_extream, data.tired_extream);

        id          = uid;
        typeId      = 1;
        skinColorId = Globals.rd.Next(utils.Instance.maxColors);
        headSkin    = 1000;
        bodySkin    = 1000;
        skinColor   = utils.Instance.getSkinColor(skinColorId);
        name        = "human";
        runSpeed    = data.runSpeed;
        hpMax       = data.hpMax;
        mood        = data.mood;
        hp          = data.hpMax;
        energy      = data.energy;
        full        = data.full;
    }

    public override void SelfLoop()
    {
    }
}
