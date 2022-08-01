using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

using UnityEngine;
using UnityEngine.TestTools;

public class CollisionTest
{
    [Test]
    public void CollisionTestPointOnLine()
    {
        Vector3 a = new Vector3(0, 5, 5);
        Vector3 start = new Vector3(0, 0, 0);
        Vector3 end = new Vector3(0, 10, 10);
        Assert.IsTrue(Collisions.PointOnLine(a, start, end));

        a = new Vector3(0, 5, 4);
        start = new Vector3(0, 0, 0);
        end = new Vector3(0, 10, 10);
        Assert.IsFalse(Collisions.PointOnLine(a, start, end));
    }

    [Test]
    public void CollisionTestCircleOnLine()
    {
        Vector3 a = new Vector3(1, 5, 5);
        Vector3 start = new Vector3(0, 0, 0);
        Vector3 end = new Vector3(0, 10, 10);
        Assert.IsTrue(Collisions.SphereToLine(a, 1f, start, end));
        Assert.IsFalse(Collisions.SphereToLine(a, 0.99f, start, end));
    }

    [Test]
    public void CollisionTestLineToLine()
    {
        Vector3 startA = new Vector3(0, 0, 0);
        Vector3 startB = new Vector3(0, 0, 0);
        Vector3 endA = new Vector3(0, 20, 10);
        Vector3 endB = new Vector3(0, 20, 10);
        Assert.IsTrue(Collisions.LineToLine(startA, endA, startB, endB));
    }

    [Test]
    public void ConvexAlghorithmm()
    {
        var list = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 10), new Vector2(10, 1), new Vector2(5, 10), new Vector2(10, 5), new Vector2(7, 7), new Vector2(10, 10) };
        Algorithms.GetConvexHull(list).ForEach(x=>Debug.Log(x));
    }
}
