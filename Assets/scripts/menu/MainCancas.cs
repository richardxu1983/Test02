using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainCancas : MonoBehaviour {

    public GameManager manager;
    private Text txtSelectInfo;
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

        btnRegister("Canvas/btnShowDbg", delegate ()
        {
            onClickShowDebug();
        });

        debugPanel = GameObject.Find("Canvas/debugPanel");
        btnShowDebug = GameObject.Find("Canvas/btnShowDbg");
        txtSelectInfo = GameObject.Find("Canvas/infoPanel/selectInfo").GetComponent<Text>();
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

    void onClickQuitGame()
    {
        SceneManager.LoadScene("menu");
    }

    void onClickCreateHuman()
    {
        manager.CreateRandomHuman();
    }

    void onClickCreateAnimal()
    {
        manager.CreateRandomAnimal();
    }

    void onClickHideDebug()
    {
        //debugPanel.enabled = false;
        debugPanel.SetActive(false);
        btnShowDebug.SetActive(true);
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
        if (manager.currentSelHuman >= 0)
        {
            txtSelectInfo.text = manager.getSelectHuman().name()+"\nx : " + Math.Round(manager.getSelectHuman().m_Instance.transform.position.x,2)
                + ", y : " + Math.Round(manager.getSelectHuman().m_Instance.transform.position.y,2)
                + ", z : " + Math.Round(manager.getSelectHuman().m_Instance.transform.position.z,2);
        }
        else
        {
            txtSelectInfo.text = "";
        }
    }
}
