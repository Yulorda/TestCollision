
using System.Linq;

using UnityEditor.Rendering;

using UnityEngine;

public static class Collisions
{
    public const float EPSILON = 1e-10f;

    public static bool PointToPoint(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) <= EPSILON;
    }

    public static bool PointToPoint(Vector2 a, Vector2 b)
    {
        return Vector2.SqrMagnitude(a - b) <= EPSILON;
    }

    public static bool PointToCircle(Vector2 a, Vector2 center, float radius)
    {
        return Vector2.Distance(a, center) <= radius;
        //А что будет быстрее ?
        //return Vector2.SqrMagnitude(a - center) <= Mathf.Pow(radius,2);
    }

    public static bool PointToSphere(Vector3 a, Vector3 center, float radius)
    {
        return Vector2.Distance(a, center) <= radius;
        //А что будет быстрее ?
        //return Vector3.SqrMagnitude(a - center) <= Mathf.Pow(radius,2);
    }

    public static bool CircleToCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB)
    {
        return PointToCircle(centerA, centerB, radiusA + radiusB);
        //return Vector2.Distance(centerA, centerB) <= radiusA + radiusB;
    }

    public static bool SphereToSphere(Vector3 centerA, float radiusA, Vector3 centerB, float radiusB)
    {
        return PointToSphere(centerA, centerB, radiusA + radiusB);
        //return Vector2.Distance(centerA, centerB) <= radiusA + radiusB;
    }

    public static bool PointToRectangle(Vector2 a, Vector2 rectangleMin, Vector2 rectangleMax)
    {
        return a.x >= rectangleMin.x && a.x <= rectangleMax.x &&
            a.y >= rectangleMin.y && a.y <= rectangleMax.y;
    }

    public static bool PointToСuboid(Vector3 a, Vector3 cuboidMin, Vector3 cuboidMax)
    {
        return a.x >= cuboidMin.x && a.x <= cuboidMax.x &&
            a.y >= cuboidMin.y && a.y <= cuboidMax.y &&
            a.z >= cuboidMin.z && a.z <= cuboidMax.z;
    }

    public static bool RectangleToRectangle(Vector2 rectangleMinA, Vector2 rectangleMaxA, Vector2 rectangleMinB, Vector2 rectangleMaxB)
    {
        return rectangleMaxA.x >= rectangleMinB.x &&
            rectangleMinA.x <= rectangleMaxB.x &&
            rectangleMaxA.y >= rectangleMinB.y &&
            rectangleMinA.y <= rectangleMaxB.y;
    }

    public static bool СuboidToСuboid(Vector3 cuboidMinA, Vector3 cuboidMaxA, Vector3 cuboidMinB, Vector3 cuboidMaxB)
    {
        return cuboidMaxA.x >= cuboidMinB.x &&
            cuboidMinA.x <= cuboidMaxB.x &&
            cuboidMaxA.y >= cuboidMinB.y &&
            cuboidMinA.y <= cuboidMaxB.y &&
            cuboidMaxA.z >= cuboidMinB.z &&
            cuboidMinA.z <= cuboidMaxB.z;
    }

    public static bool CircleToRectangle(Vector2 center, float radius, Vector2 rectangleMin, Vector2 rectangleMax)
    {
        return PointToCircle(GetNearstToReactangle(center, rectangleMin, rectangleMax), center, radius);
    }

    public static bool SphereToCuboid(Vector3 center, float radius, Vector3 rectangleMin, Vector3 rectangleMax)
    {
        return PointToSphere(GetNearstToReactangle(center, rectangleMin, rectangleMax), center, radius);
    }

    public static bool PointOnLine(Vector3 a, Vector3 start, Vector3 end)
    {
        var ab = end - start;
        var ac = a - start;
        var cb = a - end;

        var abLength = ab.magnitude;
        var accbLength = ac.magnitude + cb.magnitude;

        return accbLength >= abLength - EPSILON && accbLength <= abLength + EPSILON;
    }

    public static bool PointOnLine(Vector2 a, Vector2 start, Vector2 end)
    {
        var ab = end - start;
        var ac = a - start;
        var cb = a - end;

        var abLength = ab.magnitude;
        var accbLength = ac.magnitude + cb.magnitude;

        return accbLength >= abLength - EPSILON && accbLength <= abLength + EPSILON;
    }

    public static bool CircleToLine(Vector2 center, float radius, Vector2 start, Vector2 end)
    {
        if (PointToCircle(start, center, radius) || PointToCircle(end, center, radius))
        {
            return true;
        }

        var ac = center - start;
        var ab = end - start;
        var dotProduct = Vector2.Dot(ac, ab);
        var distance = dotProduct / ab.sqrMagnitude;

        if (distance >= 1 && distance <= 0)
            return false;

        return PointToCircle(start + ab * distance, center, radius);
        //Проще читать
        //return PointToCircle(GetNearstToLine(center, start, end), center, radius);
    }

    public static bool SphereToLine(Vector3 center, float radius, Vector3 start, Vector3 end)
    {
        if (PointToSphere(start, center, radius) || PointToSphere(end, center, radius))
        {
            return true;
        }

        var ac = center - start;
        var ab = end - start;
        var dotProduct = Vector3.Dot(ac, ab);
        var distance = dotProduct / ab.sqrMagnitude;

        if (distance >= 1 && distance <= 0)
            return false;

        return PointToSphere(start + ab * distance, center, radius);
        //return PointToSphere(GetNearstToLine(center, start, end), center, radius);
    }

    public static bool LineToLine(Vector2 startA, Vector2 endA, Vector2 startB, Vector2 endB)
    {
        var ab = endA - startA;
        var cd = endB - startB;
        var abcdCross = Cross(ab, cd);

        var ac = startB - startA;

        var uA = Cross(cd, ac) / abcdCross;
        var uB = Cross(ab, ac) / abcdCross;

        return uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1;

        float Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }
    }

    public static bool LineToRectangle(Vector2 start, Vector2 end, Vector2 min, Vector2 max)
    {
        var point2 = new Vector2(min.x, max.y);
        var point4 = new Vector2(max.x, min.y);

        return LineToLine(start, end, min, point2) ||
            LineToLine(start, end, point2, max) ||
            LineToLine(start, end, point4, max) ||
            LineToLine(start, end, min, point4);
    }

    //http://paulbourke.net/geometry/pointlineplane/
    public static bool LineToLine(Vector3 startA, Vector3 endA, Vector3 startB, Vector3 endB)
    {
        var ab = endA - startA;
        var cd = endB - startB;
        var ca = startA - startB;

        var cacdDot = Vector3.Dot(ca, cd);
        var cdabDot = Vector3.Dot(cd, ab);
        var caabDot = Vector3.Dot(ca, ab);
        var cdDot = Vector3.Dot(cd, cd);
        var abDot = Vector3.Dot(ab, ab);

        var temp = (abDot * cdDot - cdabDot * cdabDot);

        if (temp == 0)
            return true;

        var muA = (cacdDot * cdabDot - caabDot * cdDot) / temp;
        var muB = (cacdDot + muA * cdabDot) / cdDot;

        return (startA - startB + muA * (endA - startA) - muB * (endB - startB)).magnitude <= EPSILON;
    }

    public static bool LineToPlane(Vector3 start, Vector3 end, Vector3 normal, Vector3 planePoint)
    {
        var denominator = Vector3.Dot(normal, end - start);

        if (denominator == 0)
        {
            var temp = start - planePoint;
            return temp.x < EPSILON || temp.y < EPSILON || temp.z < EPSILON;
        }

        var u = Vector3.Dot(normal, planePoint - start) / denominator;
        return u >= 0 && u <= 1;
    }


    public static bool LineToDisk(Vector3 start, Vector3 end, Vector3 normal, Vector3 diskCenter, float radius)
    {
        var direction = end - start;
        var denominator = Vector3.Dot(normal, direction);

        if (denominator == 0)
        {
            var temp = start - diskCenter;

            if (temp.x < EPSILON)
            {
                var start2 = new Vector2(start.y, start.z);
                var end2 = new Vector2(end.y, end.z);

                var circleCenter = new Vector2(diskCenter.y, diskCenter.z);

                return CircleToLine(circleCenter, radius, start2, end2);
            }
            else if (temp.y < EPSILON)
            {
                var start2 = new Vector2(start.z, start.z);
                var end2 = new Vector2(end.x, end.z);

                var circleCenter = new Vector2(diskCenter.x, diskCenter.z);

                return CircleToLine(circleCenter, radius, start2, end2);
            }
            else if (temp.z < EPSILON)
            {
                var start2 = new Vector2(start.x, start.y);
                var end2 = new Vector2(end.x, end.y);

                var circleCenter = new Vector2(diskCenter.x, diskCenter.y);

                return CircleToLine(circleCenter, radius, start2, end2);
            }
            else
            {
                return false;
            }
        }

        var u = Vector3.Dot(normal, diskCenter - start) / denominator;

        if (u >= 0 && u <= 1)
        {
            var pointInPlane = start + direction * u;
            var delta = pointInPlane - diskCenter;
            return (delta.magnitude <= radius);
        }

        return false;
    }

    public static bool RayToCuboid(Vector3 start, Vector3 direction, Vector3 min, Vector3 max)
    {
        float txmin = (min.x - start.x) / direction.x;
        float txmax = (max.x - start.x) / direction.x;

        if (txmin > txmax) 
        {
            Swap(ref txmin, ref txmax);
        } 

        float tymin = (min.y - start.y) / direction.y;
        float tymax = (max.y - start.y) / direction.y;

        if (tymin > tymax)
        {
            Swap(ref tymin, ref tymax);
        }

        if ((txmin > tymax) || (tymin > txmax))
            return false;

        if (tymin > txmin)
            txmin = tymin;

        if (tymax < txmax)
            txmax = tymax;

        float tzmin = (min.z - start.z) / direction.z;
        float tzmax = (max.z - start.z) / direction.z;

        if (tzmin > tzmax)
        {
            Swap(ref tzmin, ref tzmax);
        }

        if ((txmin > tzmax) || (tzmin > txmax))
            return false;

        if (tzmin > txmin)
            txmin = tzmin;

        if (tzmax < txmax)
            txmax = tzmax;

        return true;

        void Swap(ref float min, ref float max)
        {
            var temp = max;
            max = min;
            min = temp;
        }
    }

    public static bool PointToPolygon(Vector2 point, Vector2[] polygon)
    {
        var collision = false;
        for (int i = 0; i < polygon.Length; i++)
        {
            var current = polygon[i];
            var next = Next(polygon, i);
            //Определяем находится ли искомая точка, между двумя точками полигона по оси Y
            //Переключаем состояния колизии если точка находится с одной стороны по X
            //В случае, если точка будет находится за полигоном, то другой полигон, тоже переключит состояние колизии
            //А если будет находится с другой стороны, то колизия отработает 1 раз.
            //В сложных случаях, будет происходить подсчет количества пересечений , если он будет четный, то коллизии нет.
            //                                                              a + dir * cnpr(p)
            if (((current.y > point.y) != (next.y > point.y)) && (point.x < current.x + (next.x - current.x) * (point.y - current.y) / (next.y - current.y)))
            {
                collision = !collision;
            }
        }

        return collision;
    }

    public static bool PointToTriangle(Vector2 point, Vector2[] triangle)
    {
        var ab = triangle[1] - triangle[0];
        var ac = triangle[2] - triangle[0];

        var area = Mathf.Abs(ab.x * ac.y - ab.y * ac.x);

        var ap = point - triangle[0];
        var bp = point - triangle[1];
        var cp = point - triangle[2];


        //Мб завести cross ?
        var area1 = Mathf.Abs(ap.x * cp.y - ap.y * cp.x);
        var area2 = Mathf.Abs(ap.x * bp.y - ap.y * bp.y);
        var area3 = Mathf.Abs(cp.x * bp.y - cp.y * bp.x);

        return area1 + area2 + area3 == area;
    }

    public static bool PolygonToPolygon(Vector2[] polygonA, Vector2[] polygonB)
    {
        for (int i = 0; i < polygonA.Length; i++)
        {
            var current = polygonA[i];
            var next = Next(polygonA, i);

            if (LineToPolygon(current, next, polygonB))
            {
                return true;
            }
        }

        return PointToPolygon(polygonB[0], polygonA) || PointToPolygon(polygonA[0], polygonB);
    }

    public static bool LineToPolygon(Vector2 start, Vector2 end, Vector2[] polygon)
    {
        for (int i = 0; i < polygon.Length; i++)
        {
            var current = polygon[i];
            var next = Next(polygon, i);

            if (LineToLine(start, end, current, next))
            {
                return true;
            }
        }

        return PointToPolygon(start, polygon);
    }

    public static bool CircleToConvexPolygon(Vector2 center, float radius, Vector2[] polygon)
    {
        for (int i = 0; i < polygon.Length; i++)
        {
            var current = polygon[i];
            var next = Next(polygon, i);

            if (CircleToLine(center, radius, current, next))
            {
                return true;
            }
        }

        return PointToPolygon(center, polygon);
    }

    public static bool RectangleToPolygon(Vector2 min, Vector2 max, Vector2[] polygon)
    {
        for (int i = 0; i < polygon.Length; i++)
        {
            var current = polygon[i];
            var next = Next(polygon, i);

            if (LineToRectangle(current, next, min, max))
            {
                return true;
            }
        }

        return (PointToPolygon(min, polygon) || PointToRectangle(polygon[0], min, max));
    }


    //https://www.youtube.com/watch?v=vWs33LVrs74&ab_channel=Two-BitCoding
    public static bool CircleToConvexPolygon2(Vector2 center, float radius, Vector2[] polygon)
    {
        var normal = Vector2.zero;
        var depth = float.MaxValue;
        var axis = Vector2.zero;
        var axisDepth = float.MinValue;
        float minA, minB, maxA, maxB;

        for (int i = 0; i < polygon.Length; i++)
        {
            var current = polygon[i];
            var next = Next(polygon, i);

            var edge = next - current;
            axis = new Vector2(-edge.y, edge.x);

            ProjectVertices(polygon, axis, out minA, out maxA);
            ProjectCircle(center, radius, axis, out minB, out maxB);

            if (minA >= maxB || minB >= maxA)
            {
                return false;
            }

            axisDepth = Mathf.Min(maxB - minA, maxA - minB);

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        var closestIndex = FindClosestPointOnPolygon(polygon, center);
        var closestPoint = polygon[closestIndex];
        axis = closestPoint - center;

        ProjectVertices(polygon, axis, out minA, out maxA);
        ProjectCircle(center, radius, axis, out minB, out maxB);

        if (minA >= maxB || minB >= maxA)
        {
            return false;
        }

        return true;

        //В примере он искал дельту для выталкивания, я пока таким не увлекаюсь

        axisDepth = Mathf.Min(maxB - minA, maxA - minB);

        if (axisDepth < depth)
        {
            depth = axisDepth;
            normal = axis;
        }

        depth /= normal.magnitude;
        normal = normal.normalized;

        var polygonCenter = polygon.Aggregate((x, y) => x + y) / polygon.Length;
        var direction = polygonCenter - center;
        if (Vector2.Dot(direction, normal) < 0f)
        {
            normal = -normal;
        }

        return true;
    }

    private static int FindClosestPointOnPolygon(Vector2[] polygon, Vector2 point)
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

    private static Vector2 Next(Vector2[] array, int index)
    {
        return array[(index + 1) % array.Length];
    }

    private static void ProjectVertices(Vector2[] vertices, Vector2 axis,
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

    private static void ProjectCircle(Vector2 center, float radius, Vector2 axis,
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

    //Lengyel_E_-_Mathematics_for_3D_Game_Programmin
    //public static bool LineToLine2(Vector3 startA, Vector3 endA, Vector3 startB, Vector3 endB)
    //{
    //    var directionA = (endA - startA).normalized;
    //    var directionB = (endB - startB).normalized;
    //    Matrix<float> A = Matrix<float>.Build.DenseOfArray(
    //        new float[,] { { Vector3.Dot(directionA, directionA), Vector3.Dot(-directionA, directionB) },
    //                       { Vector3.Dot(directionA, directionB), Vector3.Dot(directionB, directionB) } }).Transpose();

    //    Matrix<float> B = Matrix<float>.Build.DenseOfArray(
    //        new float[,] { { Vector3.Dot(startB - startA,directionA)},
    //                       { Vector3.Dot(startB - startA,directionB)}});

    //    var result = A.Solve(B).ToArray();
    //    var t1 = result[0, 0];
    //    var t2 = result[0, 1];

    //    if (t1 >= 0 && t1 <= 1 && t2 >= 0 && t2 <= 1)
    //    {
    //        var point1 = directionA * t1 + startA;    
    //        var point2 = directionB * t2 + startB;

    //        if (Vector3.Distance(point1, point2) < EPSILON)
    //            return true;
    //    }

    //    return false;
    //}






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
}
