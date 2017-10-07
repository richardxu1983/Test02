using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingTool : MonoBehaviour {

    public Text txtProgress;
    public Slider sliderProgress;

    //异步对象
    AsyncOperation async;

    void Start()
    {
        //在这里开启一个异步任务，
        //进入loadScene方法。
        GlobalControl.Instance.beforeEnterScene();
        sliderProgress.value = 0;
        txtProgress.text = "0 %";
        StartCoroutine(loadScene());
    }

    //注意这里返回值一定是 IEnumerator
    IEnumerator loadScene()
    {
        int displayProgress = 0;
        int toProgress = 0;

        //异步读取场景。
        //Globe.loadName 就是A场景中需要读取的C场景名称。
        async = SceneManager.LoadSceneAsync("main");
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            toProgress = (int)async.progress * 100;
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                SetLoadingPercentage(displayProgress);
                yield return new WaitForEndOfFrame();
            }
        }

        toProgress = 100;
        while (displayProgress < toProgress)
        {
            ++displayProgress;
            SetLoadingPercentage(displayProgress);
            yield return new WaitForEndOfFrame();
        }
        async.allowSceneActivation = true;

        //读取完毕后返回， 系统会自动进入C场景
        //yield return async;

    }

    void OnGUI()
    {
        //因为在异步读取场景，
        //所以这里我们可以刷新UI
    }

    void SetLoadingPercentage(float v)
    {
        sliderProgress.value = v/100;
        txtProgress.text = v + " %";
    }

    /*
    void Update()
    {

        //在这里计算读取的进度，
        //progress 的取值范围在0.1 - 1之间， 但是它不会等于1
        //也就是说progress可能是0.9的时候就直接进入新场景了
        //所以在写进度条的时候需要注意一下。
        //为了计算百分比 所以直接乘以100即可
        progress = (int)(async.progress * 100);
        sliderProgress.value = progress;

        txtProgress.text = progress + " %";

        //有了读取进度的数值，大家可以自行制作进度条啦。
        //Debug.Log("p : " + progress);
    }
    */

}
