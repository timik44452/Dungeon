using System;

namespace Utility.GameTerminal
{
    public class TerminalEnviropmentVariable
    {
        private event Action OnPropertyChanged;

        public bool IsMethod
        {
            get => PropertyType == typeof(void);
        }
        public object Value
        {
            get => m_value;
            set
            {
                m_value = value;
                OnPropertyChanged?.Invoke();

                is_changed = true;
            }
        }
        public Type PropertyType { get; }

        private bool is_changed;
        private object m_value;

        public TerminalEnviropmentVariable(Type type)
        {
            PropertyType = type;
        }

        public TerminalEnviropmentVariable(object currentValue, Type type)
        {
            PropertyType = type;
            m_value = currentValue;
        }

        public void RegisterCallback(Action action)
        {
            if (!IsMethod && is_changed)
            {
                action.Invoke();
            }

            OnPropertyChanged += action;
        }
    }
}