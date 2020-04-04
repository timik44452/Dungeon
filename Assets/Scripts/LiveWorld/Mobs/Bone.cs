namespace LiveWorld.Mobs
{
    public class Bone
    {
        public string fromJointName;
        public string toJointName;

        public Bone(string fromJointName, string toJointName)
        {
            this.fromJointName = fromJointName;
            this.toJointName = toJointName;
        }
    }
}
