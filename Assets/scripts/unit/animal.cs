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
        animalData data = animalXML.Instance.get(typeId);

        iSet(UIA.fullMax, data.fullMax);
        iSet(UIA.fullDec, data.fullDec);
        iSet(UIA.energyMax, data.energyMax);
        iSet(UIA.energyDec, data.energyDec);
        iSet(UIA.hungry_slight, data.hungry_slight);
        iSet(UIA.hungry_medium, data.hungry_medium);
        iSet(UIA.hungry_extream, data.hungry_extream);
        iSet(UIA.tired_slight, data.tired_slight);
        iSet(UIA.tired_medium, data.tired_medium);
        iSet(UIA.tired_extream, data.tired_extream);

        id          = uid;
        headSkin    = data.headId;
        skinColor   = utils.Instance.getSkinColor(skinColorId);
        bodySkin    = data.bodyId;
        name        = data.name;
        runSpeed    = data.runSpeed;
        hpMax       = data.hpMax;
        mood        = data.mood;
        hp          = data.hp;
        energy      = data.energy;
        full        = data.full;
    }

    public override void SelfLoop()
    {
    }
}
