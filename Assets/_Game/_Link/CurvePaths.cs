using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvePaths 
{
    //tinh path duong cong spiral
    //start : diem dau
    //finish : diem ket thuc
    //stepR : khoang tang cua ban kinh r
    //stepAngle : khoang cach tang giua moi vong
    public static List<Vector3> SpiralExtend(Vector3 start, Vector3 finish, float stepR, float stepAngle)
    {
        List<Vector3> path = new List<Vector3>();

        Vector3 ab = finish - start;
        float distance = Vector3.Distance(ab, Vector3.zero);
        float angle = Mathf.Atan2(ab.z, ab.x);
        float x, z, r = 0;
        Vector3 point = start;

        float space = Mathf.Sqrt(2 * distance * distance * (1 - Mathf.Cos(stepAngle)));

        float dt = stepR;
        while (Vector3.Distance(point, finish) > 0.1f)
        {
            if (r < distance)
            {
                r += dt;
                if (r > distance)
                {
                    r = distance;
                }
            }
            else
            {
                if (Vector3.Distance(point, finish) <= space)
                {
                    path.Add(finish);
                    break;
                }
            }

            angle += stepAngle;

            x = r * Mathf.Cos(angle);
            z = r * Mathf.Sin(angle);

            point = new Vector3(x, 0, z) + start;

            path.Add(point);
        }

        return path;

    }

    public static List<Vector3> SpiralNarrow(Vector3 start, Vector3 finish, float stepR, float stepAngle)
    {
        List<Vector3> path = SpiralExtend(finish, start, stepR, stepAngle);
        path.Reverse();
        return path;
    }

}
