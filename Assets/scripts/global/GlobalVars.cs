﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    //----------max config numbers--------//
    public static int MAX_ANIMAL_CONFIG_NUM = 100;
    public static int MAX_SKINCOLOR_CONFIG_NUM = 100;

    //---------game time vars----------//
    public static int GAME_LOOP_INTERVAL = 250;
    public static int TIME_IN_TICK = 4;     //多少游戏分钟是游戏1小时
    public static int MIN_IN_TICK = 5;     //多少游戏分钟是游戏1小时
    public static int HOUR_IN_MINUTE = 60;     //多少游戏分钟是游戏1小时
    public static int MONTH_IN_DAY = 15;      //一个月多少天
    public static int DAY_IN_HOUR = 24;      //一天24小时
    public static int YEAR_IN_MONTH = 12;    //一年12个月
    public static int INIT_HOUR = 3;          //初始的小时
    public static int INIT_YEAR = 1000;          //初始的小时

    //----------camera vars--------//
    public static float MAX_CAMERA = 50.0f;
    public static float MIN_CAMERA = 20.0f;
    public static float CAMERA_STEP = 1.1f;
    public static float NAME_SHOW_SIZE = 35.0f;

    //----------unit ui vars-------//
    public static float HP_YELLOW = 0.75f;
    public static float HP_RED = 0.35f;
    public static int UNIT_DELETE_TIME = 1;

    //----------unit ui img vars-------//
    public static float UNIT_IMG_BOTTOM = 0.35f;
    public static float UNIT_IMG_HEAD = 0.5f;

    //------------------------------------//
    public static int wander_walk_dis = 0;
    public static int wander_walk_ran = 6;
    public static int wander_idle_time = 2;
    public static int wander_idle_ran = 2;
    public static int humanType = 1000;
    public static int animalType = 2000;
    public static System.Random rd = new System.Random();
}

public static class Files
{
    //----------config files--------//
    public static string ANIMAL_CONFIG = "animal";
    public static string SKIN_COLOR_CONFIG = "skinColor";
}