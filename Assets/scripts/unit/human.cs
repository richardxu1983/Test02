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
        Debug.Log("data.energyReg=" + data.energyReg);
        iSet(UIA.energyReg, data.energyReg);
        iSet(UIA.energyDec, data.energyDecNight);
        iSet(UIA.energyRegSec, data.energyRegSec);
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

    public override void SelfAttrLoop()
    {
        bool isDay = GTime.Instance.IsDay();
        if (isDay)
        {
            //如果是白天
            if (ai.ai != AI.sleep)
            {
                //极度疲倦
                if (hasBuff(6))
                {
                    Debug.Log("睡觉把!!");
                    ai.ai = AI.sleep;
                }
                else
                {
                    if(ai.op==OP.sleep)
                        ai.op = OP.idle;
                }
            }
            else
            {
                if (energy >= iGet(UIA.tired_slight))
                {
                    Debug.Log("起床吧！");
                    ai.ai = AI.die;
                    ai.op = OP.idle;
                }
            }
        }
        else
        {
            //如果是晚上
            if (ai.ai != AI.sleep)
            {
                //有一点累就睡觉
                if (hasBuff(4) || hasBuff(5) || hasBuff(6))
                {
                    Debug.Log("睡觉把!!");
                    ai.ai = AI.sleep;
                }
                else
                {
                    if (ai.op == OP.sleep)
                        ai.op = OP.idle;
                }
            }
            else
            {
                if(energy >= iGet(UIA.tired_slight))
                {
                    Debug.Log("起床吧！");
                    ai.ai = AI.die;
                    if (ai.op == OP.sleep)
                        ai.op = OP.idle;
                }
            }
        }
    }

    public override void SelfAiLoop()
    {
        if (ai.cmdMode())
            return;

        if(ai.ai==AI.sleep)
        {
            if(ai.op!=OP.sleep)
            {
                
                ai.op = OP.sleep;
            }
        }
    }
}
