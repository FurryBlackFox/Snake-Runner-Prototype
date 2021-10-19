using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 660,661
public struct Marker
#pragma warning restore 660,661
{
    public Vector3 position;
    public Quaternion rotation;

    public static Marker Zero => new Marker(Vector3.zero, Quaternion.identity);

    public Marker(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }

    public static bool operator == (Marker m1, Marker m2)
    {
        return m1.position == m2.position && Mathf.Abs(Quaternion.Dot(m1.rotation, m2.rotation)) < 1 - float.Epsilon;
    }

    public static bool operator !=(Marker m1, Marker m2)
    {
        return !(m1 == m2);
    }
}
