﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class animal : unitBase
{

    public animal(int _tid):base(3, _tid)
    {
        
    }

    public void create(int uid)
    {
        id          = uid;
        headSkin    = XMLLoader.Instance.animalConfig[typeId].headId;
        skinColor   = utils.Instance.getSkinColor(skinColorId);
        bodySkin    = XMLLoader.Instance.animalConfig[typeId].bodyId;
        name        = XMLLoader.Instance.animalConfig[typeId].name;
        runSpeed    = XMLLoader.Instance.animalConfig[typeId].speed;
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
