using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TriangleCollection : IEnumerable<Triangle>
{
    public static TriangleCollection Empty
    {
        get => new TriangleCollection();
    }

    private List<Triangle> triangles;

    public TriangleCollection()
    {
        this.triangles = new List<Triangle>();
    }

    public TriangleCollection(IEnumerable<Triangle> triangles)
    {
        this.triangles = triangles.ToList();
    }


    public void AddTriangle(Triangle triangle)
    {
        triangles.Add(triangle);
    }

    public IEnumerator<Triangle> GetEnumerator()
    {
        return triangles.GetEnumerator() as IEnumerator<Triangle>;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return triangles.GetEnumerator();
    }
}