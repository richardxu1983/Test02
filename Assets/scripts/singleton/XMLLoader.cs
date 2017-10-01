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
    public ST_Condition[]       GCondition;     //buff表

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
    }

    public void surfaceInit()
    {
        GsurIndex   = new ST_GSurface[Globals.MAX_GSUR_NUM];
        GSHelper    = new int[Globals.MAX_GSUR_NUM];
        GSurSearch  = new ST_GSURS[Globals.MAX_GSUR_NUM];
        GCondition  = new ST_Condition[128];


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
            animalConfig[i].typeId = i;
            animalConfig[i].bodyId = GetNodeInt(node, "bodyId");
            animalConfig[i].headId = GetNodeInt(node, "headId");
            animalConfig[i].name = GetNodeStr(node, "name");
            animalConfig[i].speed = GetNodeFl(node, "speed");
            animalConfig[i].r = GetNodeFl(node, "r");
            animalConfig[i].g = GetNodeFl(node, "r");
            animalConfig[i].b = GetNodeFl(node, "r");
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
            //Debug.Log(node.GetAttribute("name"));
            GCondition[i].id = GetNodeInt(node, "id");
            GCondition[i].cover = GetNodeInt(node, "cover");
            GCondition[i].mood = GetNodeInt(node, "mood");
            GCondition[i].deleteTime = GetNodeInt(node, "deleteTime");
            GCondition[i].deleteValue = GetNodeInt(node, "deleteValue");
            GCondition[i].duration = GetNodeInt(node, "duration");
            GCondition[i].trigAction = GetNodeInt(node, "trigAction");
            GCondition[i].trigCondi = GetNodeInt(node, "trigCondi");
            GCondition[i].trigTime = GetNodeInt(node, "trigTime");
            GCondition[i].name = GetNodeStr(node, "name");
            GCondition[i].buff = GetNodeBool(node, "buff");
            GCondition[i].actionTime = GetNodeInt(node, "actionTime");
            
            switch (GetNodeStr(node, "deleteAttr"))
            {
                case "full":
                    GCondition[i].deleteAttr = UIA.full;
                    break;
                default:
                    GCondition[i].deleteAttr = UIA.full;
                    break;
            }
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
}

