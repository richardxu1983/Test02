using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class unitManager {

    public unitAi m_ai;
    public unitData m_data;
    public GameObject m_Instance;
    private unitMovement m_movement;
    public unitSkin m_skin;
    private int m_type = 0;
    private bool m_isDead = false;
    private bool isSet = false;


    public unitManager()
    {
        m_data = new unitData();
        m_ai = new unitAi();
        m_data.init(this);
        m_ai.init(this);
    }

    public void spawnAt(Vector3 v)
    {
        Assert.IsTrue(isSet);
        m_Instance = Object.Instantiate(Resources.Load("Prefab/unit"), v, new Quaternion(0, 0, 0, 0)) as GameObject;
        m_movement = m_Instance.GetComponent<unitMovement>();
        m_skin = m_Instance.GetComponent<unitSkin>();
        m_movement.init(this);
        m_skin.init(this);
    }

    public void CreateHuman(int uid, int speed)
    {
        setId(uid);
        m_type = Globals.humanType;
        int headSkin = 1000;
        int bodySkin = 1000;
        int skinColorId = Globals.rd.Next(utils.Instance.maxColors);
        setHeadSkin(headSkin);
        setBodySkin(bodySkin);
        setSkinColor(skinColorId);
        m_data.skinColor = utils.Instance.getSkinColor(m_data.getInt(UnitIntAttr.skinColor));
        setName("human");
        Setup(speed);
    }

    public void CreateAnimalById(int uid, int typeId)
    {
        setId(uid);
        m_type = Globals.animalType;
        setHeadSkin(XMLLoader.Instance.animalConfig[typeId].headId);
        m_data.skinColor = utils.Instance.getSkinColor(m_data.getInt(UnitIntAttr.skinColor));
        setBodySkin(XMLLoader.Instance.animalConfig[typeId].bodyId);
        setName(XMLLoader.Instance.animalConfig[typeId].name);
        Setup(XMLLoader.Instance.animalConfig[typeId].speed);
    }

    public void Setup(float speed)
    {
        setRunSpeed(speed);
        m_data.setInt(UnitIntAttr.hp, 100);
        isSet = true;
    }

    public int id() { return m_data.getInt(UnitIntAttr.uid); }
    public int hp() { return m_data.getInt(UnitIntAttr.hp); }
    public unitAi ai() { return m_ai; }
    public float runSpeed() { return m_data.getF(UnitFloatAttr.run_speed); }
    public float walkSpeed() { return m_data.getF(UnitFloatAttr.run_speed); }
    public Vector3 pos() { return m_data.pos; }
    public int hpMax() { return m_data.getInt(UnitIntAttr.hpMax); }
    public unitData attr() { return m_data; }
    public int headSkin() { return m_data.getInt(UnitIntAttr.headSkin); }
    public int bodySkin() { return m_data.getInt(UnitIntAttr.bodySkin); }
    public string name() { return m_data.name; }
    public Color skinColor() { return m_data.skinColor; }

    public void hpAdd(int v)
    {
        int hp = this.hp();
        if(hp>0)
        {
            hp += v;
            hp = hp < 0 ? 0 : hp;
            hp = hp > hpMax() ? hpMax() : hp;
            m_data.setInt(UnitIntAttr.hp, hp);
        }
        if( hp<=0 && m_isDead==false )
        {
            die();
        }
    }

    public void setName(string v)
    {
        m_data.name = v;
    }

    public void setId(int v)
    {
        m_data.setInt(UnitIntAttr.uid, v);
    }

    public void setSkinColor(int v)
    {
        m_data.setInt(UnitIntAttr.skinColor, v);
    }

    public void setTargetPos(Vector3 v)
    {
        m_ai.TargetPos = v;
    }

    public void setRunSpeed(float v)
    {
        m_data.setF(UnitFloatAttr.run_speed, v);
    }

    public void setWalkSpeed(float v)
    {
        m_data.setF(UnitFloatAttr.walk_speed, v);
    }

    public void setHeadSkin(int v)
    {
        m_data.setInt(UnitIntAttr.headSkin, v);
    }

    public void setBodySkin(int v)
    {
        m_data.setInt(UnitIntAttr.bodySkin, v);
    }

    public void setPos(Vector3 v)
    {
        m_data.pos = v;
    }

    public void die()
    {
        setDead(true);
        m_ai.die();
        m_skin.die();
    }

    public bool isDead()
    {
        return m_isDead;
    }

    public void setDead(bool v)
    {
        m_isDead = v;
    }

    public void loop()
    {
        m_data.loop();
        m_ai.loop();
    }
}
