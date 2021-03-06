﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour {

    public GameManager manager;
    private Vector3 pos;
    private Vector3 ext;
    private Material mat;
    private Vector3 leftTop_1 = new Vector3();
    private Vector3 leftTop_2 = new Vector3();
    private Vector3 leftTop_3 = new Vector3();
    private Vector3 rightTop_1 = new Vector3();
    private Vector3 rightTop_2 = new Vector3();
    private Vector3 rightTop_3 = new Vector3();
    private Vector3 leftBottom_1 = new Vector3();
    private Vector3 leftBottom_2 = new Vector3();
    private Vector3 leftBottom_3 = new Vector3();
    private Vector3 rightBottom_1 = new Vector3();
    private Vector3 rightBottom_2 = new Vector3();
    private Vector3 rightBottom_3 = new Vector3();
    private float length = 0.5f;

    private Vector3 PreMouseMPos;
    private Vector3 PreMouseLPos;

    // Use this for initialization
    void Start () {
        if (Camera.main.orthographicSize >= Globals.NAME_SHOW_SIZE)
        {
            GlobalControl.Instance.showUnitName = false;
        }
        else
        {
            GlobalControl.Instance.showUnitName = true;
        }
    }

    void OnPostRender()
    {
        if(unitPool.Instance.currentSelHuman>=0)
        {
            if(unitPool.Instance.getSelectHuman().toDelete!=2)
            {
                //pos = manager.getSelectPos();
                pos = unitPool.Instance.getSelectHuman().m_skin.bodyRenderer.bounds.center - transform.position;
                ext = unitPool.Instance.getSelectHuman().m_skin.bodyRenderer.bounds.extents * 1.2f;
                if (unitPool.Instance.getSelectHuman().m_skin.hasHead)
                {
                    ext.z += unitPool.Instance.getSelectHuman().m_skin.headRenderer.bounds.extents.z - 0.2f;
                    pos.z += unitPool.Instance.getSelectHuman().m_skin.headRenderer.bounds.extents.z - 0.2f;
                }

                leftBottom_2.x = (pos.x - ext.x);
                leftBottom_2.y = (pos.z - ext.z);
                leftBottom_2.z = 3;

                leftBottom_1.x = (pos.x - ext.x);
                leftBottom_1.y = (pos.z - ext.z) + length;
                leftBottom_1.z = 3;

                leftBottom_3.x = (pos.x - ext.x) + length;
                leftBottom_3.y = (pos.z - ext.z);
                leftBottom_3.z = 3;


                rightBottom_2.x = (pos.x + ext.x);
                rightBottom_2.y = (pos.z - ext.z);
                rightBottom_2.z = 3;

                rightBottom_1.x = (pos.x + ext.x) - length;
                rightBottom_1.y = (pos.z - ext.z);
                rightBottom_1.z = 3;

                rightBottom_3.x = (pos.x + ext.x);
                rightBottom_3.y = (pos.z - ext.z) + length;
                rightBottom_3.z = 3;

                rightTop_2.x = (pos.x + ext.x);
                rightTop_2.y = (pos.z + ext.z);
                rightTop_2.z = 3;

                rightTop_1.x = (pos.x + ext.x);
                rightTop_1.y = (pos.z + ext.z) - length;
                rightTop_1.z = 3;

                rightTop_3.x = (pos.x + ext.x) - length;
                rightTop_3.y = (pos.z + ext.z);
                rightTop_3.z = 3;

                leftTop_2.x = (pos.x - ext.x);
                leftTop_2.y = (pos.z + ext.z);
                leftTop_2.z = 3;

                leftTop_1.x = (pos.x - ext.x) + length;
                leftTop_1.y = (pos.z + ext.z);
                leftTop_1.z = 3;

                leftTop_3.x = (pos.x - ext.x);
                leftTop_3.y = (pos.z + ext.z) - length;
                leftTop_3.z = 3;

                //Debug.Log(pos);
                //Debug.Log(ext);
                CreateLineMaterial();
                mat.SetPass(0);
                GL.PushMatrix();
                // Set transformation matrix for drawing to
                // match our transform
                GL.MultMatrix(transform.localToWorldMatrix);
                // Draw lines
                GL.Begin(GL.LINES);
                GL.Color(Color.green);
                DrawLine(leftBottom_1, leftBottom_2);
                DrawLine(leftBottom_2, leftBottom_3);
                GL.End();
                GL.Begin(GL.LINES);
                GL.Color(Color.green);
                DrawLine(rightBottom_1, rightBottom_2);
                DrawLine(rightBottom_2, rightBottom_3);
                GL.End();
                GL.Begin(GL.LINES);
                GL.Color(Color.green);
                DrawLine(rightTop_1, rightTop_2);
                DrawLine(rightTop_2, rightTop_3);
                GL.End();
                GL.Begin(GL.LINES);
                GL.Color(Color.green);
                DrawLine(leftTop_1, leftTop_2);
                DrawLine(leftTop_2, leftTop_3);
                GL.End();

                
                GL.Begin(GL.LINES);
                GL.Color(Color.white);
                if (unitPool.Instance.getSelectHuman().ai.path.Count>0)
                {
                    DrawLineNew(unitPool.Instance.getSelectHuman().pos - transform.position, unitPool.Instance.getSelectHuman().ai.path[unitPool.Instance.getSelectHuman().ai.pathIndex].worldPosition -transform.position);
                    if (unitPool.Instance.getSelectHuman().ai.path.Count - unitPool.Instance.getSelectHuman().ai.pathIndex >= 2)
                    {
                        for (int i = unitPool.Instance.getSelectHuman().ai.pathIndex; i < unitPool.Instance.getSelectHuman().ai.path.Count-1; i++)
                        {
                            DrawLineNew(unitPool.Instance.getSelectHuman().ai.path[i].worldPosition - transform.position, unitPool.Instance.getSelectHuman().ai.path[i+1].worldPosition - transform.position);
                        }
                    }
                }
                GL.End();
                
                GL.PopMatrix();
            }
        }
    }

    void DrawLineNew(Vector3 v1, Vector3 v2)
    {
        Vector3 v3 = new Vector3(v1.x,v1.z,3);
        Vector3 v4 = new Vector3(v2.x, v2.z, 3);

        //绘制线段  
        GL.Vertex(v3);
        GL.Vertex(v4);
    }

    void DrawLine(Vector3 v1, Vector3 v2)
    {
        //绘制线段  
        GL.Vertex(v1);
        GL.Vertex(v2);
    }


    void CreateLineMaterial()
    {
        if (!mat)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            mat = new Material(shader);
            mat.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            mat.SetInt("_ZWrite", 0);
        }
    }

    // Update is called once per frame
    void Update () {
        //Zoom out  
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.orthographicSize += Globals.CAMERA_STEP;
            Camera.main.orthographicSize = Camera.main.orthographicSize >= Globals.MAX_CAMERA ? Globals.MAX_CAMERA : Camera.main.orthographicSize;
            if(Camera.main.orthographicSize>= Globals.NAME_SHOW_SIZE)
            {
                GlobalControl.Instance.showUnitName = false;
            }
        }
        //Zoom in  
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Camera.main.orthographicSize -= Globals.CAMERA_STEP;
            Camera.main.orthographicSize = Camera.main.orthographicSize <= Globals.MIN_CAMERA ? Globals.MIN_CAMERA : Camera.main.orthographicSize;
            if (Camera.main.orthographicSize <= Globals.NAME_SHOW_SIZE)
            {
                GlobalControl.Instance.showUnitName = true;
            }
        }
        //鼠标中键按下，上下左右移动相机  
        if (Input.GetMouseButton(2))
        {

            if (PreMouseMPos.x <= 0)
            {
                PreMouseMPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
            }
            else
            {
                Vector3 CurMouseMPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
                Vector3 offset = CurMouseMPos - PreMouseMPos;
                offset = -offset * 0.1f;//0.1这个数字的大小可以调节速度  
                transform.Translate(offset);
                PreMouseMPos = CurMouseMPos;
            }
        }
        else
        {
            PreMouseMPos = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
}
