using UnityEngine;

namespace LiveWorld.Mobs.Core
{
    public class MobConfiguration
    {
        private enum SenseCombination
        {
            Hearing_Smell,
            Smell_Eye,
            Eye_Hearing
        }

        public float height { get; set; } = 1.0F;
        public float musculeMass { get; set; } = 1.0F;
        public float fieldOfView { get; set; } = 45;

        public float hearingPower
        {
            get => m_hearingPower;
            set
            {
                m_hearingPower = Mathf.Clamp01(value);
                RecalculateSense(SenseCombination.Smell_Eye);
            }
        }
        public float smellPower
        {
            get => m_smellPower;
            set
            {
                m_smellPower = Mathf.Clamp01(value);
                RecalculateSense(SenseCombination.Eye_Hearing);
            }
        }
        public float eyePower
        {
            get => m_eyePower;
            set
            {
                m_eyePower = Mathf.Clamp01(value);
                RecalculateSense(SenseCombination.Hearing_Smell);
            }
        }

        private float m_hearingPower = 0.0F;
        private float m_smellPower = 0.0F;
        private float m_eyePower = 0.0F;

        private void RecalculateSense(SenseCombination sense)
        {
            // sense = sense / (sense_a + sense_b) * (1.0F - changedSense)
            switch (sense)
            {
                case SenseCombination.Eye_Hearing:
                    {
                        m_eyePower = m_eyePower / (m_eyePower + m_hearingPower) * (1.0F - m_smellPower);
                        m_hearingPower = m_hearingPower / (m_eyePower + m_hearingPower) * (1.0F - m_smellPower);
                    }
                    break;
                case SenseCombination.Hearing_Smell:
                    {
                        m_smellPower = m_smellPower / (m_smellPower + m_hearingPower) * (1.0F - m_eyePower);
                        m_hearingPower = m_hearingPower / (m_smellPower + m_hearingPower) * (1.0F - m_eyePower);
                    }
                    break;
                case SenseCombination.Smell_Eye:
                    {
                        m_eyePower = m_eyePower / (m_smellPower + m_eyePower) * (1.0F - m_hearingPower);
                        m_smellPower = m_smellPower / (m_smellPower + m_eyePower) * (1.0F - m_hearingPower);
                    }
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }

    }
}
