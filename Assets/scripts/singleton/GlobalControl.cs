using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : UnitySingleton<GlobalControl>
{
    public bool enableDebug = false;

    public void GameInit()
    {
        utils.Instance.init();
        XMLLoader.Instance.init();
        XMLLoader.Instance.load();
    }

    public void ToggleDebug()
    {
        enableDebug = enableDebug ? false : true;
        //Debug.Log(enableDebug);
    }

}
