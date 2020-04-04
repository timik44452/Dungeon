namespace LiveWorld.Mobs
{
    public class MobProfile
    {
        public Skeleton currentSkeleton { get; }
        public MobConfiguration configuration { get; }

        public MobProfile(MobConfiguration configuration, Skeleton skeleton)
        {
            this.currentSkeleton = skeleton;
            this.configuration = configuration;
        }
    }
}
