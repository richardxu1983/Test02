﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
type :
    1:草
    2:树木
    3:动物
    4:人
*/

public class entity {

    public GridID grid { get; set; }
    public int type { get; set; }
    public int typeId { get; set; }
    public string name = "name";
    public int toDelete = 0;

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
}

public class plant : entity
{
    public GameObject m_Instance;
    public SpriteRenderer renderer;
    public string prefabname;
    public string spritename;
    public string parentname;
    public plantMo mo;

    public plant(int x, int y,int _type,int _tid):base(x,y,_type,_tid)
    {
    }

    public void spawn()
    {
        Vector3 v = GSceneMap.Instance.gridToWorldPosition(grid);
        m_Instance = Object.Instantiate(Resources.Load("Prefab/" + prefabname), v, new Quaternion(0, 0, 0, 0)) as GameObject;
        renderer = m_Instance.transform.Find("img").GetComponent<SpriteRenderer>();
        renderer.sprite = SpManager.Instance.LoadSprite(spritename);
        m_Instance.transform.parent = GameObject.Find(parentname).transform;
        renderer.sortingOrder = Mathf.RoundToInt(m_Instance.transform.position.z * -100);
        mo = m_Instance.GetComponent<plantMo>();
        mo.init(this);
    }

    public void delete()
    {
        toDelete = 1;
    }
}

public class grass: plant
{
    public grass(int x, int y) : base(x, y, 1, 1)
    {
        prefabname = "grass";
        spritename = "grass_" + Globals.rd.Next(1, 4);
        parentname = "grasses";
    }
}

public class tree : plant
{
    public tree(int x, int y) : base(x, y, 2, 1)
    {
        prefabname = "tree";
        spritename = "tree";
        parentname = "trees";
    }
}