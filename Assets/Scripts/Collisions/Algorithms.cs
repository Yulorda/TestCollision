
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public static class Algorithms
{
    public static List<Vector2> GetConvexHull(List<Vector2> points)
    {
        if (points.Count <= 3)
        {
            return points;
        }

        var convexHull = new List<Vector2>();
        var startVertex = points[0];

        for (int i = 1; i < points.Count; i++)
        {
            if (points[i].x < startVertex.x || Mathf.Abs(points[i].x - startVertex.x) < Utills.EPSILON && points[i].y < startVertex.y)
            {
                startVertex = points[i];
            }
        }

        convexHull.Add(startVertex);
        points.Remove(startVertex);

        var currentPoint = convexHull[0];
        var colinearPoints = new List<Vector2>();

        while (points.Count > 0)
        {
            var index = Random.Range(0, points.Count);
            var nextPoint = points[index];

            for (int i = 0; i < points.Count; i++)
            {
                if (i == index)
                {
                    continue;
                }

                var c = points[i];

                float relation = Vector2.Dot(c, nextPoint - currentPoint);

                if (relation < Utills.EPSILON && relation > -Utills.EPSILON)
                {
                    colinearPoints.Add(points[i]);
                }
                else if (relation < 0f)
                {
                    nextPoint = points[i];
                    index = i;

                    colinearPoints.Clear();
                }
            }

            if (colinearPoints.Count > 0)
            {
                colinearPoints.Add(nextPoint);
                colinearPoints = colinearPoints.OrderBy(n => Vector3.SqrMagnitude(n - currentPoint)).ToList();
                convexHull.AddRange(colinearPoints);
                currentPoint = colinearPoints[colinearPoints.Count - 1];

                for (int i = 0; i < colinearPoints.Count; i++)
                {
                    points.Remove(colinearPoints[i]);
                }

                colinearPoints.Clear();
            }
            else
            {
                convexHull.Add(nextPoint);
                points.Remove(nextPoint);
                currentPoint = nextPoint;
            }
        }

        return convexHull;
    }

    public static int[] GetTrianglesForConvex(Vector2[] convexHull)
    {
        var result = new int[(convexHull.Length - 2) * 3];

        var index = 0;
        for (int i = 2; i < convexHull.Length; i++)
        {
            result[index++] = 0;
            result[index++] = i - 1;
            result[index++] = i;
        }

        return result;
    }

    public static int[] GetTrianglesForConcave(Vector2[] concavePolygon)
    {
        var resultTriangleVertCount = (concavePolygon.Length - 2) * 3;
        var indexList = new List<int>(resultTriangleVertCount);
        var result = new int[resultTriangleVertCount];
        var currentIndex = 0;

        for (int i = 0; i < concavePolygon.Length; i++)
        {
            indexList.Add(i);
        }

        while (indexList.Count > 3)
        {
            for (int i = 0; i < indexList.Count; i++)
            {
                int a = indexList[i];
                int b = indexList.Next(i);
                int c = indexList.Previous(i);

                var va = concavePolygon[a];
                var vb = concavePolygon[b];
                var vc = concavePolygon[c];

                if (Utills.Cross(vb - va, vc - va) < 0)
                {
                    continue;
                }

                var complete = true;
                for (int j = 0; j < concavePolygon.Length; j++)
                {
                    if (j == a || j == b || j == c)
                    {
                        continue;
                    }

                    if (Collisions.PointToTriangle(concavePolygon[j], va, vb, vc))
                    {
                        complete = false;
                        break;
                    }
                }

                if (complete)
                {
                    result[currentIndex++] = a;
                    result[currentIndex++] = b;
                    result[currentIndex++] = c;

                    indexList.RemoveAt(i);
                }
            }
        }

        result[currentIndex++] = indexList[0];
        result[currentIndex++] = indexList[1];
        result[currentIndex] = indexList[2];

        return result;
    }


}
