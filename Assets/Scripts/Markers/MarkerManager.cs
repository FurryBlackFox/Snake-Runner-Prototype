using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManager //seems like a good idea but a crooked implementation 
{
    private Queue<Marker> queue = new Queue<Marker>();

    public int Count => queue.Count;


    public Marker GetMarker()
    {
        var marker = queue.Dequeue();
        marker.Unsubscribe();
        return marker;
    }
    
    public Marker Peek()
    {
        return queue.Peek();
    }

    public void Enqueue(Marker marker)
    {
        marker.Subscribe();
        queue.Enqueue(marker);
    }

    public Marker CreateMarker(Vector3 postion, Quaternion rotation)
    {
        var marker = ObjectPoolsManager.MarkerObjectPool.Get();
        marker.position = postion;
        marker.rotation = rotation;
        return marker;
    }

    public void DeleteFirstMarker()
    {
        queue.Dequeue().ReturnToPool();
    }
}
