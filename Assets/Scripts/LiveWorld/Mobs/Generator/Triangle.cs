using UnityEngine;

public enum Edge
{
    AB,
    BC,
    CA
}

public enum Orientation
{
    XY,
    YZ,
    XZ
}

public class Triangle
{
    public Vector3 a { get; }
    public Vector3 b { get; }
    public Vector3 c { get; }

    public Vector3 center { get; }

    public Vector3 normal { get; }

    public float AngleA { get; }
    public float AngleB { get; }
    public float AngleC { get; }

    public Vector3 edgeNormalAB { get; }
    public Vector3 edgeNormalBC { get; }
    public Vector3 edgeNormalCA { get; }


    public Triangle(Vector3 a, Vector3 b, Vector3 c)
    {
        this.a = a;
        this.b = b;
        this.c = c;

        center = (a + b + c) * 0.33F;
        normal = Vector3.Cross(c - a, c - b);

        edgeNormalAB = Vector3.Cross(normal, a - b);
        edgeNormalBC = Vector3.Cross(normal, b - c);
        edgeNormalCA = Vector3.Cross(normal, c - a);

        Vector3 ab_center = ((a + b) * 0.5F - center).normalized;
        Vector3 bc_center = ((b + c) * 0.5F - center).normalized;
        Vector3 ca_center = ((c + a) * 0.5F - center).normalized;

        if (MathUtility.AngleMore90(edgeNormalAB, ab_center))
            edgeNormalAB *= -1;

        if (MathUtility.AngleMore90(edgeNormalBC, bc_center))
            edgeNormalBC *= -1;

        if (MathUtility.AngleMore90(edgeNormalBC, ca_center))
            edgeNormalBC *= -1;

        AngleA = Vector3.Angle(-edgeNormalAB, -edgeNormalCA);
        AngleB = Vector3.Angle(-edgeNormalAB, -edgeNormalBC);
        AngleC = Vector3.Angle(-edgeNormalBC, -edgeNormalCA);
    }

    public bool Contains(Vector3 point)
    {
        float da = Vector3.Distance(a, point);
        float db = Vector3.Distance(b, point);
        float dc = Vector3.Distance(c, point);

        float _da = Vector3.Distance(a, (b + c) * 0.5F);
        float _db = Vector3.Distance(b, (a + c) * 0.5F);
        float _dc = Vector3.Distance(c, (a + b) * 0.5F);

        return
            da < _da &&
            db < _db &&
            dc < _dc;
    }

    public bool Contains(Vector3 point, Orientation orientation)
    {
        Vector2 a = Vector2.zero;
        Vector2 b = Vector2.zero;
        Vector2 c = Vector2.zero;

        switch (orientation)
        {
            case Orientation.XY:
                {
                    a = new Vector2(this.a.x, this.a.y);
                    b = new Vector2(this.b.x, this.b.y);
                    c = new Vector2(this.c.x, this.c.y);
                }
                break;
            case Orientation.XZ:
                {
                    a = new Vector2(this.a.x, this.a.z);
                    b = new Vector2(this.b.x, this.b.z);
                    c = new Vector2(this.c.x, this.c.z);
                }
                break;
            case Orientation.YZ:
                {
                    a = new Vector2(this.a.y, this.a.z);
                    b = new Vector2(this.b.y, this.b.z);
                    c = new Vector2(this.c.y, this.c.z);
                }
                break;
        }

        float da = Vector2.Distance(a, point);
        float db = Vector2.Distance(b, point);
        float dc = Vector2.Distance(c, point);

        float _da = Vector2.Distance(a, (b + c) * 0.5F);
        float _db = Vector2.Distance(b, (a + c) * 0.5F);
        float _dc = Vector2.Distance(c, (a + b) * 0.5F);

        return
            da < _da &&
            db < _db &&
            dc < _dc;
    }

    public Rect GetRect(Orientation orientation)
    {
        float a_x = 0, a_y = 0;
        float b_x = 0, b_y = 0;
        float c_x = 0, c_y = 0;

        switch (orientation)
        {
            case Orientation.XY:
                {
                    a_x = a.x;
                    a_y = a.y;

                    b_x = b.x;
                    b_y = b.y;

                    c_x = c.x;
                    c_y = c.y;
                }
                break;
            case Orientation.XZ:
                {
                    a_x = a.x;
                    a_y = a.z;

                    b_x = b.x;
                    b_y = b.z;

                    c_x = c.x;
                    c_y = c.z;
                }
                break;
            case Orientation.YZ:
                {
                    a_x = a.y;
                    a_y = a.z;

                    b_x = b.y;
                    b_y = b.z;

                    c_x = c.y;
                    c_y = c.z;
                }
                break;
        }

        float start_x = MathUtility.Min(a_x, b_x, c_x);
        float start_y = MathUtility.Min(a_y, b_y, c_y);

        float end_x = MathUtility.Max(a_x, b_x, c_x);
        float end_y = MathUtility.Max(a_y, b_y, c_y);

        return new Rect(start_x, start_y, end_x - start_x, end_y - start_y);
    }

    public Triangle[] Split(Vector3 point)
    {
        return new Triangle[]
            {
                new Triangle(a, b, point),
                new Triangle(b, c, point),
                new Triangle(c, a, point),
            };
    }

    public bool IsViewedEdge(Edge edge, Vector3 viewPoint)
    {
        Vector3 edgeNormal = Vector3.zero;
        Vector3 viewNormal = (center - viewPoint).normalized;

        switch(edge)
        {
            case Edge.AB: edgeNormal = edgeNormalAB; break;
            case Edge.BC: edgeNormal = edgeNormalBC; break;
            case Edge.CA: edgeNormal = edgeNormalCA; break;
        }

        return !MathUtility.AngleMore90(edgeNormal, viewNormal);
    }
}
