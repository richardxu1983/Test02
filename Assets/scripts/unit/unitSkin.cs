using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitSkin : MonoBehaviour {

    private int bodySkinIdNow = 0;
    private int headSkinIdNow = 0;
    private SpriteRenderer bodyRenderer;
    private SpriteRenderer headRenderer;
    private SpriteRenderer heairRenderer;
    private unitMovement m_umovement;
    private unitManager m_manager = new unitManager();
    private bool hasHead = true;

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
        heairRenderer.sprite = SpManager.Instance.LoadSprite(sp);
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
        bodyRenderer.color = m_manager.skinColor();
        if (hasHead)
        {
            headRenderer = transform.Find("head").GetComponent<SpriteRenderer>();
            headRenderer.color = m_manager.skinColor();
            heairRenderer = transform.Find("hair").GetComponent<SpriteRenderer>();
        }
        m_umovement = GetComponent<unitMovement>();
    }
	
	// Update is called once per frame
	void Update ()
    {

        if ((m_umovement.getLastFaceTo() != m_umovement.getFaceTo()) || m_manager.bodySkin() != bodySkinIdNow || m_manager.headSkin() != headSkinIdNow)
        {
            PlayAnim();
            headSkinIdNow = m_manager.headSkin();
            bodySkinIdNow = m_manager.bodySkin();
            m_umovement.setLastFace(m_umovement.getFaceTo());
        }

        /*
        if (Input.GetKeyDown("space"))
        {
            if(test)
            {
                bodyRenderer.sprite = SpManager.Instance.LoadSprite("player_head_front_1");
                test = false;
            }
            else
            {
                bodyRenderer.sprite = SpManager.Instance.LoadSprite("player_body_front_1");
                test = true;
            }
        }
        */
    }
}
