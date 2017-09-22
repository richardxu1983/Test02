using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class unitUI : MonoBehaviour {

    public Text txtUnitName;
    public Slider hpBar;
    public GameObject hpBarObj;
    private Image bottomPanel;
    public Image hpFill;
    private GameObject bPanel;
    public Text textInfo;
    private unitBase m_manager = new unitBase(1,1);
    private SpriteRenderer bodyRenderer;

    public void init(unitBase v)
    {
        m_manager = v;
    }

    // Use this for initialization
    void Start () {
        bodyRenderer = transform.Find("img/body").GetComponent<SpriteRenderer>();
        bottomPanel = transform.Find("Canvas/bottomPanel").GetComponent<Image>();
        bPanel = transform.Find("Canvas/bottomPanel").gameObject;
        hpBarObj = transform.Find("Canvas/hpBar").gameObject;
        txtUnitName.text = m_manager.name;
        bottomPanel.rectTransform.sizeDelta = new Vector2(txtUnitName.preferredWidth+1, 1);
        setNamePos();
    }

    void setNamePos()
    {
        if(m_manager.m_setFinish)
        {
            bPanel.transform.position = m_manager.m_skin.getBottomPos(0f);
            hpBarObj.transform.position = m_manager.m_skin.getHeadPos();
        }
    }

    void refresHpBar()
    {
        if (!m_manager.dead)
        {
            float hp = m_manager.hp();
            float hpMax = m_manager.hpMax;

            if (m_manager.hp() < m_manager.hpMax)
            {
                float rate = hp / hpMax;
                if (rate>Globals.HP_YELLOW)
                {
                    hpFill.color = Color.green;
                }
                else if(rate>=Globals.HP_RED)
                {
                    hpFill.color = Color.yellow;
                }
                else
                {
                    hpFill.color = Color.red;
                }
                hpBarObj.SetActive(true);
                hpBar.maxValue = m_manager.hpMax;
                hpBar.value = m_manager.hp();
            }
            else
            {
                hpBarObj.SetActive(false);
            }
        }
    }

    public void die()
    {
        hpBarObj.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(m_manager.bDebugInfo)
        {
            if (GlobalControl.Instance.enableDebug == true)
            {
                if (textInfo.enabled == false)
                {
                    textInfo.enabled = true;
                }
                textInfo.text = "ai : " + m_manager.ai.ai + "\nop : " + m_manager.ai.op + "\nr : " + m_manager.ai.reason;
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
        refresHpBar();

        if (GlobalControl.Instance.showUnitName)
        {
            bPanel.SetActive(true);
        }
        else
        {
            bPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
