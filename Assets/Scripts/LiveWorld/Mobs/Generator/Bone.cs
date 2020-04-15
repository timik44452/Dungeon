namespace LiveWorld.Mobs
{
    public class Bone
    {
        public float length;
        public string fromJointName;
        public string toJointName;

        public Bone(float length, string fromJointName, string toJointName)
        {
            this.length = length;
            this.fromJointName = fromJointName;
            this.toJointName = toJointName;
        }
    }
}
