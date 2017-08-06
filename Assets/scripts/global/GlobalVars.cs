using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    //----------max config numbers--------//
    public static int MAX_ANIMAL_CONFIG_NUM = 100;
    public static int MAX_SKINCOLOR_CONFIG_NUM = 100;

    //----------camera vars--------//
    public static float MAX_CAMERA = 50.0f;
    public static float MIN_CAMERA = 20.0f;
    public static float CAMERA_STEP = 1.1f;
    public static float NAME_SHOW_SIZE = 35.0f;

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