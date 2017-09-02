using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitSkin : MonoBehaviour {

    private int bodySkinIdNow = 0;
    private int headSkinIdNow = 0;
    public SpriteRenderer bodyRenderer;
    public SpriteRenderer headRenderer;
    public SpriteRenderer hairRenderer;
    //public SpriteRenderer shadowRenderer;
    private BoxCollider m_collider;
    private unitMovement m_umovement;
    private GameObject mainImg;
    private unitManager m_manager = new unitManager();
    public bool hasHead = true;
    private Vector3 bottomPos;
    private Vector3 headPos;
    private float imgHeight = 0;

    public void init(unitManager v)
    {
        m_manager = v;
        if(m_manager.headSkin()<0)
        {
            hasHead = false;
            headSkinIdNow = m_manager.headSkin();
        }
    }

    // Use this for initialization
    void Start()
    {

        bodyRenderer = transform.Find("img/body").GetComponent<SpriteRenderer>();
        hairRenderer = transform.Find("img/hair").GetComponent<SpriteRenderer>();
        headRenderer = transform.Find("img/head").GetComponent<SpriteRenderer>();
        //shadowRenderer = transform.Find("img/shadow").GetComponent<SpriteRenderer>();
        mainImg = transform.Find("img").gameObject;
        m_collider = GetComponent<BoxCollider>();
        //shadowRenderer.sprite = SpManager.Instance.LoadSprite("shadow");
        bodyRenderer.color = m_manager.skinColor();
        if (hasHead)
        {
            headRenderer.color = m_manager.skinColor();
        }
        else
        {
            headRenderer.enabled = false;
            hairRenderer.enabled = false;
        }
        m_umovement = GetComponent<unitMovement>();
        Update();
    }

    public Vector3 getBottomPos(float v)
    {
        float zOffset = (bodyRenderer.sprite.pivot.y / bodyRenderer.sprite.rect.height) * -1 * bodyRenderer.bounds.size.z - Globals.UNIT_IMG_BOTTOM + v;
        Vector3 p1 = new Vector3(0, 0, zOffset);
        bottomPos = transform.position + p1;
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
                    faceDown();
                    break;
                }
            case faceTo.up:
                {
                    faceUp();
                    break;
                }
            case faceTo.left:
                {
                    faceLeft();
                    break;
                }
            case faceTo.right:
                {
                    faceRight();
                    break;
                }
            default:
                {
                    faceDown();
                    break;
                }
        }
    }

    private void faceDown()
    {
        setBodySprite(m_manager.bodySkin()+"_body_front");
        if(hasHead)
        {
            setheadSprite(m_manager.headSkin()+"_head_front");
            sethairSprite(m_manager.headSkin() + "_hair_front");
        }
    }

    private void faceUp()
    {
        setBodySprite(m_manager.bodySkin() + "_body_back");
        if (hasHead)
        {
            setheadSprite(m_manager.headSkin() + "_head_back");
            sethairSprite(m_manager.headSkin() + "_hair_back");
        }
    }

    private void faceLeft()
    {
        setBodySprite(m_manager.bodySkin() + "_body_left");
        if (hasHead)
        {
            setheadSprite(m_manager.headSkin() + "_head_left");
            sethairSprite(m_manager.headSkin() + "_hair_left");
        }
    }

    private void faceRight()
    {
        setBodySprite(m_manager.bodySkin() + "_body_right");
        if (hasHead)
        {
            setheadSprite(m_manager.headSkin() + "_head_right");
            sethairSprite(m_manager.headSkin() + "_hair_right");
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
        if ((m_umovement.getLastFaceTo() != m_umovement.getFaceTo()) || m_manager.bodySkin() != bodySkinIdNow || m_manager.headSkin() != headSkinIdNow)
        {
            PlayAnim();
            headSkinIdNow = m_manager.headSkin();
            bodySkinIdNow = m_manager.bodySkin();
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
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(m_manager.isDead()==false)
        {
            updateSkin();
        }
    }

    public void die()
    {
        updateSkin();
        switch(m_umovement.getFaceTo())
        {
            case faceTo.down:
                {
                    if (Globals.rd.Next(10) > 5)
                        mainImg.transform.Rotate(Vector3.up * (-130 + Globals.rd.Next(60)));
                    else
                        mainImg.transform.Rotate(Vector3.up * (70 + Globals.rd.Next(60)));
                    break;
                }
            case faceTo.up:
                {
                    if (Globals.rd.Next(10) > 5)
                        mainImg.transform.Rotate(Vector3.up * (-130 + Globals.rd.Next(60)));
                    else
                        mainImg.transform.Rotate(Vector3.up * (70 + Globals.rd.Next(60)));
                    break;
                }
            case faceTo.left:
                {
                    mainImg.transform.Rotate(Vector3.up * (-130 + Globals.rd.Next(60)));
                    break;
                }
            case faceTo.right:
                {
                    mainImg.transform.Rotate(Vector3.up * (70 + Globals.rd.Next(60)));
                    break;
                }
            default:
                break;

        }
    }
}
