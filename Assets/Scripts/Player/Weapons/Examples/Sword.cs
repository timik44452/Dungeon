using UnityEngine;

public class Sword : Weapon
{
    public float attackDistance = 1.0F;

    private void Start()
    {
        defaultSkill = ResourceUtility.skillDatabase.GetSkill("sword");    
    }

    public override void Invoke(ITarget target, Skill skill)
    {
        if(target == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);    

        if(distance > attackDistance)
        {
            return;
        }

        Skill attackSkill = Skill.CombineSkills(defaultSkill, skill);

        foreach (var effectListener in target.gameObject.GetComponents<ISkillEffectListener>())
        {
            effectListener.Invoke(attackSkill);
        }
    }
}
