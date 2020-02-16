
//Calculates the distance to a 3d Bezier curve defined by 3 control points
//For Unity.

//Adapted from iq
//https://www.shadertoy.com/view/ldj3Wh
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierSDF : MonoBehaviour
{



    Vector2 sdBezier(Vector3 A, Vector3 B, Vector3 C, Vector3 pos)
    {
        Vector3 a = B - A;
        Vector3 b = A - 2.0f * B + C;
        Vector3 c = a * 2.0f;
        Vector3 d = A - pos;

        float kk = 1.0f / Vector3.Dot(b, b);
        float kx = kk * Vector3.Dot(a, b);
        float ky = kk * (2.0f * Vector3.Dot(a, a) + Vector3.Dot(d, b)) / 3.0f;
        float kz = kk * Vector3.Dot(d, a);

        Vector2 res;

        float p = ky - kx * kx;
        float p3 = p * p * p;
        float q = kx * (2.0f * kx * kx - 3.0f * ky) + kz;
        float h = q * q + 4.0f * p3;

        if (h >= 0.0)
        {
            h = Mathf.Sqrt(h);
            Vector2 x = (new Vector2(h, -h) - Vector2.one * q) / 2.0f;
            Vector2 cwSign_x = new Vector2(Mathf.Sign(x.x), Mathf.Sign(x.y));
            Vector2 cwPowAbs_x = new Vector2(Mathf.Pow(Mathf.Abs(x.x), 0.33333333333f), Mathf.Pow(Mathf.Abs(x.y), 0.33333333333f));
            Vector2 uv = Vector2.Scale(cwSign_x,cwPowAbs_x);
            float t = uv.x + uv.y - kx;
            t = Mathf.Clamp01(t);

            // 1 root
            Vector3 qos = d + (c + b * t) * t;
            res = new Vector2(Vector3.Magnitude(qos), t);
        }
        else
        {
            float z = Mathf.Sqrt(-p);
            float v = Mathf.Acos(q / (p * z * 2.0f)) / 3.0f;
            float m = Mathf.Cos(v);
            float n = Mathf.Sin(v) * 1.732050808f;
            Vector3 t = new Vector3(m + m, -n - m, n - m) * z - Vector3.one * kx;
            t = new Vector3(Mathf.Clamp01(t.x), Mathf.Clamp01(t.y), Mathf.Clamp01(t.z));


            // 3 roots
            Vector3 qos = d + (c + b * t.x) * t.x;
            float dis = Vector3.Dot(qos, qos);

            res = new Vector2(dis, t.x);

            qos = d + (c + b * t.y) * t.y;
            dis = Vector3.Dot(qos, qos);
            if (dis < res.x) res = new Vector2(dis, t.y);

            qos = d + (c + b * t.z) * t.z;
            dis = Vector3.Dot(qos, qos);
            if (dis < res.x) res = new Vector2(dis, t.z);

            res.x = Mathf.Sqrt(res.x);
        }

        return res;
    }

}





