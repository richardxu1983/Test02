using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour {

    System.Timers.Timer t = new System.Timers.Timer(Globals.GAME_LOOP_INTERVAL);

    // Use this for initialization
    void Start () {

        GlobalControl.Instance.onEnterScene();

        Application.targetFrameRate = 60;
        t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//到达时间的时候执行事件；
        t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
        t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
    }

    public void OnDestroy()
    {
        t.Enabled = false;

        GlobalControl.Instance.onLeaveScene();
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
        if(!GlobalControl.Instance.bLogicPause)
        {
            GTime.Instance.tick();
            unitPool.Instance.loop();
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
            unitPool.Instance.debug_AtkSelectUnit();
        }
        //
        if (Input.GetMouseButtonDown(1))
        {
            unitPool.Instance.onRightClick();
        }

        if(Input.GetMouseButtonDown(0))
        {
            unitPool.Instance.onLeftClick();
        }
    }
}
