using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularBuffer<T>{

    List<T> data;
    int size;
    int pointer;

    public CircularBuffer(List<T> nData)
    {
        data = new List<T>(nData);
        size = data.Count;
        pointer = 0;
    }

    public int getSize()
    {
        return size;
    }

    public T getNext()
    {
        T item = data[pointer];
        pointer += 1;
        pointer %= size;
        return item;
    }

    public T getPrevious()
    {
        T item = data[pointer];
        pointer -= 1;
        if(pointer < 0)
        {
            pointer += size;
        }
        return item;
    }

    public void insert(T item, int pos)
    {
        data[pos] = item;
    }

    

    



}
