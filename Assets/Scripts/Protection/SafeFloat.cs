namespace Protection
{
    public class SafeFloat
    {
        public float Value
        {
            get => get_value();
            set => set_value(value);
        }

        private float offset;
        private float value;

        public SafeFloat(float value)
        {
            set_value(value);

            offset = UnityEngine.Random.value;
        }

        private float get_value()
        {
            return value - offset;
        }

        private void set_value(float value)
        {
            this.value = value + offset;
        }

        public static SafeFloat operator + (SafeFloat a, SafeFloat b)
        {
            return new SafeFloat(a.Value + b.Value);
        }
    }
}