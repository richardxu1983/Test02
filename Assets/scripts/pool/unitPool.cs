using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using System;

[Serializable]
public class unitPool : UnitySingleton<unitPool>
{
    //public List<unitManager> units;
    public nList<unitBase> units;

    [NonSerialized]
    public int currentSelHuman = -1;

    public void init()
    {
        units = new nList<unitBase>(Globals.MAX_UNIT_NUM);
    }

    public void loop()
    {
        for(int i=0;i< units.index;i++)
        {
            if(units.get(i)!=null)
            {
                units.get(i).loop();
                if (units.get(i).toDelete == 2)
                {
                    units.removeAt(units.get(i).id);
                }
            }
        }
    }

    public void CreateRandomHuman()
    {
        human v = new human(0);
        int index = units.add(v);

        if (index >= 0)
        {
            ((human)units.get(index)).create(index);
            ((human)units.get(index)).spawn(100, 100);
            //((animal)units.get(index)).ai.wander(true);
        }
    }

    public void CreateRandomAnimal()
    {
        animal v = new animal(0);
        int index = units.add(v);

        if (index >= 0)
        {
            ((animal)units.get(index)).create(index);
            ((animal)units.get(index)).spawn(100, 100);
            //((animal)units.get(index)).ai.wander(true);
        }
    }

    public unitBase getSelectHuman()
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
            units.get(currentSelHuman).ai.TryToMoveToVector3(v, true);
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
                currentSelHuman = hit.collider.gameObject.GetComponent<unitMovement>().manager().id;
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

    public void onRightClick()
    {
        if (currentSelHuman >= 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "unit")
                {
                    int i = hit.collider.gameObject.GetComponent<unitMovement>().manager().id;
                    units.get(currentSelHuman).ai.TryMoveToTarget(units.get(i), true);
                }
                else
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        MoveSelectToWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    }
                }
            }
            else
            {
                MoveSelectToWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }

    public void ClearAll()
    {
        for (int i = 0; i < units.index; i++)
        {
            if (units.get(i) != null)
            {
                if (units.get(i).toDelete == 0)
                {
                    units.get(i).toDelete = 1;
                }
            }
        }
    }

    public void spawnAll()
    {
        for (int i = 0; i < units.index; i++)
        {
            if (units.get(i) != null)
            {
                units.get(i).spawn();
            }
        }
    }
}

