using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitSkin : MonoBehaviour {

    private int uid = 0;
    private int bodySkinIdNow = 0;
    private int headSkinIdNow = 0;
    private SpriteRenderer bodyRenderer;
    private SpriteRenderer headRenderer;
    private unitMovement m_umovement;
    private unitManager m_manager = new unitManager();
    private bool test = true;

    public void init(unitManager v)
    {
        m_manager = v;
    }

    private void setBodySprite(string sp)
    {
        bodyRenderer.sprite = SpManager.Instance.ReadSpritesByString(sp);
    }

    private void setheadSprite(string sp)
    {
        headRenderer.sprite = SpManager.Instance.ReadSpritesByString(sp);
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
        setBodySprite("player_body_front_" + m_manager.bodySkin());
        setheadSprite("player_head_front_" + m_manager.headSkin());
    }

    private void faceUp()
    {
        setBodySprite("player_body_back_" + m_manager.bodySkin());
        setheadSprite("player_head_back_" + m_manager.headSkin());
    }

    private void faceLeft()
    {
        setBodySprite("player_body_left_" + m_manager.bodySkin());
        setheadSprite("player_head_left_" + m_manager.headSkin());
    }

    private void faceRight()
    {
        setBodySprite("player_body_right_" + m_manager.bodySkin());
        setheadSprite("player_head_right_" + m_manager.headSkin());
    }

    // Use this for initialization
    void Start () {
        bodyRenderer = transform.Find("body").GetComponent<SpriteRenderer>();
        //headRenderer = transform.Find("head").GetComponent<SpriteRenderer>();
        //m_umovement = GetComponent<unitMovement>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        /*
        if ((m_umovement.getLastFaceTo() != m_umovement.getFaceTo()) || m_manager.bodySkin() != bodySkinIdNow || m_manager.headSkin() != headSkinIdNow)
        {
            PlayAnim();
            headSkinIdNow = m_manager.headSkin();
            bodySkinIdNow = m_manager.bodySkin();
            m_umovement.setLastFace(m_umovement.getFaceTo());
        }
        */
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
    }
}
