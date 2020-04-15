using System.Collections.Generic;

namespace LiveWorld.Mobs.Core
{
    public class DNA
    {
        public byte[] code
        {
            get => m_code;
            set
            {
                if(value.Length > maximalCodeSize)
                {
                    m_code = new byte[maximalCodeSize];
                    value.CopyTo(m_code, 0);
                }
                else
                {
                    m_code = value;
                }

                string array_text = string.Empty;

                for (int index = 0; index < m_code.Length; index++)
                    array_text += m_code[index];

                m_hashCode = array_text.GetHashCode();
            }
        }

        private byte[] m_code;
        private int m_hashCode;

        private const int maximalCodeSize = 8;

        public DNA(params byte[] code)
        {
            this.code = code;
        }

        public static DNA Combine(DNA a, DNA b)
        {
            throw new System.NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return obj is DNA dNA &&
                   m_hashCode == dNA.m_hashCode;
        }

        public override int GetHashCode()
        {
            return m_hashCode;
        }
    }
}
