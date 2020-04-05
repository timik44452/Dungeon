using UnityEngine;
using LiveWorld.Mobs;

public struct Triangle
{
    public enum Edge
    { 
        AB,
        BC,
        CA
    }

    public Vector3 a;
    public Vector3 b;
    public Vector3 c;

    public Vector3 center { get; }

    public Vector3 normal { get; }

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

        if (GenerationUtility.AngleMore90(edgeNormalAB, ab_center))
            edgeNormalAB *= -1;

        if (GenerationUtility.AngleMore90(edgeNormalBC, bc_center))
            edgeNormalBC *= -1;

        if (GenerationUtility.AngleMore90(edgeNormalBC, ca_center))
            edgeNormalBC *= -1;
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

        return !GenerationUtility.AngleMore90(edgeNormal, viewNormal);
    }
}
