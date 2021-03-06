﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
type :
    1:草
    2:树木
    3:动物
    4:人
*/

[Serializable]
public class entity {

    public GridID grid { get; set; }
    public int type { get; set; }
    public int typeId { get; set; }
    public string name = "name";
    public int toDelete = 0;
    public Vector3 m_pos;

    public Vector3 pos
    {
        get { return m_pos; }
        set
        {
            m_pos = value;
            grid = GSceneMap.Instance.nodeFromWorldPoint(m_pos).gridId;
            //Debug.Log(grid.x + " , " + grid.y);
        }
    }

    public entity(int _type, int _tid)
    {
        type = _type;
        typeId = _tid;
    }

    public void setGrid(int x,int y)
    {
        grid.x = x;
        grid.y = y;
    }

    public entity(int x, int y, int _type, int _tid)
    {
        grid = new GridID(x, y);
        type = _type;
        typeId = _tid;
    }

    public int disTo(entity v)
    {
        return GSceneMap.Instance.Dis(grid, v.grid);
    }

    public float v3Dis(entity v)
    {
        return (pos-v.pos).sqrMagnitude;
    }
}

[Serializable]
public class plant : entity
{
    public string prefabname;
    public string spritename;
    public string parentname;


    [NonSerialized]
    public plantMo mo;
    [NonSerialized]
    public GameObject m_Instance;
    [NonSerialized]
    public SpriteRenderer renderer;

    public plant(int x, int y,int _type,int _tid):base(x,y,_type,_tid)
    {
    }

    public void spawn()
    {
        Vector3 v = GSceneMap.Instance.gridToWorldPosition(grid);
        //Debug.Log(grid.x+" , "+ grid.y);
        //Debug.Log(v);
        m_pos = v;
        m_Instance = UnityEngine.Object.Instantiate(Resources.Load("Prefab/" + prefabname), v, new Quaternion(0, 0, 0, 0)) as GameObject;
        renderer = m_Instance.transform.Find("img").GetComponent<SpriteRenderer>();
        renderer.sprite = SpManager.Instance.LoadSprite(spritename);
        m_Instance.transform.parent = GameObject.Find(parentname).transform;
        renderer.sortingOrder = Mathf.RoundToInt(m_Instance.transform.position.z * -100);

        renderer.receiveShadows = false;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        Vector3 v3 = m_Instance.transform.position;
        v3.y = Mathf.RoundToInt(m_Instance.transform.position.z/100)+ GSceneMap.Instance.gridWorldSize.y/100;
        m_Instance.transform.position = v3;
        //mo = m_Instance.GetComponent<plantMo>();
        //mo.init(this);
    }

    public void delete()
    {
        UnityEngine.Object.Destroy(m_Instance);
    }
}

[Serializable]
public class grass: plant
{
    public grass(int x, int y) : base(x, y, 1, 1)
    {
        prefabname = "grass";
        spritename = "grass_" + Globals.rd.Next(1, 4);
        parentname = "grasses";
    }
}


public class treeData
{
    public string img;     //
    public string name;     //
    public int id;
}

public class treeXML : Singleton<treeXML>
{
    public treeData[] data;

    public void init()
    {
        data = new treeData[256];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = new treeData();
        }
    }

    public treeData get(int v)
    {
        return data[v];
    }
}

[Serializable]
public class tree : plant
{
    public tree(int x, int y,int typeid) : base(x, y, 2, typeid)
    {
        prefabname = "tree";
        spritename = treeXML.Instance.get(typeid).img;
        //Debug.Log(spritename);
        parentname = "trees";
    }
}