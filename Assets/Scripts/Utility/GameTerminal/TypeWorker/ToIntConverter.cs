using System;

namespace Utility.GameTerminal.TypeWorker
{
    public class ToIntConverter : TypeConverter
    {
        public override Type OutputType => typeof(int);

        /// <summary>
        /// Avilable convertors: float, double, byte, bool, string
        /// </summary>
        protected override bool CanConvert(object value, out object result)
        {
            if (value is string && int.TryParse(value.ToString(), out int int_result))
            {
                result = int_result;

                return true;
            }

            if (value is float || value is double || value is byte)
            {
                result = (int)value;

                return true;
            }

            if (value is bool)
            {
                result = ((bool)value) ? 1 : 0;

                return true;
            }

            result = 0;

            return false;
        }
    }
}
