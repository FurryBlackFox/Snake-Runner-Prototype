using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolableObject
{
    public void AssignToPool<T>(ObjectPool<T> objectPool) where T : IPoolableObject, new();
    public void ReturnToPool();
    public void Hide();
    public void Reveal();
}
