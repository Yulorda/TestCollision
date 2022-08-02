
using System.Collections.Generic;

using UnityEngine;

public static class Utills
{
    public const float EPSILON = 1e-10f;
    public static float PolygonArea(Vector2[] polygon)
    {
        var area = 0f;
        for (int i = 0; i < polygon.Length; i++)
        {
            var current = polygon[i];
            var next = polygon.Next(i);

            area += Cross(current, next);
        }

        return area / 2f;
    }

    public static int Side(Vector2 a, Vector2 b, Vector2 c)
    {
        var ab = b - a;
        var ac = c - a;
        var cross = Cross(ab, ac);
        return cross > EPSILON ? 1 : (cross < -EPSILON ? -1 : 0);
    }

    public static float Cross(this Vector2 lhs, Vector2 rhs)
    {
        return lhs.x * rhs.y - lhs.x * rhs.y;
    }

    public static Vector2 GetNearstToLine(Vector2 point, Vector2 start, Vector2 end)
    {
        var ac = point - start;
        var ab = end - start;
        var dotProduct = Vector2.Dot(ac, ab);
        var distance = Mathf.Clamp(dotProduct / ab.sqrMagnitude, 0, 1);

        return start + ab * distance;
    }

    public static Vector3 GetNearstToLine(Vector3 point, Vector3 start, Vector3 end)
    {
        var ac = point - start;
        var ab = end - start;
        var dotProduct = Vector3.Dot(ac, ab);
        var distance = Mathf.Clamp(dotProduct / ab.sqrMagnitude, 0, 1);

        return start + ab * distance;
    }

    public static Vector2 GetNearstToReactangle(Vector2 point, Vector2 rectangleMin, Vector2 rectangleMax)
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

    public static Vector2 GetNearstToReactangle(Vector3 point, Vector3 rectangleMin, Vector3 rectangleMax)
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

    public static void ProjectVertices(Vector2[] vertices, Vector2 axis,
                                        out float min, out float max)
    {
        min = float.MaxValue;
        max = float.MinValue;

        for (int i = 0; i < vertices.Length; i++)
        {
            var temp = Vector2.Dot(vertices[i], axis);
            if (min > temp)
            {
                min = temp;
            }

            if (max < temp)
            {
                max = temp;
            }
        }
    }

    public static void ProjectCircle(Vector2 center, float radius, Vector2 axis,
                                        out float min, out float max)
    {
        var dir = axis.normalized;
        min = Vector2.Dot(center - dir * radius, axis);
        max = Vector2.Dot(center + dir * radius, axis);

        if (min > max)
        {
            var t = max;
            max = min;
            min = t;
        }
    }

    public static int FindClosestPointOnPolygon(Vector2[] polygon, Vector2 point)
    {
        var result = -1;
        var distance = float.MaxValue;

        for (int i = 0; i < polygon.Length; i++)
        {
            var tempDist = Vector2.Distance(polygon[i], point);
            if (tempDist < distance)
            {
                result = i;
                distance = tempDist;
            }
        }

        return result;
    }

    public static T Next<T>(this T[] array, int index)
    {
        return array[(index++) % array.Length];
    }

    public static T Next<T>(this List<T> array, int index)
    {
        return array[(index++) % array.Count];
    }

    public static T Previous<T>(this T[] array, int index)
    {
        index--;
        while (index < 0)
        {
            index = array.Length + index;
        }

        return array[index];
    }

    public static T Previous<T>(this List<T> array, int index)
    {
        index--;
        while (index < 0)
        {
            index = array.Count + index;
        }

        return array[index];
    }
}