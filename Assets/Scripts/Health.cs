using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, ISkillEffectListener
{
    public event UnityEventHandler OnWasted;
    public event UnityEventHandler OnHealthChanged;
    public event UnityEventHandler<float> OnDamaged;
    public event UnityEventHandler<float> OnHealing;

    public float initHealth = 100.0F;

    public float Value
    {
        get => safe_health.Value;
        set => safe_health.Value = value;
    }

    private Protection.SafeFloat safe_health;


    private void Awake()
    {
        safe_health = new Protection.SafeFloat(initHealth);
    }

    public void Damage(Component sender, float value)
    {
        if (value == 0)
        {
            return;
        }

        Value = Mathf.Max(Value - value, 0);

        OnDamaged?.Invoke(sender, value);

        if (Value == 0)
        {
            OnWasted?.Invoke(sender);
        }

        OnHealthChanged?.Invoke(sender);
    }

    public void Healing(Component sender, float value)
    {
        if (value == 0)
        {
            return;
        }

        Value = Mathf.Max(Value + value, 0);

        OnHealing?.Invoke(sender, value);

        OnHealthChanged?.Invoke(sender);
    }

    public void EffectInvoke(Component sender, IEnumerable<SkillEffect> effects)
    {
        foreach (SkillEffect effect in effects)
        {
            if (TryGetEffect(effect, out DamageEffect damageEffect))
            {
                Damage(sender, damageEffect.damage);
            }

            if (TryGetEffect(effect, out HealEffect healEffect))
            {
                Healing(sender, healEffect.heal);
            }
        }
    }

    private bool TryGetEffect<T>(object value, out T effect) where T : SkillEffect
    {
        if (value is T)
        {
            effect = (T)value;

            return true;
        }

        effect = null;

        return false;
    }
}
