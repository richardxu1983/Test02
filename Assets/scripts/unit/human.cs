using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class human : unitBase
{
    public human(int _tid):base(4, _tid)
    {

    }

    public void create(int uid)
    {
        id = uid;
        typeId = 1;
        int _headSkin = 1000;
        int _bodySkin = 1000;
        int _skinColorId = Globals.rd.Next(utils.Instance.maxColors);
        headSkin = _headSkin;
        skinColor = utils.Instance.getSkinColor(_skinColorId);
        bodySkin = _bodySkin;
        name = "human";
        runSpeed = 4;
        iSet(UIA.hp, 100);
        hpMax = 100;
    }
}
