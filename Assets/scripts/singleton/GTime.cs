﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class timeData : Singleton<timeData>
{
    public int TIME_IN_TICK;     //
    public int MIN_IN_TICK;     //多少tick是1分钟
    public int HOUR_IN_MINUTE;     //多少游戏分钟是游戏1小时
    public int MONTH_IN_DAY;      //一个月多少天
    public int DAY_IN_HOUR;      //一天几小时
    public int YEAR_IN_MONTH;    //一年几个月
    public int INIT_HOUR;          //初始的小时
    public int INIT_YEAR;          //初始的年
    public int NIGHT_HOUR;        //晚上开始于几点
    public int DAY_HOUR;          //白天开始于几点
    public float DAY_LIGHT;          
    public float NIGHT_LIGHT;          
}

[Serializable]
public class GTime : Singleton<GTime>
{
    private int m_minInTick;             //多少个逻辑帧过游戏里的1分钟
    private int m_hourInMinuts = 60;     //多少游戏分钟是游戏1小时
    private int m_monthInDays = 15;      //一个月多少天
    private int m_dayInHours = 24;      //一天24小时
    private int m_yearInMonths = 12;    //一年12个月
    private int m_MinutsInDay;
    private int m_MinuteNowInDay;
    private int m_year;
    private int m_minute;
    private int m_hour;
    private int m_day;
    private int m_month;
    private int m_tick;
    private int m_daysInYear;
    private int m_totalDaysInYear;
    private int m_cur_tick;
    private int m_GTick;
    public int m_dayHour;
    public int m_nightHour;

    public delegate void NotifyMin();
    public event NotifyMin MinNotifier;

    public delegate void NotifyDay();
    public event NotifyDay DayNotifier;

    //
    public void init()
    {
        m_minInTick = timeData.Instance.MIN_IN_TICK;
        m_hourInMinuts = timeData.Instance.HOUR_IN_MINUTE;
        m_monthInDays = timeData.Instance.MONTH_IN_DAY;
        m_yearInMonths = timeData.Instance.YEAR_IN_MONTH;
        m_dayInHours = timeData.Instance.DAY_IN_HOUR;
        m_minute = 0;
        m_tick = 0;
        m_day = 1;
        m_month = 1;
        m_cur_tick = 0;
        m_GTick = 0;
        m_hour = timeData.Instance.INIT_HOUR;
        m_year = timeData.Instance.INIT_YEAR;
        m_totalDaysInYear = m_monthInDays * m_yearInMonths;
        m_MinutsInDay = m_dayInHours * m_hourInMinuts;
        m_dayHour = timeData.Instance.DAY_HOUR;
        m_nightHour = timeData.Instance.NIGHT_HOUR;
    }

    public void tick()
    {
        m_cur_tick++;
        if(m_cur_tick>= timeData.Instance.TIME_IN_TICK)
        {
            StepTickTime();
            m_cur_tick = 0;
        }
    }

    //
    public void SetTime(int initHour, int initMonth, int initDay)
    {
        m_minute = 0;
        m_hour = initHour;
        m_month = initMonth;
        m_day = initDay;
        m_day = m_day < 1 ? 1 : m_day;
        m_day = m_day > m_monthInDays ? m_monthInDays : m_day;
        m_MinuteNowInDay = m_minute + m_hour * m_hourInMinuts;
        m_daysInYear = m_month * m_monthInDays + m_day;
    }

    public bool IsDay()
    {
        if(m_hour >= m_dayHour && m_hour< m_nightHour)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string dayString()
    {
        if(IsDay())
        {
            return "白天";
        }
        else
        {
            return "夜晚";
        }
    }

    public int monthInDay()
    {
        return m_monthInDays;
    }

    //
    public int CurrentDayInYear()
    {
        return m_daysInYear;
    }

    //
    public int TotalDaysInYear()
    {
        return m_totalDaysInYear;
    }

    //
    public int MinutsInDay()
    {
        return m_MinutsInDay;
    }

    //
    public float MinutNow()
    {
        return m_MinuteNowInDay;
    }

    //
    public int GTick
    {
        get { return m_GTick; }
    }

    //Step Tick Time
    public void StepTickTime()
    {
        m_tick++;
        m_GTick++;
        if (m_tick >= m_minInTick)
        {
            m_tick = 0;
            m_minute++;

            if (m_minute >= m_hourInMinuts)
            {
                m_minute = 0;
                m_hour++;
                onStepHour();
                if (m_hour >= m_dayInHours)
                {
                    m_hour = 0;
                    m_day++;
                    if (m_day >= m_monthInDays)
                    {
                        m_day = 1;
                        m_month++;
                        if (m_month >= m_yearInMonths)
                        {
                            m_month = 1;
                            m_year++;
                        }
                    }
                    m_daysInYear = m_month * m_monthInDays + m_day;
                    //DayNotifier.Invoke();
                }
            }
            m_MinuteNowInDay = m_minute + m_hour * m_hourInMinuts;
            //MinNotifier.Invoke();
        }
    }

    public string TimeString()
    {
        return ""+m_year + "年" + " " + m_month + "月"+" , "+ "第" + " " + m_day + "天"+" , " + m_hour.ToString() + "点" + " [ " +dayString()+" ] "+"";
    }

    public void onStepHour()
    {

    }

    //
    public int Year()
    {
        return m_year;
    }

    public int Month()
    {
        return m_month;
    }

    public int Day()
    {
        return m_day;
    }

    public int Hour()
    {
        return m_hour;
    }

    public int Minute()
    {
        return m_minute;
    }

    //
    public void DebugSetHour(int hour)
    {
        m_hour = hour;
    }

    //
    public void DebugAddHour(int hour)
    {
        m_hour += hour;
        if (m_hour >= 24)
        {
            m_hour = 0;
        }
    }
}
