using System.Linq;
using UnityEngine;

using LiveWorld.Mobs;
using System.Collections.Generic;

public class MobGeneratorExample : MonoBehaviour
{
    private Skeleton skeleton;

    private void Start()
    {
        //skeleton = GetSkeleton();

        //var shape = GenerationUtility.CreateShapePoints(skeleton);
        //var triangulation = GenerationUtility.Triangulation(shape);
        //var mesh = GenerationUtility.GetMesh(triangulation);

        if (!GetComponent<MeshFilter>())
            gameObject.AddComponent<MeshFilter>();
        if (!GetComponent<MeshRenderer>())
            gameObject.AddComponent<MeshRenderer>();

        //GetComponent<MeshFilter>().sharedMesh = mesh;

        GenerationStep();
    }

    private void GenerationStep()
    {
        skeleton = GenerationUtility.SkeletonFromTransform(transform);

        var shape = GenerationUtility.CreateShapePoints(skeleton);

        List<Vector3> vertices = new List<Vector3>(shape);
        List<Triangle> triangles = new List<Triangle>();

        triangles.Add(new Triangle(vertices[0], vertices[1], vertices[2]));

        for (int i = 0; i < 5; i++)
        {
            for (int index = 0; index < vertices.Count; index++)
            {
                Vector3 point = vertices[index];

                foreach (var triangle in triangles.FindAll(tringle => 
                    tringle.Contains(point, Orientation.XY) || 
                    tringle.Contains(point, Orientation.XZ) || 
                    tringle.Contains(point, Orientation.YZ)))
                {
                    triangles.Remove(triangle);
                    triangles.AddRange(triangle.Split(point));
                }
            }
        }

        var mesh = GenerationUtility.GetMesh(new TriangleCollection(triangles));

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void OnDrawGizmos()
    {
        bool drawPoints = true;
        bool drawBones = false;
        bool drawJoints = false;

        skeleton = GenerationUtility.SkeletonFromTransform(transform);

        if (drawJoints)
        {
            foreach (var joint in skeleton.GetJoints())
            {
                Gizmos.DrawSphere(transform.position + joint.localPosition, 0.25F);
            }
        }
        if (drawPoints)
        {
            foreach (var point in GenerationUtility.CreateShapePoints(skeleton))
            {
                Gizmos.DrawSphere(point, 0.1F);
            }
        }
        if (drawBones)
        {
            foreach (var bone in skeleton.GetBones())
            {
                if (skeleton.TryGetJoint(bone, out MobJoint from, out MobJoint to))
                {
                    Gizmos.DrawLine(transform.position + from.localPosition, transform.position + to.localPosition);
                }
            }
        }
    }
}
