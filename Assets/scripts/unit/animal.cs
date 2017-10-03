using System.Collections;
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

        iSet(UIA.fullMax, unitDefault.Instance.fullMax);
        iSet(UIA.fullDec, unitDefault.Instance.fullDec);
        iSet(UIA.energyMax, unitDefault.Instance.energyMax);
        iSet(UIA.energyDec, unitDefault.Instance.energyDec);

        id          = uid;
        headSkin    = XMLLoader.Instance.animalConfig[typeId].headId;
        skinColor   = utils.Instance.getSkinColor(skinColorId);
        bodySkin    = XMLLoader.Instance.animalConfig[typeId].bodyId;
        name        = XMLLoader.Instance.animalConfig[typeId].name;
        runSpeed    = XMLLoader.Instance.animalConfig[typeId].speed;
        hpMax       = unitDefault.Instance.hpMax;
        mood        = Globals.MOOD_BASE;
        hp          = 100;
        energy      = unitDefault.Instance.energyInit;
        full        = unitDefault.Instance.fullInit;
    }

    public override void SelfLoop()
    {
    }
}
