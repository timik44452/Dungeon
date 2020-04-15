using UnityEngine;

namespace LiveWorld.Mobs
{
    [System.Serializable]
    public class MobJoint
    {
        public string Name;
        public Vector3 localPosition;

        public bool foot { get; }

        public MobJoint(string Name, Vector3 localPosition, bool foot = false)
        {
            this.Name = Name;
            this.localPosition = localPosition;
            this.foot = foot;
        }
    }
}
