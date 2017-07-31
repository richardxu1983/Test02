using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainCancas : MonoBehaviour {

    public GameManager manager;
    private Text txtSelectInfo;

    // Use this for initialization
    void Start()
    {
        //获取按钮游戏对象
        GameObject btnObj = GameObject.Find("Canvas/Quit");
        //获取按钮脚本组件
        Button btn = btnObj.GetComponent<Button>();
        //添加点击侦听
        btn.onClick.AddListener(delegate () {
            onClickQuitGame(btnObj);
        });

        //获取按钮游戏对象
        GameObject btnObj1 = GameObject.Find("Canvas/CreateHuman");
        //获取按钮脚本组件
        Button btn1 = btnObj1.GetComponent<Button>();
        Debug.Log(btn1);
        //添加点击侦听
        btn1.onClick.AddListener(delegate () {
            onClickCreateHuman(btnObj1);
        });

        txtSelectInfo = GameObject.Find("Canvas/infoPanel/selectInfo").GetComponent<Text>();


        //获取按钮游戏对象
        GameObject btnObj2 = GameObject.Find("Canvas/CreateAnimal");
        //获取按钮脚本组件
        Button btn2 = btnObj2.GetComponent<Button>();
        Debug.Log(btn2);
        //添加点击侦听
        btn2.onClick.AddListener(delegate () {
            onClickCreateAnimal(btnObj2);
        });

        txtSelectInfo = GameObject.Find("Canvas/infoPanel/selectInfo").GetComponent<Text>();
    }

    void onClickQuitGame(GameObject obj)
    {
        //Debug.Log("click: " + obj.name);
        //Application.LoadLevel("Scene_2");
        SceneManager.LoadScene("menu");
    }

    void onClickCreateHuman(GameObject obj)
    {
        //Debug.Log("click: " + obj.name);
        //Application.LoadLevel("Scene_2");
        manager.CreateRandomHuman();
    }

    void onClickCreateAnimal(GameObject obj)
    {
        manager.CreateRandomAnimal();
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
