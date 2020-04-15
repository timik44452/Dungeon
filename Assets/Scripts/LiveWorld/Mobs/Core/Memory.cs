namespace LiveWorld.Mobs.Core
{
    public class Memory
    {
        public float lastActionType;
        public Feeling feeling;

        public Memory(Feeling feeling)
        {
            this.feeling = feeling;
        }
    }
}
