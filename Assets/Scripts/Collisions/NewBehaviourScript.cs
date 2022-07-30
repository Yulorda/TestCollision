using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public const float EPSILON = 1e10f;

    public bool PointToPoint(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) <= EPSILON;
    }

    public bool PointToPoint(Vector2 a, Vector2 b)
    {
        return Vector2.SqrMagnitude(a - b) <= EPSILON;
    }

    public bool PointToCircle(Vector2 a, Vector2 center, float radius)
    {
        return Vector2.Distance(a, center) <= radius;
    }

    public bool PointToSphere(Vector3 a, Vector3 center, float radius)
    {
        return Vector2.Distance(a, center) <= radius;
    }

    public bool CircleToCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB)
    {
        return Vector2.Distance(centerA, centerB) <= radiusA + radiusB;
    }

    public bool SphereToSphere(Vector3 centerA, float radiusA, Vector3 centerB, float radiusB)
    {
        return Vector2.Distance(centerA, centerB) <= radiusA + radiusB;
    }

    public bool PointToRectangle(Vector2 a, Vector2 rectangleMin, Vector2 rectangleMax)
    {
        return a.x >= rectangleMin.x && a.x <= rectangleMax.x &&
            a.y >= rectangleMin.y && a.y <= rectangleMax.y;
    }

    public bool PointToÑuboid(Vector3 a, Vector3 cuboidMin, Vector3 cuboidMax)
    {
        return a.x >= cuboidMin.x && a.x <= cuboidMax.x &&
            a.y >= cuboidMin.y && a.y <= cuboidMax.y &&
            a.z >= cuboidMin.z && a.z <= cuboidMax.z;
    }

    public bool RectangleToRectangle(Vector2 rectangleMinA, Vector2 rectangleMaxA, Vector2 rectangleMinB, Vector2 rectangleMaxB)
    {
        return rectangleMaxA.x >= rectangleMinB.x &&
            rectangleMinA.x <= rectangleMaxB.x &&
            rectangleMaxA.y >= rectangleMinB.y &&
            rectangleMinA.y <= rectangleMaxB.y;
    }

    public bool ÑuboidToÑuboid(Vector3 cuboidMinA, Vector3 cuboidMaxA, Vector3 cuboidMinB, Vector3 cuboidMaxB)
    {
        return cuboidMaxA.x >= cuboidMinB.x &&
            cuboidMinA.x <= cuboidMaxB.x &&
            cuboidMaxA.y >= cuboidMinB.y &&
            cuboidMinA.y <= cuboidMaxB.y &&
            cuboidMaxA.z >= cuboidMinB.z &&
            cuboidMinA.z <= cuboidMaxB.z;
    }

    public Vector2 GetNearstToPoint(Vector2 point, Vector2 rectangleMin, Vector2 rectangleMax)
    {
        var nearest = point;

        if (point.x < rectangleMin.x)
        {
            nearest.x = rectangleMin.x;
        }
        else if (point.x > rectangleMax.x)
        {
            nearest.x = rectangleMax.x;
        }

        if (point.y < rectangleMin.y)
        {
            nearest.y = rectangleMin.y;
        }
        else if (point.x > rectangleMax.y)
        {
            nearest.y = rectangleMax.y;
        }

        return nearest;
    }

    public Vector2 GetNearstToPoint(Vector3 point, Vector3 rectangleMin, Vector3 rectangleMax)
    {
        var nearest = point;

        if (point.x < rectangleMin.x)
        {
            nearest.x = rectangleMin.x;
        }
        else if (point.x > rectangleMax.x)
        {
            nearest.x = rectangleMax.x;
        }

        if (point.y < rectangleMin.y)
        {
            nearest.y = rectangleMin.y;
        }
        else if (point.x > rectangleMax.y)
        {
            nearest.y = rectangleMax.y;
        }

        if (point.z < rectangleMin.z)
        {
            nearest.z = rectangleMin.z;
        }
        else if (point.z > rectangleMax.z)
        {
            nearest.z = rectangleMax.z;
        }

        return nearest;
    }

    public bool CircleToRectangle(Vector2 center, float radius, Vector2 rectangleMin, Vector2 rectangleMax)
    {
        return PointToCircle(GetNearstToPoint(center, rectangleMin, rectangleMax), center, radius);
    }

    public bool SphereToCuboid(Vector3 center, float radius, Vector3 rectangleMin, Vector3 rectangleMax)
    {
        return PointToSphere(GetNearstToPoint(center, rectangleMin, rectangleMax), center, radius);
    }

    public bool PointOnLine(Vector3 a, Vector3 start, Vector3 end)
    {
        return Vector3.Dot(end - start, a - start) >= 0.98f;
    }
}
