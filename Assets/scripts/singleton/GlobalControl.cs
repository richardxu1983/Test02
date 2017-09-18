using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

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

        FileStream fs = new FileStream(Application.persistentDataPath + "/saveData.dat", FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();

            // Deserialize the hashtable from the file and 
            // assign the reference to the local variable.
            //map
            GSceneMap.Instance.gridNum = (int)formatter.Deserialize(fs);
        }
        catch (SerializationException e)
        {
            Debug.Log("Failed to deserialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }

        GSceneMap.Instance.CreateTerrain();
        GSceneMap.Instance.spawnAll();
        PathFind.Instance.init();
    }

    public void saveToFile()
    {
        Debug.Log("开始存档");

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/saveData.dat", FileMode.Create);

        SurrogateSelector ss = new SurrogateSelector();
        Assets.Editor.Vector3SerializationSurrogate v3Surrogate = new Assets.Editor.Vector3SerializationSurrogate();
        ss.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), v3Surrogate);
        formatter.SurrogateSelector = ss;

        try
        {
            formatter.Serialize(stream, GSceneMap.Instance.gridNum);
        }
        catch (SerializationException e)
        {
            Debug.Log("Failed to serialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            stream.Close();
        }

        Debug.Log("存档结束");
    }

    public void ToggleDebug()
    {
        enableDebug = enableDebug ? false : true;
        //Debug.Log(enableDebug);
    }

}
