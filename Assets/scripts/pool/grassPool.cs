using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class greassManager
{
    public GameObject m_Instance;
    public GridID grid;
    public SpriteRenderer renderer;
    public string prefabname;
    public string spritename;

    public greassManager(int x,int y)
    {
        grid = new GridID(x,y);
        prefabname = "grass";
        spritename = "grass";
    }

    public void spawn()
    {
        Vector3 v = GSceneMap.Instance.gridToWorldPosition(grid);
        m_Instance = Object.Instantiate(Resources.Load("Prefab/"+ prefabname), v, new Quaternion(0, 0, 0, 0)) as GameObject;
        renderer = m_Instance.transform.Find("img").GetComponent<SpriteRenderer>();
        renderer.sprite = SpManager.Instance.LoadSprite(spritename);
        m_Instance.transform.parent = GameObject.Find("grasses").transform;
    }
}

public class grassPool : UnitySingleton<grassPool>
{
    public nList<greassManager> grass;
    private float _seedX, _seedZ;
    private float _relief = 15f;
    float xSample, zSample;

    public void init()
    {
        grass = new nList<greassManager>(40000);
    }

    public int tryCreate(int x,int y)
    {
        greassManager v = new greassManager(x,y);
        return grass.add(v);
    }

    public void spawnAll()
    {
        for (int i = 0; i < grass.index; i++)
        {
            if (grass.get(i) != null)
            {
                grass.get(i).spawn();
            }
        }
    }
}
