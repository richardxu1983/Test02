using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpManager : UnitySingleton<SpManager>
{
    private Dictionary<string, object> spritesDictionary = new Dictionary<string, object>();
    private Sprite[] sprites;

    public void LoadAllSprites()
    {
        sprites = Resources.LoadAll<Sprite>("");//Path是路径，Sprite是类型，改成Sprites的目录就行。当然你想读取所有，那么就根目录也行。
        for (int i = 0; i < sprites.Length; i++)
        {
            //print("sprites[" + i + "]: " + sprites[i]);
            spritesDictionary.Add(sprites[i].name, sprites[i]);
        }
    }

    public Sprite ReadSpritesByString(string name)
    {
        Sprite a = null;
        foreach (KeyValuePair<string, object> pair in spritesDictionary)
        {
            if (pair.Key.ToString() == name)
            {
                //Debug.Log(pair.Key + " " + pair.Value);
                a = pair.Value as Sprite;
            }
        }
        return a;
    }

    public void init()
    {
        LoadAllSprites();
    }
}
