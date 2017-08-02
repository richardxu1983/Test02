using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitSkin : MonoBehaviour {

    private int bodySkinIdNow = 0;
    private int headSkinIdNow = 0;
    public SpriteRenderer bodyRenderer;
    public SpriteRenderer headRenderer;
    public SpriteRenderer hairRenderer;
    private BoxCollider m_collider;
    private unitMovement m_umovement;
    private unitManager m_manager = new unitManager();
    public bool hasHead = true;

    public void init(unitManager v)
    {
        m_manager = v;
        if(m_manager.headSkin()<0)
        {
            hasHead = false;
            headSkinIdNow = m_manager.headSkin();
        }
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

    // Use this for initialization
    void Start () {

        bodyRenderer = transform.Find("body").GetComponent<SpriteRenderer>();
        hairRenderer = transform.Find("hair").GetComponent<SpriteRenderer>();
        headRenderer = transform.Find("head").GetComponent<SpriteRenderer>();
        m_collider = GetComponent<BoxCollider>();

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
        if (hasHead)
        {
            headRenderer.sortingOrder = so;
            hairRenderer.sortingOrder = so;
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
                        transform.Rotate(Vector3.up * (-130 + Globals.rd.Next(60)));
                    else
                        transform.Rotate(Vector3.up * (70 + Globals.rd.Next(60)));
                    break;
                }
            case faceTo.up:
                {
                    if (Globals.rd.Next(10) > 5)
                        transform.Rotate(Vector3.up * (-130 + Globals.rd.Next(60)));
                    else
                        transform.Rotate(Vector3.up * (70 + Globals.rd.Next(60)));
                    break;
                }
            case faceTo.left:
                {
                    transform.Rotate(Vector3.up * (-130 + Globals.rd.Next(60)));
                    break;
                }
            case faceTo.right:
                {
                    transform.Rotate(Vector3.up * (70 + Globals.rd.Next(60)));
                    break;
                }
            default:
                break;

        }
    }
}
