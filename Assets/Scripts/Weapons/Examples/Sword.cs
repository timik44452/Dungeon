using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Sword")]
public class Sword : Weapon
{
    public float attackDistance = 1.0F;

    public override void BeginInvoke(Component sender, ITarget target, Skill skill)
    {
        if (TargetSystem.ITargetIsNull(target))
        {
            return;
        }

        float distance = Vector3.Distance(sender.transform.position, target.transform.position);    

        if(distance > attackDistance)
        {
            return;
        }

        foreach (var effectListener in target.gameObject.GetComponents<ISkillEffectListener>())
        {
            effectListener.EffectInvoke(sender, effects);
        }
    }
}
