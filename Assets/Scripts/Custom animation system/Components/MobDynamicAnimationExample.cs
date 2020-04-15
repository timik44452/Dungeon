using LiveWorld.Mobs;
using System.Collections.Generic;
using UnityEngine;


public class MobDynamicAnimationExample : MonoBehaviour
{
    private Skeleton skeleton;
    
    private MobJoint currentJoint;

    private Dictionary<string, Transform> joints;

    public Vector3 centerOfMass
    {
        get => (skeleton != null) ? skeleton.center : Vector3.zero; 
    }

    public Vector3 footCenterOfMass
    {
        get
        {
            Vector3 center = Vector3.zero;

            if (skeleton != null)
            {
                int count = 0;

                foreach (var joint in skeleton.GetJoints())
                {
                    center += joint.localPosition;
                    count++;
                }

                if (count > 0)
                {
                    center /= count;
                }
            }

            return center;
        }
    }


    private const float dampingCoef = 0.05F;

    private void Awake()
    {
        joints = new Dictionary<string, Transform>();
        skeleton = GenerationUtility.SkeletonFromTransform(transform);

        foreach (var joint in skeleton.GetJoints())
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            gameObject.name = joint.Name;
            gameObject.transform.localScale = Vector3.one * 0.25F;

            joints.Add(joint.Name, gameObject.transform);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (skeleton.TryGetJoint(hit.collider.name, out MobJoint joint))
            {
                currentJoint = joint;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            currentJoint = null;
        }

        if (currentJoint != null)
        {
            Vector3 offset =
                Camera.main.transform.up * Input.GetAxis("Mouse Y") +
                Camera.main.transform.right * Input.GetAxis("Mouse X");

            UpdatePosition(currentJoint.localPosition + offset * 0.2F, currentJoint);
        }

        foreach (var joint in skeleton.GetJoints())
        {
            var currentJoint = joints[joint.Name];
            currentJoint.position = Vector3.Lerp(currentJoint.position, transform.position + joint.localPosition, 15F * Time.deltaTime);
        }
    }

    public Transform GetJoint(string name)
    {
        if (joints.ContainsKey(name))
            return joints[name];

        return null;
    }

    public void UpdatePosition(Vector3 position, Transform transform)
    {
        if (skeleton.TryGetJoint(transform.name, out MobJoint joint))
        {
            UpdatePosition(position, joint);
        }
    }

    public void UpdatePosition(Vector3 position, MobJoint currentJoint, List<string> exceptions = null)
    {
        if (exceptions == null)
        {
            exceptions = new List<string>();
        }

        if (exceptions.Contains(currentJoint.Name))
        {
            return;
        }

        exceptions.Add(currentJoint.Name);

        currentJoint.localPosition = position;

        foreach (var bone in skeleton.GetBones())
        {
            MobJoint secondJoint;

            if (bone.fromJointName == currentJoint.Name && skeleton.TryGetJoint(bone.toJointName, out secondJoint) ||
                bone.toJointName == currentJoint.Name && skeleton.TryGetJoint(bone.fromJointName, out secondJoint))
            {
                float distance = Vector3.Distance(currentJoint.localPosition, secondJoint.localPosition);

                if (Mathf.Abs(distance - bone.length) > bone.length * dampingCoef)
                {
                    Vector3 _direction = (currentJoint.localPosition - secondJoint.localPosition).normalized;

                    UpdatePosition(currentJoint.localPosition - _direction * bone.length, secondJoint);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            skeleton = GenerationUtility.SkeletonFromTransform(transform);
        }

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
