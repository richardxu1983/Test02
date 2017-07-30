using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainCancas : MonoBehaviour {

    public GameManager manager;

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
    }

    void onClickQuitGame(GameObject obj)
    {
        Debug.Log("click: " + obj.name);
        //Application.LoadLevel("Scene_2");
        SceneManager.LoadScene("menu");
    }

    void onClickCreateHuman(GameObject obj)
    {
        Debug.Log("click: " + obj.name);
        //Application.LoadLevel("Scene_2");
        manager.CreateRandomHuman();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
