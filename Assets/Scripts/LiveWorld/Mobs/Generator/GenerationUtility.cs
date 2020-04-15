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

            

            return new TriangleCollection(triangles);
        }

        public static IEnumerable<Vector3> CreateShapePoints(Skeleton skeleton)
        {
            float step = 0.1F;

            List<Vector3> vertices = new List<Vector3>();
            List<MobJoint> joints = new List<MobJoint>(skeleton.GetJoints());

            foreach (var joint in joints)
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

                        if (joints.Find(_x => _x != joint && Vector3.Distance(point, _x.localPosition) < 1) == null &&
                            vertices.Find(_x => _x == point) == default)
                        {
                            vertices.Add(point);
                        }
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

        public static Skeleton SkeletonFromTransform(Transform transform)
        {
            Skeleton skeleton = new Skeleton();

            foreach (var joint in transform.GetComponentsInChildren<Transform>())
            {
                if (joint == transform)
                {
                    continue;
                }

                Vector3 position = joint.position - transform.position;

                skeleton.AddJoint(new MobJoint(joint.name, position, joint.name.Contains("Foot")));

                if (joint.parent != transform)
                {
                    float boneLength = Vector3.Distance(joint.parent.transform.position, joint.transform.position);

                    Bone bone = new Bone(boneLength, joint.parent.name, joint.name);

                    skeleton.AddBone(bone);
                }
            }

            return skeleton;
        }
    }
}
