using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour {

    public int currentSelHuman = -1;
    public List<unitManager> units;
    System.Timers.Timer t = new System.Timers.Timer(Globals.GAME_LOOP_INTERVAL);

    // Use this for initialization
    void Start () {

        GlobalControl.Instance.GameInit();
        units = new List<unitManager>();
        t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//到达时间的时候执行事件；
        t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
        t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
    }

    public void CreateRandomHuman()
    {
        unitManager v = new unitManager();
        units.Add(v);
        int index = units.IndexOf(v);
        units[index].CreateHuman(index, 2);
        units[index].spawnAt(new Vector3(0, 1, 0));
        units[index].ai().wander(true);
    }

    public void CreateRandomAnimal()
    {
        unitManager v = new unitManager();
        units.Add(v);
        int index = units.IndexOf(v);
        units[index].CreateAnimalById(index, 0);
        units[index].spawnAt(new Vector3(0, 1, 0));
        units[index].ai().wander(true);
    }

    public void OnDestroy()
    {
        t.Enabled = false;
    }

    public void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            t.Enabled = false;
        }
        else
        {
            t.Enabled = true;
        }
    }

    public void theout(object source, System.Timers.ElapsedEventArgs e)
    {
        GTime.Instance.tick();

        foreach (unitManager v in units)
        {
            v.loop();
        }
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

    void freeSelect()
    {
        if(currentSelHuman>=0)
        {
            units[currentSelHuman].onFreeSelect();
        }
    }

    // Update is called once per frame
    void Update () {

        if(Input.GetKeyDown("space"))
        {
            GlobalControl.Instance.ToggleDebug();
        }
        if (Input.GetKeyDown("a"))
        {
            if (currentSelHuman >= 0)
            {
                units[currentSelHuman].hpAdd(-10);
            }
        }
        //
        if (Input.GetMouseButtonDown(1))
        {
            if(currentSelHuman>=0)
            {
                units[currentSelHuman].ai().moveTo(Camera.main.ScreenToWorldPoint(Input.mousePosition), true);
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject.tag == "unit")
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
                        currentSelHuman = -1;
                    }
                }
            }
            else
            {
                freeSelect();
                currentSelHuman = -1;
            }
        }
    }
}
