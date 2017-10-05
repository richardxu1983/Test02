using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{

    public static string lodingSceneName = "loading";

    //----------max config numbers--------//
    public static int MAX_ANIMAL_CONFIG_NUM = 100;
    public static int MAX_SKINCOLOR_CONFIG_NUM = 100;

    //----------camera vars--------//
    public static float MAX_CAMERA = 40.0f;
    public static float MIN_CAMERA = 16.0f;
    public static float CAMERA_STEP = 1.1f;
    public static float NAME_SHOW_SIZE = 35.0f;

    //----------unit ui vars-------//
    public static float HP_YELLOW = 0.75f;
    public static float HP_RED = 0.35f;
    public static int UNIT_DELETE_TIME = 1;

    //----------unit ui img vars-------//
    public static float UNIT_IMG_BOTTOM = 0.35f;
    public static float UNIT_IMG_HEAD = 0.5f;

    //----------pool numbers-------//
    public static int MAX_UNIT_NUM = 500;
    public static int MAX_GSUR_NUM = 16;

    //----------map---------------------//
    public static int BASIC_MAP_SUR = 0;

    //------------------------------------//
    public static int wander_walk_dis = 0;
    public static int wander_walk_ran = 6;
    public static int wander_idle_time = 2;
    public static int wander_idle_ran = 2;
    public static int humanType = 1000;
    public static int animalType = 2000;
    public static System.Random rd = new System.Random();
    public static int MOOD_BASE = 60;
    public static int MOOD_LEVEL_1 = 11;
    public static int MOOD_LEVEL_2 = 30;
    public static int MOOD_LEVEL_3 = 60;
    public static int MOOD_LEVEL_4 = 85;
    public static int MOOD_LEVEL_5 = 100;
}

public static class Files
{
    //----------config files--------//
    public static string ANIMAL_CONFIG = "animal";
    public static string CONDITION_CONFIG = "condition";
    public static string GSUR_CONFIG = "groundSurface";
    public static string SKIN_COLOR_CONFIG = "skinColor";
}

//


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
}