﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class unitMovement : MonoBehaviour {


    [SerializeField] int fps;

    private unitBase m_manager = new unitBase(1,1);
    private Vector3 m_movement;

    public void init(unitBase v)
    {
        m_manager = v;
    }

    private void Awake()
    {

    }

    public unitBase manager()
    {
        return m_manager;
    }

    public faceTo getFaceTo()
    {
        return m_manager.playerFace;
    }

    public faceTo getLastFaceTo()
    {
        return m_manager.playerFaceLast;
    }

    public void setLastFace(faceTo f)
    {
        m_manager.playerFaceLast = f;
    }

    private void OnEnable()
    {
        //m_manager.ai().TargetPos = transform.position;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!GlobalControl.Instance.bLogicPause)
        {
            if (m_manager.ai.op == OP.moving)
            {
                Move();
            }
            m_manager.pos = transform.position;
        }
    }

    void FixedUpdate()
    {
        if (m_manager.toDelete == 1)
        {
            Destroy(m_manager.m_Instance, Globals.UNIT_DELETE_TIME);
        }
    }

    void OnDestroy()
    {
        unitPool.Instance.freeSelect();
        m_manager.toDelete = 2;
    }

    private void Move()
    {
        GridID g = m_manager.ai.nextGrid();
        if(g!=null)
        {
            Vector3 movement = GSceneMap.Instance.nodeFromGrid(g).worldPosition - transform.position;
            movement.y = 0;
            
            if (Mathf.Abs(movement.x) >= 0.03f || Mathf.Abs(movement.z) >= 0.03f)
            {
                movement.Normalize();

                if (Mathf.Abs(movement.x) > Mathf.Abs(movement.z)*1.1)
                {
                    if (movement.x > 0)
                    {
                        m_manager.playerFace = faceTo.right;
                    }
                    else if(movement.x < 0)
                    {
                        m_manager.playerFace = faceTo.left;
                    }
                }
                else
                {
                    if (movement.z > 0)
                    {
                        m_manager.playerFace = faceTo.up;
                    }
                    else if(movement.z < 0)
                    {
                        m_manager.playerFace = faceTo.down;
                    }
                }
                
                m_movement = movement * m_manager.runSpeed * Time.deltaTime;

                //Debug.Log(movement+" , "+m_movement+","+ m_manager.runSpeed+" , "+ Time.deltaTime);
                transform.position += m_movement;
            }
        }
    }
}
