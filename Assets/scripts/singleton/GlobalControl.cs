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
    public bool binit = false;
    public bool bLogicPause = true;
    public string loadFile;
    

    public void GameInit()
    {
        if(!binit)
        {
            conditionData.Instance.init();
            SpManager.Instance.init();
            utils.Instance.init();
            animalXML.Instance.init();
            unitDefault.Instance.init();
            XMLLoader.Instance.init();
            XMLLoader.Instance.load();
            unitPool.Instance.init();
            SerializeHelper.init();
            binit = true;
        }
    }

    public void onEnterScene()
    {
        bLogicPause = true;
        if (newGame)
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
        GameSceneUI.Instance.init();
        bLogicPause = false;
    }

    public void loadGame()
    {
        Debug.Log("开始读取");
        string path = Application.persistentDataPath + "/saveData.dat";
        //time
        GTime.Instance.init();

        SerializeHelper.LoadStart(path);
        GSceneMap.Instance.gridNum = SerializeHelper.Load<int>();
        GSceneMap.Instance.LoadMap();
        GSceneMap.Instance.grid = SerializeHelper.Load<Node[,]>();
        unitPool.Instance.units = SerializeHelper.Load<nList<unitBase>>();
        GTime.Instance = SerializeHelper.Load<GTime>();
        SerializeHelper.LoadEnd();
        Debug.Log("读取结束");
        GSceneMap.Instance.CreateTerrain();
        GSceneMap.Instance.spawnAll();
        unitPool.Instance.spawnAll();
        PathFind.Instance.init();
        GameSceneUI.Instance.init();
        bLogicPause = false;
    }

    public void saveToFile()
    {
        Debug.Log("开始存档");
        string path = Application.persistentDataPath + "/saveData.dat";

        SerializeHelper.SaveStart(path);
        SerializeHelper.Save<int>(GSceneMap.Instance.gridNum);
        SerializeHelper.Save<Node[,]>(GSceneMap.Instance.grid);
        SerializeHelper.Save<nList<unitBase>>(unitPool.Instance.units);
        SerializeHelper.Save<GTime>(GTime.Instance);
        SerializeHelper.SaveEnd();
        Debug.Log("存档结束");
    }

    public void ToggleDebug()
    {
        enableDebug = enableDebug ? false : true;
        //Debug.Log(enableDebug);
    }

}
