using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitSkin : MonoBehaviour {

    private int bodySkinIdNow = 0;
    private int headSkinIdNow = 0;
    private string emotion;
    private string dir;
    public SpriteRenderer bodyRenderer;
    public SpriteRenderer headRenderer;
    public SpriteRenderer hairRenderer;
    //public SpriteRenderer shadowRenderer;
    private BoxCollider m_collider;
    private unitMovement m_umovement;
    private GameObject mainImg;
    private unitBase m_manager = new unitBase(1,1);
    public bool hasHead = true;
    private Vector3 bottomPos;
    private Vector3 headPos;
    private float imgHeight = 0;
    public int statusTag = 0;
    public STU sta = STU.stand;

    public void init(unitBase v)
    {
        m_manager = v;
        if(m_manager.headSkin<0)
        {
            hasHead = false;
            headSkinIdNow = m_manager.headSkin;
        }
    }

    // Use this for initialization
    void Start()
    {
        emotion = "normal";
        bodyRenderer = transform.Find("img/body").GetComponent<SpriteRenderer>();
        hairRenderer = transform.Find("img/hair").GetComponent<SpriteRenderer>();
        headRenderer = transform.Find("img/head").GetComponent<SpriteRenderer>();
        //shadowRenderer = transform.Find("img/shadow").GetComponent<SpriteRenderer>();
        mainImg = transform.Find("img").gameObject;
        m_collider = GetComponent<BoxCollider>();
        //shadowRenderer.sprite = SpManager.Instance.LoadSprite("shadow");
        //bodyRenderer.color = m_manager.skinColor;
        //bodyRenderer.material.SetColor("_Color", m_manager.skinColor);
        if (hasHead)
        {
            //headRenderer.color = m_manager.skinColor;
            //headRenderer.material.SetColor("_Color", m_manager.skinColor);
        }
        else
        {
            headRenderer.enabled = false;
            hairRenderer.enabled = false;
        }
        m_umovement = GetComponent<unitMovement>();
        Update();
        m_manager.m_setFinish = true;
    }

    public Vector3 getBottomPos(float v)
    {
        float zOffset = (bodyRenderer.sprite.pivot.y / bodyRenderer.sprite.rect.height) * -1 * bodyRenderer.bounds.size.z - Globals.UNIT_IMG_BOTTOM + v;
        Vector3 p1 = new Vector3(0, 0, bodyRenderer.bounds.extents.z * 1.6f);
        bottomPos = bodyRenderer.bounds.center  - p1;
        return bottomPos;
    }

    public Vector3 getHeadPos()
    {
        float hOffset = bodyRenderer.bounds.size.z;
        if (hasHead)
        {
            hOffset += headRenderer.bounds.size.z;
        }
        hOffset = hOffset < imgHeight ? imgHeight : hOffset;
        imgHeight = hOffset;
        hOffset += Globals.UNIT_IMG_HEAD;
        Vector3 p2 = new Vector3(0, 0, hOffset);
        headPos = transform.position + p2;
        return headPos;
    }

    private void setBodySprite(string sp)
    {
        //Debug.Log(sp);
        bodyRenderer.sprite = SpManager.Instance.LoadSprite(sp);
        
    }

    private void setheadSprite(string sp)
    {
        //Debug.Log(sp);
        headRenderer.sprite = SpManager.Instance.LoadSprite(sp);
        
    }
    private void sethairSprite(string sp)
    {
        //Debug.Log(sp);
        hairRenderer.sprite = SpManager.Instance.LoadSprite(sp);
    }
    private void PlayAnim()
    {
        switch (m_umovement.getFaceTo())
        {
            case faceTo.down:
                {
                    dir = "front";
                    break;
                }
            case faceTo.up:
                {
                    dir = "back";
                    break;
                }
            case faceTo.left:
                {
                    dir = "left";
                    break;
                }
            case faceTo.right:
                {
                    dir = "right";
                    break;
                }
            default:
                {
                    dir = "front";
                    break;
                }
        }

        setBodySprite(m_manager.bodySkin + "_body_"+ dir);

        if (hasHead)
        {
            sethairSprite(m_manager.headSkin + "_hair_" + dir);
            if (m_umovement.getFaceTo() == faceTo.up)
            {
                setheadSprite("human_face_" + dir);
            }
            else
            {
                setheadSprite("human_face_" + dir + "_" + m_manager.emotion);
            }
        }
    }

    void setCollider()
    {
        float zOffset = (0.5f - bodyRenderer.sprite.pivot.y / bodyRenderer.sprite.rect.height);
        if (hasHead)
        {
            
            m_collider.center = new Vector3(0, 0, (bodyRenderer.bounds.size.z + headRenderer.bounds.size.z)* zOffset - 0.25f);
            m_collider.size = new Vector3(bodyRenderer.bounds.size.x, 0.1f, bodyRenderer.bounds.size.z + headRenderer.bounds.size.z - 0.125f);
        }
        else
        {
            m_collider.center = new Vector3(0, 0, bodyRenderer.bounds.size.z* zOffset);
            m_collider.size = new Vector3(bodyRenderer.bounds.size.x*1.2f, 0.1f, bodyRenderer.bounds.size.z*1.2f);
        }
    }

    public void updateSkin()
    {
        if(sta!=m_manager.sta)
        {
            if(m_manager.sta==STU.down)
            {
                getDown();
            }
            else
            {
                standUp();
            }
            sta = m_manager.sta;
        }
        if ((m_umovement.getLastFaceTo() != m_umovement.getFaceTo()) || m_manager.bodySkin != bodySkinIdNow || m_manager.headSkin != headSkinIdNow || m_manager.emotion != emotion)
        {
            PlayAnim();
            headSkinIdNow = m_manager.headSkin;
            bodySkinIdNow = m_manager.bodySkin;
            emotion = m_manager.emotion;
            m_umovement.setLastFace(m_umovement.getFaceTo());
            setCollider();
        }

        int so = Mathf.RoundToInt(transform.position.z * -100);
        bodyRenderer.sortingOrder = so;
        //shadowRenderer.sortingOrder = so-1;
        if (hasHead)
        {
            headRenderer.sortingOrder = so+1;
            hairRenderer.sortingOrder = so+2;
        }

        Vector3 v3 = transform.position;
        v3.y = Mathf.RoundToInt(transform.position.z / 100) + GSceneMap.Instance.gridWorldSize.y / 100;
        transform.position = v3;
    }
	
	// Update is called once per frame
	void Update ()
    {
        updateSkin();
    }

    public void getDown()
    {
        if (m_manager.unitAngle != 0)
            return;
 
        switch (m_umovement.getFaceTo())
        {
            case faceTo.down:
                {
                    if (Globals.rd.Next(10) > 5)
                        m_manager.unitAngle = -130 + Globals.rd.Next(60);
                    else
                        m_manager.unitAngle = 70 + Globals.rd.Next(60);
                    break;
                }
            case faceTo.up:
                {
                    if (Globals.rd.Next(10) > 5)
                        m_manager.unitAngle = -130 + Globals.rd.Next(60);
                    else
                        m_manager.unitAngle = 70 + Globals.rd.Next(60);
                    break;
                }
            case faceTo.left:
                {
                    m_manager.unitAngle = -130 + Globals.rd.Next(60);
                    break;
                }
            case faceTo.right:
                {
                    m_manager.unitAngle = 70 + Globals.rd.Next(60);
                    break;
                }
            default:
                break;
        }
        mainImg.transform.Rotate(Vector3.up * m_manager.unitAngle);
    }

    public void standUp()
    {
        if(m_manager.unitAngle!=0)
        {
            mainImg.transform.Rotate(Vector3.up * (-1) * m_manager.unitAngle);
            m_manager.unitAngle = 0;
        }
    }
}
