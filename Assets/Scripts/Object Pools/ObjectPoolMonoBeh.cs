using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolMonoBeh<T> : ObjectPool<T> where T : MonoBehaviour, IPoolableObject, new()
{
    public T objectPrefab;
    
    public ObjectPoolMonoBeh(int newPoolSize, T newObjectPrefab) : base(newPoolSize)
    {
        objectPrefab = newObjectPrefab;
    }

    public override void Return(T obj)
    {
        if(isQuitting)
            return; 
        
        base.Return(obj);
        obj.transform.parent = parent;
    }
    
    protected override void GenerateNewObjects()
    {
        for (var i = 0; i < poolSize; i++)
        {
            var newObj = Object.Instantiate(objectPrefab, parent);
            newObj.Hide();
            newObj.AssignToPool(this);
            poolObjects.Enqueue(newObj);
        }
    }
}
