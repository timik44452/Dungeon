using UnityEngine;

namespace LiveWorld.Mobs
{
    public class MobJoint
    {
        public string Name;
        public Vector3 localPosition;

        public MobJoint(string Name, Vector3 localPosition)
        {
            this.Name = Name;
            this.localPosition = localPosition;
        }
    }
}
