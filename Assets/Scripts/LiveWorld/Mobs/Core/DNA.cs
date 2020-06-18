using System.Collections.Generic;
using UnityEngine;

namespace LiveWorld.Mobs.Core
{
    public struct DNA
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

                m_hashCode = GetSTR(m_code).GetHashCode();
            }
        }

        private byte[] m_code;
        private int m_hashCode;

        private const int maximalCodeSize = 256;

        public DNA(params byte[] _code)
        {
            m_code = new byte[0];
            m_hashCode = 0;

            code = _code;
        }

        public static DNA Combine(DNA a, DNA b)
        {
            if (a.code.Length != b.code.Length)
                throw new System.Exception($"Debil nahui blyat odno {a.code.Length}, drugoe {b.code.Length}");

            List<byte> newCode = new List<byte>();

            for (int i = 0; i < a.code.Length; i++)
            {
                if (a.code[i] == b.code[i])
                    newCode.Add(a.code[i]);
            }

            newCode.CopyTo(a.code, 0);

            return new DNA(a.code);
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

        private string GetSTR(byte[] array)
        {
            string array_text = string.Empty;

            for (int index = 0; index < m_code.Length; index++)
                array_text += array[index];

            return array_text;
        }
    }
}
