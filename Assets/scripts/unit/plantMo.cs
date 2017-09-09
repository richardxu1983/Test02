using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantMo : MonoBehaviour {

    private plant p;

    public void init(plant _p)
    {
        p = _p;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if (p.toDelete == 1)
        {
            Destroy(p.m_Instance, 1);
        }
    }
}
