using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : UnitySingleton<GlobalControl>
{
    public bool enableDebug = false;
    public bool showUnitName = true;

    public void GameInit()
    {
        SpManager.Instance.init();
        utils.Instance.init();
        XMLLoader.Instance.init();
        XMLLoader.Instance.load();
        GTime.Instance.init();
        unitPool.Instance.init();
    }

    public void ToggleDebug()
    {
        enableDebug = enableDebug ? false : true;
        //Debug.Log(enableDebug);
    }

}
