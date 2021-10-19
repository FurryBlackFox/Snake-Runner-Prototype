using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManager //seems like a good idea but a crooked implementation 
{
    //private Queue<Marker> queue = new Queue<Marker>();
    private List<Marker> markers = new List<Marker>();

    public int Count => markers.Count;

    public MarkerManager()
    {
        Subscribe();
    }

    ~MarkerManager()
    {
        Unsubscribe();
    }
    
    public void Subscribe()
    {
        GameManager.OnUpdatePosition += UpdatePositions;
    }

    public void Unsubscribe()
    {
        GameManager.OnUpdatePosition -= UpdatePositions;
    }

    private void UpdatePositions(float deltaSpeed)
    {
        for (var index = 0; index < markers.Count; index++)
        {
            var marker = markers[index];
            marker.position.z += deltaSpeed;
            markers[index] = marker;
        }
    }

    public Marker Peek()
    {
        return markers[0];
    }

    public Marker Dequeue()
    {
        var marker = markers[0];
        markers.RemoveAt(0);
        return marker;
    }

    public void Enqueue(Marker marker)
    {
        markers.Add(marker);
    }
}
