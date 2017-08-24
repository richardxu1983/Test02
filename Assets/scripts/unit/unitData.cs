using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class unitData
{

    private int[] m_intattr;
    private float[] m_fattr;
    //private unitManager m_manager;
    public Vector3 pos;
    private string m_name = "name";
    public Color skinColor;
    public GridID grid;

    public unitData()
    {
        m_intattr = new int[64];
        m_fattr = new float[16];
    }

    public string name
    {
        get { return m_name; }
        set { m_name = value; }
    }

    public void init(unitManager m)
    {
        //m_manager = m;
    }

    public void     setInt(UnitIntAttr k, int v)   { m_intattr[(int)k] = v; }
    public int      getInt(UnitIntAttr k)          { return m_intattr[(int)k]; }
    public void     setF(UnitFloatAttr k, float v)   { m_fattr[(int)k] = v; }
    public float    getF(UnitFloatAttr k)            { return m_fattr[(int)k]; }


    public void loop()
    {

    }
}