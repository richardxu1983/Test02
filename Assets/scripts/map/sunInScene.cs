using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunInScene : MonoBehaviour {

    public Light sun;

    int hour;
    int dayHour;
    int nightHour;
    float dayLight;
    float nightLight;

    // Use this for initialization
    void Start ()
    {
        dayLight = timeData.Instance.DAY_LIGHT;
        nightLight = timeData.Instance.NIGHT_LIGHT;
 
        if (GTime.Instance.IsDay())
        {
            sun.intensity = dayLight;
        }
        else
        {
            sun.intensity = nightLight;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        hour = GTime.Instance.Hour();
        dayHour = GTime.Instance.m_dayHour;
        nightHour = GTime.Instance.m_nightHour;

        if (hour >= nightHour - 1 && sun.intensity > nightLight)
        {
            sun.intensity = Mathf.Lerp(sun.intensity, nightLight, Time.deltaTime * 0.1f);
        }
        else if (hour >= dayHour - 1 && hour < nightHour && sun.intensity < dayLight)
        {
            sun.intensity = Mathf.Lerp(sun.intensity, dayLight, Time.deltaTime * 0.1f);
        }
    }
}
