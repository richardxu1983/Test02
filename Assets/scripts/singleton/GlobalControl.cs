using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using PlatForm.Utilities;

public class GlobalControl : UnitySingleton<GlobalControl>
{
    public bool enableDebug = false;
    public bool showUnitName = true;
    public bool newGame = true;
    public string loadFile;

    public void GameInit()
    {
        SpManager.Instance.init();
        utils.Instance.init();
        XMLLoader.Instance.init();
        XMLLoader.Instance.load();
        unitPool.Instance.init();
        SerializeHelper.init();
    }

    public void onEnterScene()
    {
        if(newGame)
        {
            newGameInit();
        }
        else
        {
            loadGame();
        }
    }

    public void onLeaveScene()
    {
        Debug.Log("onLeaveScene");
        GSceneMap.Instance.Clear();
    }

    public void newGameInit()
    {
        Debug.Log("newGameInit");
        GTime.Instance.init();
        GSceneMap.Instance.CreateMap();
        PathFind.Instance.init();
    }

    public void loadGame()
    {
        Debug.Log("loadGame");
        //time
        GTime.Instance.init();
        GSceneMap.Instance.CreateMap(200);
        GSceneMap.Instance.CreateTerrain();
        GSceneMap.Instance.spawnAll();
        PathFind.Instance.init();
    }

    public void saveToFile()
    {
        Debug.Log("开始存档");
    }

    public void ToggleDebug()
    {
        enableDebug = enableDebug ? false : true;
        //Debug.Log(enableDebug);
    }

}
