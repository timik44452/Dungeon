using System;

namespace Utility.GameTerminal.TypeWorker
{
    public class ToBoolConverter : TypeConverter
    {
        public override Type OutputType => typeof(bool);

        /// <summary>
        /// Avilable convertors: float, double, byte, string
        /// </summary>
        protected override bool CanConvert(object value, out object result)
        {
            if (value is string)
            {
                if (bool.TryParse(value.ToString(), out bool bool_result))
                {
                    result = bool_result;

                    return true;
                }
                else if (int.TryParse(value.ToString(), out int int_result))
                {
                    value = int_result;
                }
            }

            if (value is int || value is float || value is double || value is byte)
            {
                if ((int)value == 0)
                {
                    result = false;

                    return true;
                }
                else if ((int)value == 1)
                {
                    result = true;

                    return true;
                }
            }

            result = 0;

            return false;
        }
    }
}
