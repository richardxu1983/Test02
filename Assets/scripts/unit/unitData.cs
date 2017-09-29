using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public enum UnitIntAttr
{
    uid,
    typeId,
    hp,
    hpMax,
    skinColor,
    headSkin,
    bodySkin,
    cloth,
    fed,
    hungry_tick,
    grid_x,
    grid_y,
}

public enum UnitFloatAttr
{
    run_speed,
    walk_speed,
}

