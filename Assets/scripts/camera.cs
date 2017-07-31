using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour {

    public GameManager manager;
    private Vector3 pos;
    private Material mat;



    // Use this for initialization
    void Start () {

    }

    void OnPostRender()
    {
        if(manager.currentSelHuman>=0)
        {
            pos = manager.getSelectPos();
            CreateLineMaterial();
            mat.SetPass(0);
            GL.PushMatrix();
            // Set transformation matrix for drawing to
            // match our transform
            GL.MultMatrix(transform.localToWorldMatrix);
            // Draw lines
            GL.Begin(GL.LINES);
            GL.Color(Color.green);
            //GL.LoadOrtho();
            //Debug.Log(pos);
            DrawLine((pos.x - 1f), (pos.z - 1.2f), 3, (pos.x + 1f), (pos.z - 1.2f), 3);
            DrawLine((pos.x + 1f), (pos.z - 1.2f), 3, (pos.x + 1f), (pos.z + 2.2f), 3);
            DrawLine((pos.x + 1f), (pos.z + 2.2f), 3, (pos.x - 1f), (pos.z + 2.2f), 3);
            DrawLine((pos.x - 1f), (pos.z + 2.2f), 3, (pos.x - 1f), (pos.z - 1.2f), 3);
            GL.End();
            GL.PopMatrix();
        }
    }

    void DrawLine(float x1, float y1, float z1, float x2, float y2, float z2)
    {
        //绘制线段  
        GL.Vertex(new Vector3(x1, y1, z1));
        GL.Vertex(new Vector3(x2, y2, z2));
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
		
	}
}
