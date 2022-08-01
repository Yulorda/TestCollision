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
        Assert.IsTrue(Collisions.CircleToLine(a, 1f, start, end));
        Assert.IsFalse(Collisions.CircleToLine(a, 0.99f, start, end));
    }

    [Test]
    public void CollisionTestLineToLine()
    {
        Vector3 startA = new Vector3(0, 0, 0);
        Vector3 startB = new Vector3(0, 0, 0);
        Vector3 endA = new Vector3(0, 20, 10);
        Vector3 endB = new Vector3(0, 20, 10);
        Assert.IsTrue(Collisions.LineToLine2(startA, endA, startB, endB));
    }
}
