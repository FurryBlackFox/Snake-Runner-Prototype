using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// using position markers is not perfomant
/// variants: less precision or conception changing
/// </summary>
public class SnakeBodyPart : SnakeSegment
{

    public SnakeSegment parent;
    
    private Marker currentMarker;
    
    
    public override void UpdatePosition()
    {
        FindMarkerInQueue();
        
        var newPos = currentMarker.position;
        transform.position = newPos;

    }

    public void FindMarkerInQueue()
    {
        var parentPos = parent.transform.position;
        
        if(currentMarker == null)
            currentMarker ??= parent.markerManager.Peek();
        
        var testedMarker = parent.markerManager.Count > 0 ? parent.markerManager.Peek() : currentMarker;
        var differenceWithParent = parentPos - testedMarker.position;
        var differenceWithParentMagn = differenceWithParent.magnitude;
        currentMarker = differenceWithParentMagn < snakeSettings.spawnOffset
            ? WaitForNewMarkers(parentPos, testedMarker, differenceWithParentMagn)
            : FindSuitableInMarkers(parentPos, testedMarker);
    }


    private Marker WaitForNewMarkers(Vector3 parentPos, Marker testedMarker, float differenceWithParentMagn)
    {
        var prevMarker = currentMarker;
        
        var prevDifferenceWithParent = parentPos - prevMarker.position;
        if (prevDifferenceWithParent != Vector3.zero)
        {
            testedMarker = GenerateMidMarker(prevMarker, testedMarker, 
                prevDifferenceWithParent.magnitude, differenceWithParentMagn);
        }
        else
        {
            testedMarker.position = parentPos + parent.transform.forward * -snakeSettings.spawnOffset;
            var dirToParent = (parentPos - testedMarker.position).normalized;
            testedMarker.rotation = quaternion.LookRotation(dirToParent, Vector3.up);
        }
     

        return testedMarker;
    }

    private Marker FindSuitableInMarkers(Vector3 parentPos, Marker testedMarker)
    {
        while (parent.markerManager.Count > 1)
        {
            testedMarker = parent.markerManager.GetMarker();
            var differenceWithParent = parentPos - testedMarker.position;
            var nextMarker = parent.markerManager.Peek();
            var nextDifferenceWithParent = parentPos - nextMarker.position;

            if (nextDifferenceWithParent.magnitude < snakeSettings.spawnOffset)
            {
                testedMarker = GenerateMidMarker(testedMarker, nextMarker,
                    differenceWithParent.magnitude, nextDifferenceWithParent.magnitude);
                break;
            }
        }
        return testedMarker;
    }

    private Marker GenerateMidMarker(Marker firstMarker, Marker secondMarker, float firstPosMagn, float 
    secondPosMagn)
    {
        var dif = firstPosMagn - snakeSettings.spawnOffset;
        var tValue = dif / (firstPosMagn - secondPosMagn);

        var midPos = Vector3.Lerp(firstMarker.position, secondMarker.position, tValue);
        var midRot = Quaternion.Lerp(firstMarker.rotation, secondMarker.rotation, tValue);
        
        return markerManager.CreateMarker(midPos, midRot);
    }
    

    public override void UpdateRotation()
    {
        transform.rotation = currentMarker.rotation;
    }

}
