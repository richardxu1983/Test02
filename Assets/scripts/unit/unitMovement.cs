using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class unitMovement : MonoBehaviour {


    [SerializeField] int fps;
    public Text textInfo;
    private unitManager m_manager = new unitManager();
    private faceTo playerFace;//0:down,1:up,2:left,3:right
    private faceTo playerFaceLast;//0:down,1:up,2:left,3:right
    private Vector3 m_movement;

    public void init(unitManager v)
    {
        m_manager = v;
    }

    private void Awake()
    {

    }

    public unitManager manager()
    {
        return m_manager;
    }

    public faceTo getFaceTo()
    {
        return playerFace;
    }

    public faceTo getLastFaceTo()
    {
        return playerFaceLast;
    }

    public void setLastFace(faceTo f)
    {
        playerFaceLast = f;
    }

    private void OnEnable()
    {
        //m_manager.ai().TargetPos = transform.position;
    }

    // Use this for initialization
    void Start()
    {
        playerFace = faceTo.down;
        playerFaceLast = faceTo.none;
    }

    // Update is called once per frame
    void Update()
    {

        if (m_manager.ai().op == UnitOp.moving)
        {
            Move();
        }
        m_manager.setPos(transform.position);
    }

    void LateUpdate()
    {

    }

    private void FixedUpdate()
    {
        //Debug.Log(GlobalControl.Instance.enableDebug);
        if(GlobalControl.Instance.enableDebug==true)
        {
            if(textInfo.enabled==false)
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

    private void Move()
    {

        Vector3 movement = m_manager.ai().TargetPos - transform.position;
        //Debug.Log("Move : "+ movement);
        movement.y = 0;
        if (movement.sqrMagnitude >= 0.1f)
        {
            movement.Normalize();
            //Debug.Log("Normalize : " + movement);

            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.z))
            {
                if (movement.x > 0)
                {
                    playerFace = faceTo.right;
                }
                else
                {
                    playerFace = faceTo.left;
                }
            }
            else
            {
                if (movement.z > 0)
                {
                    playerFace = faceTo.up;
                }
                else
                {
                    playerFace = faceTo.down;
                }
            }

            m_movement = movement * m_manager.runSpeed()*Time.deltaTime;
            //transform.position = transform.position + (new Vector3(0.01f,0.01f,0));
            transform.Translate(m_movement);
            //transform.position = Vector3.MoveTowards(transform.position, m_manager.ai().TargetPos, m_manager.runSpeed() * Time.deltaTime);
            
        }
        else
        {
        }
        //transform.position = transform.position + (new Vector3(.01f, 0f, .01f));
    }
}
