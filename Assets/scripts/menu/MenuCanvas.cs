using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MenuCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //获取按钮游戏对象
        GameObject btnObj = GameObject.Find("Canvas/btnEnterGame");
        //获取按钮脚本组件
        Button btn = btnObj.GetComponent<Button>();
        //添加点击侦听
        btn.onClick.AddListener(delegate () {
            onClickEnterGame(btnObj);
        });

        btnObj = GameObject.Find("Canvas/btnLoadGame");
        btn = btnObj.GetComponent<Button>();
        btn.onClick.AddListener(delegate () {
            onClickLoadGame(btnObj);
        });

        GlobalControl.Instance.GameInit();
    }

    void onClickEnterGame(GameObject obj)
    {
        //Debug.Log("click: " + obj.name);
        //Application.LoadLevel("Scene_2");
        GlobalControl.Instance.newGame = true;
        SceneManager.LoadScene("main");
    }

    void onClickLoadGame(GameObject obj)
    {
        GlobalControl.Instance.newGame = false;
        SceneManager.LoadScene("main");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
