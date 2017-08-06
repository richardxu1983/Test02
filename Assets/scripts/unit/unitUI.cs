using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class unitUI : MonoBehaviour {

    public Text txtUnitName;
    private Image bottomPanel;
    private GameObject bPanel;
    public Text textInfo;
    private unitManager m_manager = new unitManager();
    public SpriteRenderer bodyRenderer;

    public void init(unitManager v)
    {
        m_manager = v;
    }

    // Use this for initialization
    void Start () {
        bodyRenderer = transform.Find("body").GetComponent<SpriteRenderer>();
        bottomPanel = transform.Find("Canvas/bottomPanel").GetComponent<Image>();
        bPanel = transform.Find("Canvas/bottomPanel").gameObject;
        txtUnitName.text = m_manager.name();
        bottomPanel.rectTransform.sizeDelta = new Vector2(txtUnitName.preferredWidth+1, 1);
        setNamePos();
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(position.x - (nameSize.x / 2), position.y - nameSize.y - bloodSize.y, nameSize.x, nameSize.y), m_manager.name());
    }

    void setNamePos()
    {
        float zOffset = (bodyRenderer.sprite.pivot.y / bodyRenderer.sprite.rect.height)*-1* bodyRenderer.bounds.size.z - 0.2f;

        //得到NPC头顶在3D世界中的坐标
        //默认NPC坐标点在脚底下，所以这里加上npcHeight它模型的高度即可
        Vector3 worldPosition = new Vector3(0, 0, zOffset);
        //根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
        //Vector2 position = camera.WorldToScreenPoint(worldPosition);
        Vector3 position = transform.position + worldPosition;

        //txtUnitName.transform.position = new Vector3(0,0, bodyRenderer.bounds.size.z* zOffset);
        bPanel.transform.position = position;
    }

    private void FixedUpdate()
    {

        //Debug.Log(GlobalControl.Instance.enableDebug);
        if(m_manager.bDebugInfo)
        {
            if (GlobalControl.Instance.enableDebug == true)
            {
                if (textInfo.enabled == false)
                {
                    textInfo.enabled = true;
                }
                textInfo.text = "ai : " + m_manager.ai().ai + "\nop : " + m_manager.ai().op + "\nr : " + m_manager.ai().reason;
            }
            else
            {
                if (textInfo.enabled == true)
                {
                    textInfo.enabled = false;
                }
            }
        }
        else
        {
            textInfo.enabled = false;
        }

        setNamePos();

        if(GlobalControl.Instance.showUnitName)
        {
            //bottomPanel.enabled = true;
            bPanel.SetActive(true);
        }
        else
        {
            //bottomPanel.enabled = false;
            bPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
