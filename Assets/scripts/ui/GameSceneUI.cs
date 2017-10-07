using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : UnitySingleton<GameSceneUI>
{

    public GameObject unitPanel;
    public unitInfo unitInfoScript;
   

    // Use this for initialization
    public void init ()
    {
        unitPanel = GameObject.Find("Canvas/unitInfo");
        unitInfoScript = unitPanel.GetComponent<unitInfo>();
        unitPanel.SetActive(false);
    }
}
