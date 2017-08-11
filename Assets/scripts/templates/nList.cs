using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nList<T>
{
    public T[] array;
    private Stack<int> stack;
    public int count;
    public int index = 0;

    public void nLis(int v)
    {
        array = new T[v];
        count = 0;
        stack = new Stack<int>();
    }

    public void init(int v)
    {
        array = new T[v];
        count = 0;
        stack = new Stack<int>();
    }

    public int add(T v)
    {
        int i;
        if (stack.Count>0)
        {
            i = stack.Pop();
            array[i] = v;
            count++;
            return i;
        }
        else
        {
            array[index] = v;
            index++;
            count++;
            return (index-1);
        }
    }

    public void removeAt(int v)
    {
        array[v] = default(T);
        count--;
    }

    public T get(int v)
    {
        return array[v];
    }
}
