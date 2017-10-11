using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlatForm.Utilities;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class MainCancas : MonoBehaviour {

    //public GameManager manager;
    private Text txtSelectInfo;
    private Text txtTimeInfo;
    private GameObject debugPanel;
    private GameObject btnShowDebug;

    // Use this for initialization
    void Start()
    {

        btnRegister("Canvas/debugPanel/Quit", delegate ()
        {
            onClickQuitGame();
        });

        btnRegister("Canvas/debugPanel/CreateHuman", delegate ()
        {
            onClickCreateHuman();
        });

        btnRegister("Canvas/debugPanel/CreateAnimal", delegate ()
        {
            onClickCreateAnimal();
        });

        btnRegister("Canvas/debugPanel/HideDebug", delegate ()
        {
            onClickHideDebug();
        });
        btnRegister("Canvas/debugPanel/ClearAll", delegate ()
        {
            onClickClearAll();
        });
        btnRegister("Canvas/btnShowDbg", delegate ()
        {
            onClickShowDebug();
        });

        btnRegister("Canvas/debugPanel/btnSaveGame", delegate ()
        {
            onClickSaveGame();
        });

        btnRegister("Canvas/debugPanel/btnTest", delegate ()
        {
            onClickTest();
        });

        debugPanel = GameObject.Find("Canvas/debugPanel");
        btnShowDebug = GameObject.Find("Canvas/btnShowDbg");
        txtSelectInfo = GameObject.Find("Canvas/infoPanel/selectInfo").GetComponent<Text>();
        txtTimeInfo = GameObject.Find("Canvas/timeInfo/txtInfo").GetComponent<Text>();
        onClickHideDebug();
    }

    void btnRegister(string btnPath,UnityEngine.Events.UnityAction action)
    {
        GameObject btnObj2 = GameObject.Find(btnPath);
        //获取按钮脚本组件
        Button btn2 = btnObj2.GetComponent<Button>();
        //Debug.Log(btn2);
        //添加点击侦听
        btn2.onClick.AddListener(action);
    }

    void onClickSaveGame()
    {
        GlobalControl.Instance.saveToFile();
    }

    void onClickQuitGame()
    {
        SceneManager.LoadScene("menu");
    }

    void onClickCreateHuman()
    {
        unitPool.Instance.CreateRandomHuman();
    }

    void onClickCreateAnimal()
    {
        unitPool.Instance.CreateRandomAnimal();
    }

    void onClickTest()
    {
        Debug.Log("test");

        for(int i=0; i<100; i=i+1)
        {
            for (int j = 0; j < 100; j = j + 1)
            {
                GSceneMap.Instance.grid[i, j].deleteTree();
            }
        }
        

        /*
        string path = Application.persistentDataPath + "/saveData.dat";

        int a = 5;
        int b = 6;
        Node m;
        int c, d;
        Node n = new Node(false, new Vector3(2, 2, 2), new GridID(2, 2));
        
        SerializeHelper.SaveStart(path);
        SerializeHelper.Save<int>(a);
        SerializeHelper.Save<int>(b);
        SerializeHelper.Save<Node>(n);
        SerializeHelper.SaveEnd();

        SerializeHelper.LoadStart(path);
        c = SerializeHelper.Load<int>();
        d = SerializeHelper.Load<int>();
        m = SerializeHelper.Load<Node>();
        SerializeHelper.LoadEnd();

        Debug.Log("c=" + c + " , d=" + d + " , m.worldPosition.x=" + m.worldPosition.x);
        */
    }

    void onClickHideDebug()
    {
        //debugPanel.enabled = false;
        debugPanel.SetActive(false);
        btnShowDebug.SetActive(true);
    }

    void onClickClearAll()
    {
        unitPool.Instance.ClearAll();
    }

    void onClickShowDebug()
    {
        //debugPanel.enabled = true;
        debugPanel.SetActive(true);
        btnShowDebug.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        txtTimeInfo.text = GTime.Instance.TimeString();
        if (unitPool.Instance.currentSelHuman >= 0)
        {
            txtSelectInfo.text = unitPool.Instance.getSelectHuman().name+"\nx : " + Math.Round(unitPool.Instance.getSelectHuman().m_Instance.transform.position.x,2)
                + ", y : " + Math.Round(unitPool.Instance.getSelectHuman().m_Instance.transform.position.y,2)
                + ", z : " + Math.Round(unitPool.Instance.getSelectHuman().m_Instance.transform.position.z,2)
                + "\n gx : " + unitPool.Instance.getSelectHuman().grid.x
                + ", gy : " + unitPool.Instance.getSelectHuman().grid.y
                +"\n tx : " + unitPool.Instance.getSelectHuman().targetGrid.x
                + ", ty : " + unitPool.Instance.getSelectHuman().targetGrid.y;
        }
        else
        {
            txtSelectInfo.text = "";
        }
    }
}
