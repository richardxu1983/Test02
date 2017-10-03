using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class unitInfo : MonoBehaviour {

    public Text txtFull;
    public Text txtEnergy;
    public Text txtName;
    public Text txtCondition;
    public Text txtCondtionValue;
    public Text txtMood;
    public Slider MoodProgress;
    public Image moodFill;

    public Color color_ghuang = new Color(255 / 255.0f, 153 / 255.0f, 18 / 255.0f);
    public Color color_tianlan = new Color(135 / 255.0f, 206 / 255.0f, 235 / 255.0f);
    public Color color_qingse = new Color(127 / 255.0f, 255 / 255.0f, 212 / 255.0f);

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (unitPool.Instance.currentSelHuman >= 0)
        {
            refresh(unitPool.Instance.getSelectHuman());
        }
    }

    public void setName(string s)
    {
        txtName.text = s;
    }

    public void setFull(int s)
    {
        txtFull.text = s.ToString();
    }

    public void moodProgress(int v, unitBase unit)
    {
        int moodLevel = unit.iGet(UIA.moodLevel);
        MoodProgress.value = v;
        switch (moodLevel)
        {
            case 1:
                {
                    moodFill.color = Color.red;
                    break;
                }
            case 2:
                {
                    moodFill.color = color_ghuang;
                    break;
                }
            case 3:
                {
                    moodFill.color = color_tianlan;
                    break;
                }
            case 4:
                {
                    moodFill.color = color_qingse;
                    break;
                }
            case 5:
                {
                    moodFill.color = Color.green;
                    break;
                }
        }
    }

    public void setTxtMood(unitBase unit)
    {
        string s = "";
        if(unit.type == 4)
        {
            int moodLevel = unit.iGet(UIA.moodLevel);
            switch (moodLevel)
            {
                case 1:
                    {
                        s = "<color=red>"+"崩溃"+"</color>";
                        break;
                    }
                case 2:
                    {
                        s = "<color=#FF9912ff>" + "难受" + "</color>";
                        break;
                    }
                case 3:
                    {
                        s = "<color=#87ceebff>" + "一般" + "</color>";
                        break;
                    }
                case 4:
                    {
                        s = "<color=#00ffffff>" + "满足" + "</color>";
                        break;
                    }
                case 5:
                    {
                        s = "<color=green>" + "幸福" + "</color>";
                        break;
                    }
            }
        }
        txtMood.text = s;
    }

    public void refresh(unitBase unit)
    {
        setName(unit.name);
        setFull(unit.full);
        txtEnergy.text = unit.energy.ToString();
        moodProgress(unit.mood, unit);

        string s = "";
        string k = "";

        foreach (KeyValuePair<int, Condition> v in unit.buff)
        {
            //Console.WriteLine("姓名：{0},电影：{1}", v.Key, v.Value);
            s += "<color=green>" + XMLLoader.Instance.GCondition[v.Value.id].name + "</color>\n";
            k += "<color=green>+" + XMLLoader.Instance.GCondition[v.Value.id].mood + "</color>\n";
        }

        foreach (KeyValuePair<int, Condition> v in unit.debuff)
        {
            //Debug.Log("v.Value.id=" + v.Value.id);
            s += "<color=red>" + XMLLoader.Instance.GCondition[v.Value.id].name + "</color>\n";
            k += "<color=red>" + XMLLoader.Instance.GCondition[v.Value.id].mood + "</color>\n";
        }
        txtCondition.text = s;
        txtCondtionValue.text = k;
        setTxtMood(unit);
    }
}
