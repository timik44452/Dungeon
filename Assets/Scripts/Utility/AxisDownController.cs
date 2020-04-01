using System;

using UnityEngine;

[Serializable]
public class AxisDownController
{
    public event Action OnDownEvent;
    public event Action OnUpEvent;

    public int pressCount { get => m_pressCount; }
    public float time { get => m_time; }

    private float m_axisValue;
    private float m_sign;
    private float m_time;
    private float m_resetTimer;
    private int m_pressCount;

    public void Update(float value, float deltaTime)
    {
        float dh = m_axisValue - Mathf.Abs(value);
        float sign = Mathf.Sign(dh);

        if (dh != 0 && sign != m_sign)
        {
            if (sign < 0)
            {
                m_pressCount++;
                m_resetTimer = 0.2F;

                OnDownEvent?.Invoke();
            }
            else
            {
                OnUpEvent?.Invoke();
            }

            m_sign = sign;
        }

        m_time = (m_axisValue > 0) ? m_time + deltaTime : 0;
        m_resetTimer = Mathf.Clamp01(m_resetTimer - deltaTime);

        if(m_resetTimer == 0)
        {
            m_pressCount = 0;
        }

        m_axisValue = Mathf.Abs(value);
    }
}