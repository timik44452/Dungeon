using System;

namespace Utility.GameTerminal.TypeWorker
{
    public abstract class TypeConverter
    {
        public abstract Type OutputType { get; }

        public bool CanConverted(object value, out object result)
        {
            if (value == null)
            {
                result = false;
                return false;
            }

            if (value.GetType() == OutputType)
            {
                result = true;
                return true;
            }

            if (value is string)
            {
                value = ((string)value).Replace('.', ',').Trim();
            }

            return CanConvert(value, out result);
        }

        protected abstract bool CanConvert(object value, out object result);
    }
}
