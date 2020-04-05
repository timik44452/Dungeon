using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Gun")]
public class Gun : Weapon
{
    public override void Invoke(Component sender, ITarget target, Skill skill)
    {
        if (TargetSystem.ITargetIsNull(target))
        {
            return;
        }

        foreach (var effectListener in target.gameObject.GetComponents<ISkillEffectListener>())
        {
            effectListener.EffectInvoke(sender, effects);
        }
    }
}
