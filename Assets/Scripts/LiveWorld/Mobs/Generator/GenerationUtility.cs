using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace LiveWorld.Mobs
{
    public static class GenerationUtility
    {
        public static TriangleCollection Triangulation(IEnumerable<Vector3> shape)
        {
            List<Vector3> vertices = new List<Vector3>(shape);
            List<Triangle> triangles = new List<Triangle>();

            if (shape.Count() < 3)
            {
                return TriangleCollection.Empty;
            }

            triangles.Add(new Triangle(vertices[0], vertices[1], vertices[2]));

            //for (int index = 3; index < vertices.Count; index++)
            int index = 3;
            {
                var tempPoint = vertices[index];
                var tempTris = new List<Triangle>();

                foreach (Triangle triangle in triangles)
                {
                    if (triangle.IsViewedEdge(Triangle.Edge.AB, tempPoint)) 
                        tempTris.Add(new Triangle(tempPoint, triangle.a, triangle.b));

                    if (triangle.IsViewedEdge(Triangle.Edge.BC, tempPoint))
                        tempTris.Add(new Triangle(tempPoint, triangle.b, triangle.c));

                    if (triangle.IsViewedEdge(Triangle.Edge.CA, tempPoint))
                        tempTris.Add(new Triangle(tempPoint, triangle.c, triangle.a));
                }

                triangles.AddRange(tempTris);
            }

            return new TriangleCollection(triangles);
        }
        public static IEnumerable<Vector3> CreateShapePoints(MobConfiguration configuration, Skeleton skeleton)
        {
            float step = 0.1F;

            List<Vector3> vertices = new List<Vector3>();

            foreach (var joint in skeleton.GetJoints())
            {
                Vector3 center = joint.localPosition;

                for (float beta = 0; beta < 1.0F; beta += step)
                {
                    for (float alpha = 0; alpha < 1.0F; alpha += step)
                    {
                        float x = Mathf.Sin(alpha * Mathf.PI * 2) * Mathf.Cos(beta * Mathf.PI * 2);
                        float y = Mathf.Sin(alpha * Mathf.PI * 2) * Mathf.Sin(beta * Mathf.PI * 2);
                        float z = Mathf.Cos(alpha * Mathf.PI * 2);

                        Vector3 point = center + new Vector3(x, y, z);

                        vertices.Add(point);
                    }
                }
            }

            return vertices;
        }

        public static Mesh GetMesh(TriangleCollection triangulation)
        {
            List<int> triangles = new List<int>();
            List<Vector3> vertices = new List<Vector3>();

            foreach (Triangle triangle in triangulation)
            {
                triangles.Add(vertices.Count);
                triangles.Add(vertices.Count + 1);
                triangles.Add(vertices.Count + 2);

                vertices.Add(triangle.a);
                vertices.Add(triangle.b);
                vertices.Add(triangle.c);
            }

            Mesh mesh = new Mesh();

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }

        #region Math utility
        public static bool AngleMore90(Vector3 a, Vector3 b)
        {
            float temp = (a.magnitude * b.magnitude);

            if (temp == 0)
            {
                return false;
            }

            return (a.x * b.x + a.y * b.y + a.z * b.z) / temp < 0;
        }

        private static float Max(params float[] values)
        {
            return values.Max();
        }
        #endregion
    }
}
