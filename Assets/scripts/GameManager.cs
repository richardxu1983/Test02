using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class PerfectOverride
{
    public int referenceOrthographicSize;
    public float referencePixelsPerUnit;
}

public class GameManager : MonoBehaviour {

    
    public int referenceOrthographicSize;
    public float referencePixelsPerUnit;
    public List<PerfectOverride> overrides;
    public int Multiplyer = 1;
    public int currentSelHuman = -1;
    private int lastSize = 0;
    public List<unitManager> units;
    System.Timers.Timer t = new System.Timers.Timer(250);

    // Use this for initialization
    void Start () {

        SpManager.Instance.init();
        utils.Instance.init();
        //CreateLineMaterial();

        units = new List<unitManager>();
        //CreateRandomHuman();

        t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//到达时间的时候执行事件；
        t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
        t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
    }

    public void CreateRandomHuman()
    {
        unitManager v = new unitManager();
        units.Add(v);
        int index = units.IndexOf(v);
        //Debug.Log(index);
        units[index].CreateHuman(index, 2);
        units[index].spawnAt(new Vector3(0, 1, 0));
        units[index].ai().wander(true);
    }

    public void CreateRandomAnimal()
    {
        unitManager v = new unitManager();
        units.Add(v);
        int index = units.IndexOf(v);
        //Debug.Log(index);
        units[index].CreateAnimal(index, 2);
        units[index].spawnAt(new Vector3(0, 1, 0));
        units[index].ai().wander(true);
    }

    public void OnDestroy()
    {
        t.Enabled = false;
    }

    public void OnApplicationPause(bool pause)
    {
        Debug.Log("pause : " + pause);
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
        foreach (unitManager v in units)
        {
            v.ai().loop();
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

    PerfectOverride FindOverride(int size)
    {
        return overrides.FirstOrDefault(x => x.referenceOrthographicSize == size);
    }

    void UpdateOrthoSize()
    {
        lastSize = Screen.height;

        // first find the reference orthoSize
        float refOrthoSize = (referenceOrthographicSize / referencePixelsPerUnit) * 0.5f;

        // then find the current orthoSize
        var overRide = FindOverride(lastSize);
        float ppu = overRide != null ? overRide.referencePixelsPerUnit : referencePixelsPerUnit;
        float orthoSize = (lastSize / ppu) * 0.5f;

        // the multiplier is to make sure the orthoSize is as close to the reference as possible
        float multiplier = Mathf.Max(1, Mathf.Round(orthoSize / refOrthoSize)) * Multiplyer;

        // then we rescale the orthoSize by the multipler
        orthoSize /= multiplier;

        // set it
        Camera.main.orthographicSize = orthoSize;
        //this.GetComponent<Camera>().orthographicSize = orthoSize;

        Debug.Log(lastSize + " " + orthoSize + " " + multiplier + " " + ppu);
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


    // Update is called once per frame
    void Update () {
        /*
        if (lastSize != Screen.height)
            UpdateOrthoSize();
        */
        if(Input.GetKeyDown("space"))
        {
            GlobalControl.Instance.ToggleDebug();
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
                    currentSelHuman = hit.collider.gameObject.GetComponent<unitMovement>().manager().id();
                }
                else
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                        currentSelHuman = -1;
                }
                
                //Debug.Log("currentSelHuman : "+ currentSelHuman);
            }
            else
            {
                currentSelHuman = -1;
            }
        }
    }
}
