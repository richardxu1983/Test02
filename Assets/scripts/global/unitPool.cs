using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class unitPool : UnitySingleton<unitPool>
{
    //public List<unitManager> units;
    public int currentSelHuman = -1;
    public nList<unitManager> units;

    public void init()
    {
        units = new nList<unitManager>(Globals.MAX_UNIT_NUM);
    }

    public void loop()
    {
        for(int i=0;i< units.index;i++)
        {
            if(units.get(i)!=null)
            {
                units.get(i).loop();
                if (units.get(i).ToDelete == 2)
                {
                    units.removeAt(units.get(i).id());
                }
            }
        }
    }

    public int tryCreate()
    {
        unitManager v = new unitManager();
        return units.add(v);
    }

    public void CreateRandomHuman()
    {
        int index = tryCreate();
        if (index >= 0)
        {
            units.get(index).CreateHuman(index, 2);
            units.get(index).spawnAt(100,100);
            units.get(index).ai().wander(true);
        }
    }

    public void CreateRandomAnimal()
    {
        int index = tryCreate();
        if (index >= 0)
        {
            units.get(index).CreateAnimalById(index, 0);
            units.get(index).spawnAt(100, 100);
            units.get(index).ai().wander(true);
        }
    }

    public unitManager getSelectHuman()
    {
        if (currentSelHuman >= 0)
        {
            return units.get(currentSelHuman);
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
            return units.get(currentSelHuman).m_Instance.transform.position;
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
            return units.get(currentSelHuman).m_Instance;
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
            units.get(currentSelHuman).onFreeSelect();
        }
        currentSelHuman = -1;
    }

    public void debug_AtkSelectUnit()
    {
        if (currentSelHuman >= 0)
        {
            units.get(currentSelHuman).hpAdd(-10);
        }
    }

    public void MoveSelectToWorldPos(Vector3 v)
    {
        //Debug.Log(v);
        //Debug.Log(GSceneMap.Instance.nodeFromWorldPoint(v).gridId.x+" , "+ GSceneMap.Instance.nodeFromWorldPoint(v).gridId.x);
        if (currentSelHuman >= 0)
        {
            units.get(currentSelHuman).ai().moveTo(GSceneMap.Instance.nodeFromWorldPoint(v).gridId, true);
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
                units.get(currentSelHuman).bDebugInfo = true;
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
        for (int i = 0; i < units.index; i++)
        {
            if (units.get(i) != null)
            {
                if (units.get(i).ToDelete == 0)
                {
                    units.get(i).ToDelete = 1;
                }
            }
        }
    }
}

