using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class XMLLoader : UnitySingleton<XMLLoader>
{
    public ST_AnimalConfig[] animalConfig;
    private int MaxAnimalConfig;

    /// <summary>  
    /// 加载xml文档  
    /// </summary>  
    /// <returns></returns>  
    public XmlDocument ReadAndLoadXml(string file)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(Application.dataPath + "/Resources/xml/" + file + ".xml");
        return doc;
    }

    public void init()
    {
        animalConfig = new ST_AnimalConfig[Globals.MAX_ANIMAL_CONFIG_NUM];
        MaxAnimalConfig = 0;
    }

    public void load()
    {
        loadAnimalConfig();
        loadSkincolorConfig();
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

    public string GetNodeStr(XmlElement node, string v)
    {
        return node.GetAttribute(v);
    }
    public float GetNodeFl(XmlElement node, string v)
    {
        return float.Parse(node.GetAttribute(v));
    }
}

