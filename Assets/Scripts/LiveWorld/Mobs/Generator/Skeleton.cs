using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace LiveWorld.Mobs
{
    public class Skeleton
    {
        public Vector3 center
        {
            get
            {
                RecalculateCenter();

                return m_center;
            }
        }

        private List<MobJoint> joints;
        private List<Bone> bones;

        private Vector3 m_center;

        public Skeleton()
        {
            joints = new List<MobJoint>();
            bones = new List<Bone>();
        }

        public void AddJoint(MobJoint joint)
        {
            joints.Add(joint);
        }

        public void AddBone(Bone bone)
        {
            bones.Add(bone);
        }

        public IEnumerable<Bone> GetBones()
        {
            return bones;
        }

        public IEnumerable<MobJoint> GetJoints()
        {
            return joints;
        }

        public void NormalizeJoints()
        {
            float maximalJointLength = joints.Max(x => Vector3.Distance(center, x.localPosition));

            if (maximalJointLength > 1.0F)
            {
                foreach(var joint in joints)
                {
                    joint.localPosition /= maximalJointLength;
                }
            }
        }

        public bool TryGetJoint(string name, out MobJoint joint)
        {
            joint = joints.Find(x => x.Name == name);

            return joint != null;
        }

        public bool TryGetJoint(Bone bone, out MobJoint from, out MobJoint to)
        {
            to = null;
            from = null;

            foreach (MobJoint joint in joints)
            {
                if (joint.Name == bone.toJointName) to = joint;
                if (joint.Name == bone.fromJointName) from = joint;
            }

            return from != null && to != null;
        }

        public void RecalculateCenter()
        {
            m_center = Vector3.zero;

            if (joints.Count > 0)
            {
                foreach (var joint in joints)
                    m_center += joint.localPosition;

                m_center /= joints.Count;
            }
        }
    }
}