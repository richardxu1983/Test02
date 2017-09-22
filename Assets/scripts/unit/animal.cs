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
        id = uid;
        headSkin = XMLLoader.Instance.animalConfig[typeId].headId;
        skinColor = utils.Instance.getSkinColor(skinColorId);
        bodySkin = XMLLoader.Instance.animalConfig[typeId].bodyId;
        name = XMLLoader.Instance.animalConfig[typeId].name;
        runSpeed = XMLLoader.Instance.animalConfig[typeId].speed;
        iSet(UIA.hp, 100);
        hpMax = 100;
    }
}
