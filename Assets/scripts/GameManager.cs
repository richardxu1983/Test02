using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

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
    private int lastSize = 0;
    private unitManager[] m_units;
    System.Timers.Timer t = new System.Timers.Timer(250);

    // Use this for initialization
    void Start () {

        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;
        SpManager.Instance.init();

        m_units = new unitManager[100];

        for (int i = 0; i < 1; i++)
        {
            m_units[i] = new unitManager();
            m_units[i].Setup(1, 2, 1, 1);
            m_units[i].spawnAt(new Vector3(0, 1, 0));
            m_units[i].ai().wander(true);
        }

        //t = new System.Timers.Timer(250);//实例化Timer类，设置间隔时间
        t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//到达时间的时候执行事件；
        t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
        t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
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
        for (int i = 0; i < 1; i++)
        {
            m_units[i].ai().loop();
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

    // Update is called once per frame
    void Update () {
        /*
        if (lastSize != Screen.height)
            UpdateOrthoSize();
        
        //test skin change
        if (Input.GetKeyDown("space"))
        {
            if (m_units[0].bodySkin() == 2)
            {
                m_units[0].setBodySkin(1);
            }
            else
            {
                m_units[0].setBodySkin(2);
            }
        }
        */
        //
        if (Input.GetMouseButtonDown(1))
        {
            m_units[0].ai().moveTo(Camera.main.ScreenToWorldPoint(Input.mousePosition), true);
        }
    }
}
