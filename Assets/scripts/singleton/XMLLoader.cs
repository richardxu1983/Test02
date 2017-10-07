using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class XMLLoader : UnitySingleton<XMLLoader>
{
    public ST_AnimalConfig[]    animalConfig;   //动物表
    public ST_GSurface[]        GsurIndex;      //地面表
    public int[]                GSHelper;
    public ST_GSURS[]           GSurSearch;

    private int MaxAnimalConfig;
    public int MaxGSurConfig;

    /// <summary>  
    /// 加载xml文档  
    /// </summary>  
    /// <returns></returns>  
    public XmlDocument ReadAndLoadXml(string file)
    {
        //XmlDocument doc = new XmlDocument();
        //doc.Load(Application.dataPath + "/Resources/xml/" + file + ".xml");
        string data = Resources.Load("xml/"+ file).ToString();
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(data);
        return doc;
    }

    public void init()
    {
        animalConfig = new ST_AnimalConfig[Globals.MAX_ANIMAL_CONFIG_NUM];
        surfaceInit();
        MaxAnimalConfig = 0;
        
    }

    public void load()
    {
        loadAnimalConfig();
        loadSkincolorConfig();
        loadGSurConfig();
        loadCondition();
        loadUnitDefault();
        loadTimeData();
    }

    public void surfaceInit()
    {
        GsurIndex   = new ST_GSurface[Globals.MAX_GSUR_NUM];
        GSHelper    = new int[Globals.MAX_GSUR_NUM];
        GSurSearch  = new ST_GSURS[Globals.MAX_GSUR_NUM];

        for (int i = 0; i < Globals.MAX_GSUR_NUM; i++)
        {
            GSurSearch[i].begin = -1;
            GSurSearch[i].end = -1;
            GSHelper[i] = 0;
        }
        MaxGSurConfig = 0;
    }

    public void loadGSurConfig()
    {
        XmlDocument xmlDoc = ReadAndLoadXml(Files.GSUR_CONFIG);
        XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("objects").ChildNodes;
        int ibegin = 0;

        foreach (XmlElement node in xmlNodeList)
        {
            //Debug.Log(node.GetAttribute("name"));
            GsurIndex[GetNodeInt(node, "id")].id = GetNodeInt(node, "id");
            GsurIndex[GetNodeInt(node, "id")].name = GetNodeStr(node, "name");
            GsurIndex[GetNodeInt(node, "id")].type = GetNodeInt(node, "type");
            GsurIndex[GetNodeInt(node, "id")].typeId = GetNodeInt(node, "typeId");
            GsurIndex[GetNodeInt(node, "id")].canPlant = GetNodeBool(node, "canPlant");
            GSHelper[GetNodeInt(node, "type")]++;
            MaxGSurConfig++;
        }

        for (int i = 0; i < Globals.MAX_GSUR_NUM; i++)
        {
            if (GSHelper[i] > 0)
            {
                GSurSearch[i].begin = ibegin;
                GSurSearch[i].end = ibegin + GSHelper[i] - 1;
                ibegin = GSurSearch[i].end + 1;
            }
        }
    }

    public void loadAnimalConfig()
    {
        XmlDocument xmlDoc = ReadAndLoadXml(Files.ANIMAL_CONFIG);
        XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("objects").ChildNodes;
        int i = 0;

        foreach (XmlElement node in xmlNodeList)
        {
            //Debug.Log(node.GetAttribute("name"));
            animalXML.Instance.get(i).typeId = i;
            animalXML.Instance.get(i).bodyId = GetNodeInt(node, "bodyId");
            animalXML.Instance.get(i).headId = GetNodeInt(node, "headId");
            animalXML.Instance.get(i).name = GetNodeStr(node, "name");

            animalXML.Instance.get(i).hp = childNodeInt(node, 0, "hp");
            animalXML.Instance.get(i).hpMax = childNodeInt(node, 0, "hpMax");
            animalXML.Instance.get(i).runSpeed = childNodeInt(node, 0, "run");

            animalXML.Instance.get(i).mood = childNodeInt(node, 1, "init");
            animalXML.Instance.get(i).moodMax = childNodeInt(node, 1, "max");
            animalXML.Instance.get(i).mood_level_1 = childNodeInt(node, 1, "level_1");
            animalXML.Instance.get(i).mood_level_2 = childNodeInt(node, 1, "level_2");
            animalXML.Instance.get(i).mood_level_3 = childNodeInt(node, 1, "level_3");
            animalXML.Instance.get(i).mood_level_4 = childNodeInt(node, 1, "level_4");
            animalXML.Instance.get(i).mood_level_5 = childNodeInt(node, 1, "level_5");

            animalXML.Instance.get(i).full = childNodeInt(node, 2, "init");
            animalXML.Instance.get(i).fullDec = childNodeInt(node, 2, "dec");
            animalXML.Instance.get(i).fullDecSec = childNodeInt(node, 2, "decSec");
            animalXML.Instance.get(i).fullMax = childNodeInt(node, 2, "max");

            animalXML.Instance.get(i).hungry_slight = childNodeInt(node, 3, "slight");
            animalXML.Instance.get(i).hungry_medium = childNodeInt(node, 3, "medium");
            animalXML.Instance.get(i).hungry_extream = childNodeInt(node, 3, "extream");

            animalXML.Instance.get(i).energy = childNodeInt(node, 4, "init");
            animalXML.Instance.get(i).energyDec = childNodeInt(node, 4, "dec");
            animalXML.Instance.get(i).energyDecSec = childNodeInt(node, 4, "decSec");
            animalXML.Instance.get(i).energyMax = childNodeInt(node, 4, "max");

            animalXML.Instance.get(i).tired_slight = childNodeInt(node, 5, "slight");
            animalXML.Instance.get(i).tired_medium = childNodeInt(node, 5, "medium");
            animalXML.Instance.get(i).tired_extream = childNodeInt(node, 5, "extream");

            MaxAnimalConfig++;
            i++;
        }
    }

    public void loadCondition()
    {
        XmlDocument xmlDoc = ReadAndLoadXml(Files.CONDITION_CONFIG);
        XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("objects").ChildNodes;
        int i = 0;

        foreach (XmlElement node in xmlNodeList)
        {
            conditionData.Instance.condi[i].id = GetNodeInt(node, "id");
            conditionData.Instance.condi[i].name = GetNodeStr(node, "name");
            conditionData.Instance.condi[i].cover = GetNodeInt(node, "cover");
            conditionData.Instance.condi[i].buff = GetNodeBool(node, "buff");
            conditionData.Instance.condi[i].mood = GetNodeInt(node, "mood");
            conditionData.Instance.condi[i].duration = GetNodeInt(node, "duration");


            switch (childNodeStr(node, 0, "attr"))
            {
                case "full":
                    conditionData.Instance.condi[i].deleteAttr = UIA.full;
                    break;
                case "energy":
                    conditionData.Instance.condi[i].deleteAttr = UIA.energy;
                    break;
                default:
                    conditionData.Instance.condi[i].deleteAttr = UIA.full;
                    break;
            }

            conditionData.Instance.condi[i].deleteTime = childNodeInt(node, 0, "value");
            conditionData.Instance.condi[i].deleteValue = childNodeInt(node, 0, "time");
            conditionData.Instance.condi[i].trigCondi = childNodeInt(node, 1, "cond");
            conditionData.Instance.condi[i].trigTime = childNodeInt(node, 1, "time");
            conditionData.Instance.condi[i].actionTime = childNodeInt(node, 2, "time");
            conditionData.Instance.condi[i].trigAction = childNodeInt(node, 2, "act");
            conditionData.Instance.condi[i].intvalTime = childNodeInt(node, 3, "time");
            conditionData.Instance.condi[i].intvalMood = childNodeInt(node, 3, "mood");
            conditionData.Instance.condi[i].intvalMaxMood = childNodeInt(node, 3, "maxMood");

            i++;
        }
    }

    public void loadSkincolorConfig()
    {
        XmlDocument xmlDoc = ReadAndLoadXml(Files.SKIN_COLOR_CONFIG);
        XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("objects").ChildNodes;
        int i = 0;
        foreach (XmlElement node in xmlNodeList)
        {
            //Debug.Log(node.GetAttribute("name"));
            utils.Instance.SkinColors[i].r = GetNodeFl(node, "r") / 255.0f;
            utils.Instance.SkinColors[i].g = GetNodeFl(node, "g") / 255.0f;
            utils.Instance.SkinColors[i].b = GetNodeFl(node, "b") / 255.0f;
            utils.Instance.SkinColors[i].a = 1;
            i++;
        }
        utils.Instance.maxColors = i;
    }

    public void loadUnitDefault()
    {
        XmlDocument xmlDoc = ReadAndLoadXml("unitDefault");
        XmlNodeList node = xmlDoc.SelectSingleNode("objects").ChildNodes;

        unitDefault.Instance.data.hp = childNodeInt(node, 0, "hp");
        unitDefault.Instance.data.hpMax = childNodeInt(node, 0, "hpMax");
        unitDefault.Instance.data.runSpeed = childNodeInt(node, 0, "run");

        unitDefault.Instance.data.mood = childNodeInt(node, 1, "init");
        unitDefault.Instance.data.moodMax = childNodeInt(node, 1, "max");
        unitDefault.Instance.data.mood_level_1 = childNodeInt(node, 1, "level_1");
        unitDefault.Instance.data.mood_level_2 = childNodeInt(node, 1, "level_2");
        unitDefault.Instance.data.mood_level_3 = childNodeInt(node, 1, "level_3");
        unitDefault.Instance.data.mood_level_4 = childNodeInt(node, 1, "level_4");
        unitDefault.Instance.data.mood_level_5 = childNodeInt(node, 1, "level_5");

        unitDefault.Instance.data.full = childNodeInt(node, 2, "init");
        unitDefault.Instance.data.fullDec = childNodeInt(node, 2, "dec");
        unitDefault.Instance.data.fullDecSec = childNodeInt(node, 2, "decSec");
        unitDefault.Instance.data.fullMax = childNodeInt(node, 2, "max");

        unitDefault.Instance.data.hungry_slight = childNodeInt(node, 3, "slight");
        unitDefault.Instance.data.hungry_medium = childNodeInt(node, 3, "medium");
        unitDefault.Instance.data.hungry_extream = childNodeInt(node, 3, "extream");

        unitDefault.Instance.data.energy = childNodeInt(node, 4, "init");
        unitDefault.Instance.data.energyDec = childNodeInt(node, 4, "dec");
        unitDefault.Instance.data.energyDecSec = childNodeInt(node, 4, "decSec");
        unitDefault.Instance.data.energyMax = childNodeInt(node, 4, "max");

        unitDefault.Instance.data.tired_slight = childNodeInt(node, 5, "slight");
        unitDefault.Instance.data.tired_medium = childNodeInt(node, 5, "medium");
        unitDefault.Instance.data.tired_extream = childNodeInt(node, 5, "extream");

    }

    public void loadTimeData()
    {
        XmlDocument xmlDoc = ReadAndLoadXml("timeData");
        XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("objects").ChildNodes;

        timeData.Instance.TIME_IN_TICK = int.Parse(xmlNodeList.Item(0).InnerText);     //
        timeData.Instance.MIN_IN_TICK = int.Parse(xmlNodeList.Item(1).InnerText);    //多少tick是1分钟
        timeData.Instance.HOUR_IN_MINUTE = int.Parse(xmlNodeList.Item(2).InnerText);     //多少游戏分钟是游戏1小时
        timeData.Instance.MONTH_IN_DAY = int.Parse(xmlNodeList.Item(3).InnerText);     //一个月多少天
        timeData.Instance.DAY_IN_HOUR = int.Parse(xmlNodeList.Item(4).InnerText);      //一天几小时
        timeData.Instance.YEAR_IN_MONTH = int.Parse(xmlNodeList.Item(5).InnerText);    //一年几个月
        timeData.Instance.INIT_HOUR = int.Parse(xmlNodeList.Item(6).InnerText);          //初始的小时
        timeData.Instance.INIT_YEAR = int.Parse(xmlNodeList.Item(7).InnerText);          //初始的年
        timeData.Instance.NIGHT_HOUR = int.Parse(xmlNodeList.Item(8).InnerText);
        timeData.Instance.DAY_HOUR = int.Parse(xmlNodeList.Item(9).InnerText);
        timeData.Instance.DAY_LIGHT = float.Parse(xmlNodeList.Item(10).InnerText);
        timeData.Instance.NIGHT_LIGHT = float.Parse(xmlNodeList.Item(11).InnerText);
    }

    public int GetNodeInt(XmlElement node,string v)
    {
        return int.Parse(node.GetAttribute(v));
    }

    public bool GetNodeBool(XmlElement node, string v)
    {
        int a = int.Parse(node.GetAttribute(v));
        return a == 1 ? true : false;
    }

    public string GetNodeStr(XmlElement node, string v)
    {
        return node.GetAttribute(v);
    }

    public float GetNodeFl(XmlElement node, string v)
    {
        return float.Parse(node.GetAttribute(v));
    }

    public int childNodeInt(XmlElement node,int v,string attr)
    {
        return int.Parse(node.ChildNodes.Item(v).Attributes[attr].InnerText);
    }

    public string childNodeStr(XmlElement node, int v, string attr)
    {
        return node.ChildNodes.Item(v).Attributes[attr].InnerText;
    }

    public int childNodeInt(XmlNodeList node, int v, string attr)
    {
        return int.Parse(node.Item(v).Attributes[attr].InnerText);
    }

    public string childNodeStr(XmlNodeList node, int v, string attr)
    {
        return node.Item(v).Attributes[attr].InnerText;
    }
}

