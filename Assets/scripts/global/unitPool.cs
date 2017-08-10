﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class unitPool : UnitySingleton<unitPool>
{
    public List<unitManager> units;
    public int currentSelHuman = -1;

    public void init()
    {
        units = new List<unitManager>();
    }

    public void loop()
    {
        foreach (unitManager v in units)
        {
            v.loop();
            if (v.ToDelete == 2)
            {
                units.Remove(v);
                foreach (unitManager k in units)
                {
                    k.setId(units.IndexOf(k));
                }
            }
        }
    }

    public void CreateRandomHuman()
    {
        unitManager v = new unitManager();
        units.Add(v);
        int index = units.IndexOf(v);
        //Debug.Log("添加" + index + "位置角色");
        units[index].CreateHuman(index, 2);
        units[index].spawnAt(new Vector3(0, 1, 0));
        units[index].ai().wander(true);
    }

    public void CreateRandomAnimal()
    {
        unitManager v = new unitManager();
        units.Add(v);
        int index = units.IndexOf(v);
        //Debug.Log("添加" + index + "位置角色");
        units[index].CreateAnimalById(index, 0);
        units[index].spawnAt(new Vector3(0, 1, 0));
        units[index].ai().wander(true);
    }

    public unitManager getSelectHuman()
    {
        if (currentSelHuman >= 0)
        {
            return units[currentSelHuman];
        }
        else
        {
            return null;
        }
    }

    public Vector3 getSelectPos()
    {
        if (currentSelHuman >= 0)
        {
            return units[currentSelHuman].m_Instance.transform.position;
        }
        else
        {
            return new Vector3(0, 0, 0);
        }
    }

    public GameObject getSelectObj()
    {
        if (currentSelHuman >= 0)
        {
            return units[currentSelHuman].m_Instance;
        }
        else
        {
            return null;
        }
    }

    public void freeSelect()
    {
        if (currentSelHuman >= 0)
        {
            units[currentSelHuman].onFreeSelect();
        }
        currentSelHuman = -1;
    }

    public void debug_AtkSelectUnit()
    {
        if (currentSelHuman >= 0)
        {
            units[currentSelHuman].hpAdd(-10);
        }
    }

    public void MoveSelectTo(Vector3 v)
    {
        if (currentSelHuman >= 0)
        {
            units[currentSelHuman].ai().moveTo(v, true);
        }
    }

    public void onLeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "unit")
            {
                freeSelect();
                currentSelHuman = hit.collider.gameObject.GetComponent<unitMovement>().manager().id();
                units[currentSelHuman].bDebugInfo = true;
            }
            else
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    freeSelect();
                }
            }
        }
        else
        {
            freeSelect();
        }
    }

    public void ClearAll()
    {
        foreach (unitManager v in units)
        {
            if (v.ToDelete == 0)
            {
                v.ToDelete = 1;
            }
        }
    }
}

