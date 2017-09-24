
using System;

/// <summary>
/// 单例模式的实现
/// </summary>
[Serializable]
public class Singleton<T> where T : class, new()
{
    // 定义一个静态变量来保存类的实例
    private static T _instance;

    // 定义一个标识确保线程同步
    private static readonly object locker = new object();

    /// <summary>
    /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
    /// </summary>
    /// <returns></returns>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }
}