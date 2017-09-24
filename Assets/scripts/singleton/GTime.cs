using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public delegate void NotifyMin();
    public event NotifyMin MinNotifier;

    public delegate void NotifyDay();
    public event NotifyDay DayNotifier;

    //
    public void init()
    {
        m_minInTick = Globals.MIN_IN_TICK;
        m_hourInMinuts = Globals.HOUR_IN_MINUTE;
        m_monthInDays = Globals.MONTH_IN_DAY;
        m_yearInMonths = Globals.YEAR_IN_MONTH;
        m_dayInHours = Globals.DAY_IN_HOUR;
        m_minute = 0;
        m_tick = 0;
        m_day = 1;
        m_month = 1;
        m_cur_tick = 0;
        m_year = Globals.INIT_YEAR;
        m_totalDaysInYear = m_monthInDays * m_yearInMonths;
        m_MinutsInDay = m_dayInHours * m_hourInMinuts;
    }

    public void tick()
    {
        m_cur_tick++;
        if(m_cur_tick>=Globals.TIME_IN_TICK)
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

    //Step Tick Time
    public void StepTickTime()
    {
        m_tick++;
        if (m_tick >= m_minInTick)
        {
            m_tick = 0;
            m_minute++;

            if (m_minute >= m_hourInMinuts)
            {
                m_minute = 0;
                m_hour++;
                if (m_hour >= m_dayInHours)
                {
                    m_hour = 0;
                    m_day++;
                    if (m_day >= m_monthInDays)
                    {
                        m_day = 0;
                        m_month++;
                        if (m_month >= m_yearInMonths)
                        {
                            m_month = 0;
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
        return m_hour.ToString() + " h , " + m_day + " day , " + m_month + " month , " + m_year + "\n";
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
