using System.Linq;

using UnityEngine;
using UnityEngine.Diagnostics;

public static class Collisions
{
    public static bool PointToPoint(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) <= Utills.EPSILON;
    }

    public static bool PointToPoint(Vector2 a, Vector2 b)
    {
        return Vector2.SqrMagnitude(a - b) <= Utills.EPSILON;
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
        return PointToCircle(Utills.GetNearstToReactangle(center, rectangleMin, rectangleMax), center, radius);
    }

    public static bool SphereToCuboid(Vector3 center, float radius, Vector3 rectangleMin, Vector3 rectangleMax)
    {
        return PointToSphere(Utills.GetNearstToReactangle(center, rectangleMin, rectangleMax), center, radius);
    }

    public static bool PointOnLine(Vector3 a, Vector3 start, Vector3 end)
    {
        var ab = end - start;
        var ac = a - start;
        var cb = a - end;

        var abLength = ab.magnitude;
        var accbLength = ac.magnitude + cb.magnitude;

        return accbLength >= abLength - Utills.EPSILON && accbLength <= abLength + Utills.EPSILON;
    }

    public static bool PointOnLine(Vector2 a, Vector2 start, Vector2 end)
    {
        var ab = end - start;
        var ac = a - start;
        var cb = a - end;

        var abLength = ab.magnitude;
        var accbLength = ac.magnitude + cb.magnitude;

        return accbLength >= abLength - Utills.EPSILON && accbLength <= abLength + Utills.EPSILON;
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
        var abcdCross = Utills.Cross(ab, cd);

        var ac = startB - startA;

        var uA = Utills.Cross(cd, ac) / abcdCross;
        var uB = Utills.Cross(ab, ac) / abcdCross;

        return uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1;
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

        return (startA - startB + muA * (endA - startA) - muB * (endB - startB)).magnitude <= Utills.EPSILON;
    }

    public static bool LineToPlane(Vector3 start, Vector3 end, Vector3 normal, Vector3 planePoint)
    {
        var denominator = Vector3.Dot(normal, end - start);

        if (denominator < Utills.EPSILON && denominator > -Utills.EPSILON)
        {
            var temp = start - planePoint;
            return temp.x < Utills.EPSILON && temp.x > -Utills.EPSILON || temp.y < Utills.EPSILON && temp.y > -Utills.EPSILON || temp.z < Utills.EPSILON && temp.z > -Utills.EPSILON;
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

            if (temp.x < Utills.EPSILON && temp.x > -Utills.EPSILON)
            {
                var start2 = new Vector2(start.y, start.z);
                var end2 = new Vector2(end.y, end.z);

                var circleCenter = new Vector2(diskCenter.y, diskCenter.z);

                return CircleToLine(circleCenter, radius, start2, end2);
            }
            else if (temp.y < Utills.EPSILON && temp.y > -Utills.EPSILON)
            {
                var start2 = new Vector2(start.z, start.z);
                var end2 = new Vector2(end.x, end.z);

                var circleCenter = new Vector2(diskCenter.x, diskCenter.z);

                return CircleToLine(circleCenter, radius, start2, end2);
            }
            else if (temp.z < Utills.EPSILON && temp.z > -Utills.EPSILON)
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
            var next = polygon.Next(i);
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

        var area = Mathf.Abs(Utills.Cross(ab, ac));

        var ap = point - triangle[0];
        var bp = point - triangle[1];
        var cp = point - triangle[2];

        var area1 = Mathf.Abs(Utills.Cross(ap, cp));
        var area2 = Mathf.Abs(Utills.Cross(ap, bp));
        var area3 = Mathf.Abs(Utills.Cross(cp, bp));

        return area1 + area2 + area3 == area;
    }

    public static bool PointToTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
    {
        var ab = b - a;
        var ac = c - a;

        var area = Mathf.Abs(Utills.Cross(ab, ac));

        var ap = point - a;
        var bp = point - b;
        var cp = point - c;

        var area1 = Mathf.Abs(Utills.Cross(ap, cp));
        var area2 = Mathf.Abs(Utills.Cross(ap, bp));
        var area3 = Mathf.Abs(Utills.Cross(cp, bp));

        return area1 + area2 + area3 == area;
    }

    public static bool PolygonToPolygon(Vector2[] polygonA, Vector2[] polygonB)
    {
        for (int i = 0; i < polygonA.Length; i++)
        {
            var current = polygonA[i];
            var next = polygonA.Next(i);

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
            var next = polygon.Next(i);

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
            var next = polygon.Next(i);

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
            var next = polygon.Next(i);

            if (LineToRectangle(current, next, min, max))
            {
                return true;
            }
        }

        return (PointToPolygon(min, polygon) || PointToRectangle(polygon[0], min, max));
    }


    //https://www.youtube.com/watch?v=vWs33LVrs74&ab_channel=Two-BitCoding
    //Судя по всему по принципу SAT
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
            var next = polygon.Next(i);

            var edge = next - current;
            axis = new Vector2(-edge.y, edge.x);

            Utills.ProjectVertices(polygon, axis, out minA, out maxA);
            Utills.ProjectCircle(center, radius, axis, out minB, out maxB);

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

        var closestIndex = Utills.FindClosestPointOnPolygon(polygon, center);
        var closestPoint = polygon[closestIndex];
        axis = closestPoint - center;

        Utills.ProjectVertices(polygon, axis, out minA, out maxA);
        Utills.ProjectCircle(center, radius, axis, out minB, out maxB);

        if (minA >= maxB || minB >= maxA)
        {
            return false;
        }

        return true;

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

    //SAT The Separating Axis Theorem
    //Наверное стоит передвать в качестве параметров форму как интерфейс, у которого есть функций получения проекции на ось
    //И функции получения осей из формы
    //игнорируется Containment (когда фигура внутри другой фигуры)
    public static bool SATPolygonToPolygon(Vector2[] polygonA, Vector2[] polygonB)
    {
        return SatPolygonPart(polygonA, polygonB) && SatPolygonPart(polygonB, polygonA);

        bool SatPolygonPart(Vector2[] polygonA, Vector2[] polygonB)
        {
            //Множественное число axis ))) 
            var axes = new Vector2[polygonA.Length];
            for (int i = 0; i < polygonA.Length; i++)
            {
                var current = polygonA[i];
                var next = polygonA.Next(i);

                var edge = next - current;
                axes[i] = new Vector2(-edge.y, edge.x);
            }

            for (int i = 0; i < axes.Length; i++)
            {
                if (!CheckProjectionOverlap(axes[i], polygonA, polygonB))
                {
                    return false;
                }
            }

            return true;
        }
    }

    private static bool CheckProjectionOverlap(Vector2 axis, Vector2[] polygonA, Vector2[] polygonB)
    {
        Utills.ProjectVertices(polygonA, axis, out var minA, out var maxA);
        Utills.ProjectVertices(polygonB, axis, out var minB, out var maxB);

        return minA >= maxB || minB >= maxA;
    }

    //При изучении Sat наткнулся на такой пример, ну по описаню SAT тут нету осей и нету проекций.
    //Проверяет находятся ли все точки одного полигона с одной стороны отностельно каждой стороны другого полигона
    //Сразу можно заметить что, если полигон находится внутри другого полигона, то такой алгоритм не отработает.
    //https://github.com/youlanhai/learn-physics/blob/master/Assets/02-sat/SAT.cs
    public static bool PolygonCollision(Vector2[] polygonA, Vector2[] polygonB)
    {
        return PolygonCollisionPart(polygonA, polygonB) && PolygonCollisionPart(polygonB, polygonA);
    }

    private static bool PolygonCollisionPart(Vector2[] polygonA, Vector2[] polygonB)
    {
        var side = Utills.Side(polygonA[0], polygonA[1], polygonA[2]);
        for (int i = 0; i < polygonA.Length; i++)
        {
            var current = polygonA[i];
            var next = polygonA.Next(i);

            if (IsDiferentSide(side, current, next, polygonB))
            {
                return false;
            }
        }
        return true;
    }

    private static bool IsDiferentSide(int currentSide, Vector2 start, Vector2 end, Vector2[] polygon)
    {
        for (int j = 0; j < polygon.Length; j++)
        {
            var tempSide = Utills.Side(start, end, polygon[j]);
            if (tempSide * currentSide > 0)
            {
                return false;
            }
        }
        return true;
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
}
