using UnityEngine;

using LiveWorld.Mobs;

public class MobGeneratorExample : MonoBehaviour
{
    public MobConfiguration configuration;

    private Skeleton skeleton;

    private void Start()
    {
        skeleton = GetSkeleton();

        var shape = GenerationUtility.CreateShapePoints(configuration, skeleton);
        var triangulation = GenerationUtility.Triangulation(shape);
        var mesh = GenerationUtility.GetMesh(triangulation);

        if (!GetComponent<MeshFilter>())
            gameObject.AddComponent<MeshFilter>();
        if (!GetComponent<MeshRenderer>())
            gameObject.AddComponent<MeshRenderer>();

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }


    private Skeleton GetSkeleton()
    {
        Skeleton skeleton = new Skeleton();

        foreach (var joint in transform.GetComponentsInChildren<Transform>())
        {
            if (joint == transform)
            {
                continue;
            }

            skeleton.AddJoint(new MobJoint(joint.name, joint.position));

            if (joint.parent != transform)
            {
                Bone bone = new Bone(joint.parent.name, joint.name);

                skeleton.AddBone(bone);
            }
        }

        //skeleton.NormalizeJoints();

        return skeleton;
    }

    private void OnDrawGizmos()
    {
        skeleton = GetSkeleton();

        foreach (var joint in skeleton.GetJoints())
        {
            Gizmos.DrawSphere(transform.position + joint.localPosition, 0.25F);
        }

        foreach (var bone in skeleton.GetBones())
        {
            if (skeleton.TryGetJoint(bone, out MobJoint from, out MobJoint to))
            {
                Gizmos.DrawLine(transform.position + from.localPosition, transform.position + to.localPosition);
            }
        }
    }
}
