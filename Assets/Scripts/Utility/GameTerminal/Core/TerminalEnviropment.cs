using System;
using System.Collections.Generic;

using Utility.GameTerminal.TypeWorker;

namespace Utility.GameTerminal
{
    public class TerminalEnviropment
    {
        private Dictionary<string, TerminalEnviropmentVariable> variables;
        private List<TypeConverter> typeConverters;

        public TerminalEnviropment()
        {
            typeConverters = new List<TypeConverter>()
            {
                new ToIntConverter(),
                new ToBoolConverter()
            };

            variables = new Dictionary<string, TerminalEnviropmentVariable>();
        }

        public void ChangeValue(string name, object value)
        {
            if (variables.ContainsKey(name))
            {
                TerminalEnviropmentVariable variable = variables[name];

                if (CanConverteType(value.GetType(), variable.PropertyType, value, out object result))
                {
                    variable.Value = result;
                }
                else
                {
                    throw new Exception($"Invalid input data {value} cannot be converted to {variable.PropertyType}");
                }
            }
            else
            {
                throw new Exception($"Variable {name} hasn't finded");
            }
        }

        public void Invoke(string name)
        {
            if (variables.ContainsKey(name))
            {
                TerminalEnviropmentVariable variable = variables[name];

                if (variable.IsMethod)
                {
                    variable.Value = null;
                }
                else
                {
                    throw new Exception($"{name} is a variable but used as a method");
                }
            }
            else
            {
                throw new Exception($"Method {name} hasn't finded");
            }
        }

        public void AddMethod(string name, Action callback)
        {
            TerminalEnviropmentVariable variable;

            if (variables.ContainsKey(name))
            {
                variable = variables[name];
            }
            else
            {
                variable = new TerminalEnviropmentVariable(typeof(void));
                variables.Add(name, variable);
            }

            variable.RegisterCallback(callback);
        }

        public void AddVariable<T>(string name, Action<T> changeCallback)
        {
            TerminalEnviropmentVariable variable;

            if (variables.ContainsKey(name))
            {
                variable = variables[name];
            }
            else
            {
                variable = new TerminalEnviropmentVariable(typeof(T));
                variables.Add(name, variable);
            }

            variable.RegisterCallback(() => changeCallback?.Invoke((T)variable.Value));
        }

        public void AddVariable<T>(string name, object currentValue, Action<T> changeCallback)
        {
            TerminalEnviropmentVariable variable;

            if (variables.ContainsKey(name))
            {
                variable = variables[name];
            }
            else
            {
                variable = new TerminalEnviropmentVariable(currentValue, typeof(T));
                variables.Add(name, variable);
            }

            variable.RegisterCallback(() => changeCallback?.Invoke((T)variable.Value));
        }

        private bool CanConverteType(Type inputType, Type outputType, object input, out object output)
        {
            if (inputType == outputType)
            {
                output = input;

                return true;
            }

            var converter = typeConverters.Find(x => x.OutputType == outputType);

            if (converter != null)
            {
                return converter.CanConverted(input, out output);
            }

            output = null;

            return false;
        }
    }
}
