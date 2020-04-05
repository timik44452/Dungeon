using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace LiveWorld.Mobs
{
    public class Skeleton
    {
        public Vector3 center { get; private set; }

        private List<MobJoint> joints;
        private List<Bone> bones;

        public Skeleton()
        {
            joints = new List<MobJoint>();
            bones = new List<Bone>();
        }

        public void AddJoint(MobJoint joint)
        {
            joints.Add(joint);

            RecalculateCenter();
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
            float centerX = joints.Average(x => x.localPosition.x);
            float centerY = joints.Average(x => x.localPosition.x);
            float centerZ = joints.Average(x => x.localPosition.x);

            center = new Vector3(centerX, centerY, centerZ);
        }
    }
}