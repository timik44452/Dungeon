using System.Linq;

using UnityEngine;

public static class MathUtility
{
    public static bool AngleMore90(Vector3 a, Vector3 b)
    {
        float temp = (a.magnitude * b.magnitude);

        if (temp == 0)
        {
            return false;
        }

        return (a.x * b.x + a.y * b.y + a.z * b.z) / temp < 0;
    }

    public static Vector3 GetSpherePoint(float radius, float alpha, float beta)
    {
        float sin = Mathf.Sin(alpha * Mathf.PI * 2);
        float cos = Mathf.Cos(beta * Mathf.PI * 2);

        return new Vector3(
            radius * sin * cos,
            radius * sin * sin,
            radius * cos);
    }

    public static float Min(params float[] values)
    {
        return values.Min();
    }

    public static float Max(params float[] values)
    {
        return values.Max();
    }
}