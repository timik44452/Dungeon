using UnityEngine;

public class Gun : Weapon
{
    private void Start()
    {
        defaultSkill = ResourceUtility.skillDatabase.GetSkill("gun");
    }

    private void Update()
    {
        reloadingTimer -= Time.deltaTime;
    }

    public override void Invoke(ITarget target, Skill skill)
    {
        if(IsReloading || target == null)
        {
            return;
        }

        Skill attackSkill = Skill.CombineSkills(defaultSkill, skill);

        foreach (var effectListener in target.gameObject.GetComponents<ISkillEffectListener>())
        {
            effectListener.EffectInvoke(this, attackSkill.effects);
        }

        reloadingTimer = attackSkill.reloadingTime;
    }
}
