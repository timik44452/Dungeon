using System;
using UnityEngine;

public class Health : MonoBehaviour, ISkillEffectListener
{
    public event Action OnWasted;
    public event Action OnHealthChanged;
    public event Action<float> OnDamaged;
    public event Action<float> OnHealing;

    public float initHealth = 100.0F;

    public float Value
    {
        get => safe_health.Value;
        set => safe_health.Value = value;
    }

    private Protection.SafeFloat safe_health;


    private void Start()
    {
        safe_health = new Protection.SafeFloat(initHealth);
    }

    public void Damage(float value)
    {
        if (value == 0)
        {
            return;
        }

        Value = Mathf.Max(Value - value, 0);

        OnDamaged?.Invoke(value);

        if (Value == 0)
        {
            OnWasted?.Invoke();
        }

        OnHealthChanged?.Invoke();
    }

    public void Healing(float value)
    {
        if (value == 0)
        {
            return;
        }

        Value = Mathf.Max(Value + value, 0);

        OnHealing?.Invoke(value);

        OnHealthChanged?.Invoke();
    }

    public void Invoke(Skill skill)
    {
        foreach (SkillEffect effect in skill.effects)
        {
            if (effect is DamageEffect)
            {
                var damageEffect = (DamageEffect)effect;

                Damage(damageEffect.damage);
            }
        }
    }
}
