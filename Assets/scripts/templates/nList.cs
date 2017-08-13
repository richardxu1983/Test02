using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nList<T>
{
    public T[] array;
    private Stack<int> stack;
    public int count;
    public int index = 0;
    public int MaxNum = 0;


    public nList(int v)
    {
        array = new T[v];
        count = 0;
        stack = new Stack<int>();
        MaxNum = v;
    }

    public int add(T v)
    {
        int i;
        if (stack.Count>0)
        {
            i = stack.Pop();
            array[i] = v;
            //Debug.Log("在 " + i + " 位置添加角色");
            count++;
            //Debug.Log("一共 " + count + " 个角色");
            return i;
        }
        else
        {
            if(MaxNum== index)
            {
                return -1;
            }
            array[index] = v;
            //Debug.Log("在 " + index + " 位置添加角色");
            index++;
            count++;
            //Debug.Log("一共 " + count + " 个角色");
            return (index-1);
        }
    }

    public void removeAt(int v)
    {
        array[v] = default(T);
        //Debug.Log("于 " + v + " 删除角色");
        stack.Push(v);
        count--;
        //Debug.Log("一共 " + count + " 个角色");
    }

    public T get(int v)
    {
        return array[v];
    }
}
