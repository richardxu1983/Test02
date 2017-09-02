using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treeManager
{
    public GameObject m_Instance;
    public GridID grid;
    public SpriteRenderer renderer;
    public string prefabname;
    public string spritename;

    public treeManager(int x, int y)
    {
        grid = new GridID(x, y);
        prefabname = "tree";
        spritename = "tree";
    }

    public void spawn()
    {
        Vector3 v = GSceneMap.Instance.gridToWorldPosition(grid);
        m_Instance = Object.Instantiate(Resources.Load("Prefab/" + prefabname), v, new Quaternion(0, 0, 0, 0)) as GameObject;
        renderer = m_Instance.transform.Find("img").GetComponent<SpriteRenderer>();
        renderer.sprite = SpManager.Instance.LoadSprite(spritename);
        m_Instance.transform.parent = GameObject.Find("trees").transform;
    }
}

public class treePool : UnitySingleton<treePool>
{
    public nList<treeManager> tree;


    public void init()
    {
        tree = new nList<treeManager>(40000);
    }

    public int tryCreate(int x, int y)
    {
        treeManager v = new treeManager(x, y);
        return tree.add(v);
    }

    public void spawnAll()
    {
        for (int i = 0; i < tree.index; i++)
        {
            if (tree.get(i) != null)
            {
                tree.get(i).spawn();
            }
        }
    }
}
