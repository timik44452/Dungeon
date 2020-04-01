namespace Player.Movement
{
    [System.Serializable]
    public class PlayeMovementConfiguration
    {
        public float walkSpeed = 1.0F;
        public float runSpeed = 2.0F;

        public float jerkPower = 1.0F;
        public float jumpPower = 2.0F;
        public float gravity = 0.098F;
    }
}
