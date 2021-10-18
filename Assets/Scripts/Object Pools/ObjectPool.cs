using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class ObjectPool<T> where T : IPoolableObject, new()
{
    protected Queue<T> poolObjects;
  

    protected Transform parent;
    protected int poolSize;

    protected bool isQuitting = false;
    public ObjectPool(int newPoolSize)
    {
        poolObjects = new Queue<T>();
        parent = new GameObject($"{typeof(T)} Object Pool").transform;

        poolSize = newPoolSize;
        Application.quitting += () => isQuitting = true;
    }

    public T Get()
    {
        if (poolObjects.Count == 0)
        {
            GenerateNewObjects();
        }
        var obj = poolObjects.Dequeue();
        obj.Reveal();
        return obj;
    }

    public virtual void Return(T obj)
    {
        obj.Hide();
        poolObjects.Enqueue(obj);
    }

    public int Count()
    {
        return poolObjects.Count;
    }

    protected virtual void GenerateNewObjects()
    {
        for (var i = 0; i < poolSize; i++)
        {
            var newObj = new T();
            newObj.Hide();
            newObj.AssignToPool(this);
            poolObjects.Enqueue(newObj);
        }
    }
}
