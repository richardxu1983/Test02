﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class unitMovement : MonoBehaviour {


    [SerializeField] int fps;

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

    void FixedUpdate()
    {
        if (m_manager.ToDelete == 1)
        {
            Destroy(m_manager.m_Instance, Globals.UNIT_DELETE_TIME);
        }
    }

    void OnDestroy()
    {
        unitPool.Instance.freeSelect();
        m_manager.ToDelete = 2;
    }

    private void Move()
    {
        GridID g = m_manager.ai().nextGrid();
        if(g!=null)
        {
            Vector3 movement = GSceneMap.Instance.nodeFromGrid(g).worldPosition - transform.position;

            if (Mathf.Abs(movement.x) >= 0.03f || Mathf.Abs(movement.z) >= 0.03f)
            {
                movement.Normalize();

                if (Mathf.Abs(movement.x) > Mathf.Abs(movement.z))
                {
                    if (movement.x > 0)
                    {
                        playerFace = faceTo.right;
                    }
                    else if(movement.x < 0)
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
                    else if(movement.z < 0)
                    {
                        playerFace = faceTo.down;
                    }
                }
                
                m_movement = movement * m_manager.runSpeed() * Time.deltaTime;
                transform.Translate(m_movement);
                /*
                if( Mathf.Abs( g.x-m_manager.grid().x) > Mathf.Abs(g.y - m_manager.grid().y))
                {
                    if(g.x> m_manager.grid().x)
                    {
                        playerFace = faceTo.right;
                    }
                    else if(g.x < m_manager.grid().x)
                    {
                        playerFace = faceTo.left;
                    }
                }
                else
                {
                    if (g.y > m_manager.grid().y)
                    {
                        playerFace = faceTo.up;
                    }
                    else if(g.y < m_manager.grid().y)
                    {
                        playerFace = faceTo.down;
                    }
                }*/
            }
            else
            {
            }
        }
        //transform.position = transform.position + (new Vector3(.01f, 0f, .01f));
    }
}
