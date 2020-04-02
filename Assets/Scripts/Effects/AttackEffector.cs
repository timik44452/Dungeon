using System.Collections.Generic;
using UnityEngine;

public class AttackEffector : MonoBehaviour, ISkillEffectListener
{
    public GameObject effect; 

    public void EffectInvoke(Component sender, IEnumerable<SkillEffect> effects)
    {
        if (effect == null)
        {
            return;
        }

        foreach (SkillEffect effect in effects)
        {
            if (effect is DamageEffect)
            {
                Instantiate(this.effect, transform);
            }
        }
    }
}
