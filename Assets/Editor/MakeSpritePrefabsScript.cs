// =================================================================================================
//
//    Hammerc Library
//    Copyright 2015 hammerc.org All Rights Reserved.
//
//    See LICENSE for full license information.
//
// =================================================================================================

using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
///
/// 2D 图集生成对应预制件脚本类.
///
/// 解决问题:
/// 解决不能动态设置 Sprite 对象的贴图的问题, 因为分散的小图最终需要打包为图集而 Unity3D 又将图集的处理隐藏, 所以需要将小图的信息存放到预制件中.
///
/// 使用方法及步骤:
/// 1.修改源目录及目标目录为自己使用的目录;
/// 2.把小图存放到源目录中;
/// 3.在菜单栏点击 Hammerc/2D/MakeSpritePrefabs 即可在目标目录下生成对应的预制件;
/// 4.通过获取生成的预制件的 sprite 对象即可在程序中使用小图.
///
/// </summary>
public class MakeSpritePrefabsScript
{
    /// <summary>
    /// Assets 目录下的小图片目录, 包括子目录的所有图片文件都会进行处理.
    /// </summary>
    private const string ORIGIN_DIR = "\\RawData\\Sprites";

    /// <summary>
    /// Assets 目录下的小图预制件生成的目标目录, 注意该目录下不要存放其他资源, 每次生成时都会清空该目录下的所有文件.
    /// </summary>
    private const string TARGET_DIR = "\\Resources\\Sprites";

    /// <summary>
    /// 将制定目录下的原始图片一对一打包成 Prefab 方便在游戏运行中读取指定的图片.
    /// </summary>
    [MenuItem("Hammerc/2D/MakeSpritePrefabs")]
    private static void MakeSpritePrefabs()
    {
        //EditorUtility.DisplayProgressBar();
        string spriteDir = Application.dataPath + "/Resources/Sprites";
        string originDir = Application.dataPath + ORIGIN_DIR;
        DirectoryInfo originDirInfo = new DirectoryInfo(originDir);

        if (!Directory.Exists(spriteDir))
        {
            Directory.CreateDirectory(spriteDir);
        }

        //Debug.Log(originDirInfo.GetFiles("*.png", SearchOption.AllDirectories));

        foreach (FileInfo pngFile in originDirInfo.GetFiles("*.png", SearchOption.AllDirectories))
        {
            //Debug.Log("33333");
            string allPath = pngFile.FullName;
            string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            GameObject go = new GameObject(sprite.name);
            go.AddComponent<SpriteRenderer>().sprite = sprite;
            allPath = spriteDir + "/" + sprite.name + ".prefab";
            string prefabPath = allPath.Substring(allPath.IndexOf("Assets"));
            //Debug.Log(prefabPath);
            PrefabUtility.CreatePrefab(prefabPath, go);
            GameObject.DestroyImmediate(go);
        }

    }
}